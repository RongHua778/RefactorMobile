using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TileSelect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] RenderTexture renderTexture;
    public TileShape Shape { get; set; }
    [SerializeField] LevelDownSelect m_LevelDownSelect = default;
    //[SerializeField] ElementInfoBtn m_InfoBtn = default;
    [SerializeField] TextMeshProUGUI hasNumTxt = default;
    [SerializeField] TextMeshProUGUI ElementTxt = default;

    [SerializeField] GameObject selectFrame = default;

    private float holdCounter;
    private bool isHolding = false;
    private bool isPreview = false;

    public void InitializeDisplay(int displayID, TileShape shape)
    {
        isHolding = false;
        Shape = shape;
        shape.SetUIDisplay(displayID, renderTexture);
        StrategyBase strategy = Shape.m_ElementTurret.Strategy;
        if (strategy.Quality > 1)
        {
            m_LevelDownSelect.SetStrategy(strategy, this);
            m_LevelDownSelect.gameObject.SetActive(true);
        }
        else
        {
            m_LevelDownSelect.gameObject.SetActive(false);
        }
        ElementTxt.text = StaticData.FormElementName(strategy.Attribute.element, strategy.Quality);
        selectFrame.SetActive(false);
        CheckElementNum();
    }
    private void CheckElementNum()
    {
        int amount = 0;
        foreach (var element in GameManager.Instance.elementTurrets.behaviors)
        {
            ElementTurret turret = element as ElementTurret;
            if (turret.Strategy.Attribute.element == Shape.m_ElementTurret.Strategy.Attribute.element
                && turret.Strategy.Quality == Shape.m_ElementTurret.Strategy.Quality)
            {
                amount++;
            }
        }
        hasNumTxt.text = GameMultiLang.GetTraduction("HAS") + ":" + amount;
    }
    public void OnShapeClick(bool levelDown = false)
    {
        GameManager.Instance.PreviewComposition(false);
        GameManager.Instance.SelectShape(Shape, levelDown);
        TipsManager.Instance.HideTips();
        GameEvents.Instance.TutorialTrigger(TutorialType.SelectShape);
        if (BluePrintGrid.SelectingGrid != null)
        {
            BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
        }
    }

    public void ClearShape()
    {
        if (Shape == null)
            return;
        Shape.ReclaimTiles();
        Shape = null;
    }
    private void Update()
    {
        if (isHolding)
        {
            if (holdCounter < 0.3f * GameRes.GameSpeed)
            {
                holdCounter += Time.deltaTime;
            }
            else
            {
                isPreview = true;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, Shape.m_ElementTurret.Strategy.Attribute.element, Shape.m_ElementTurret.Strategy.Quality);
        isPreview = false;
        isHolding = true;
        holdCounter = 0;
        selectFrame.SetActive(true);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPreview)
        {
            OnShapeClick(false);
        }
        GameManager.Instance.PreviewComposition(false);
        isHolding = false;
        isPreview = false;
        selectFrame.SetActive(false);

    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("Enter");
    //    if(!isPreview)
    //        GameManager.Instance.PreviewComposition(true, Shape.m_ElementTurret.Strategy.Attribute.element, Shape.m_ElementTurret.Strategy.Quality);
    //    isPreview = false;
    //    isHolding = true;
    //    holdCounter = 0f;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.Log("Exit");
    //    isPreview = false;
    //    holdCounter = 0;
    //    isHolding = false;
    //}
}
