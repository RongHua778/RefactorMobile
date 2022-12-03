using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TileSelect : MonoBehaviour
{

    [SerializeField] RenderTexture renderTexture;
    public TileShape Shape { get; set; }
    [SerializeField] LevelDownSelect m_LevelDownSelect = default;
    [SerializeField] ElementSelectPreview m_ElementPreview = default;
    [SerializeField] ElementInfoBtn m_InfoBtn = default;
    [SerializeField] TextMeshProUGUI hasNumTxt = default;


    public void InitializeDisplay(int displayID, TileShape shape)
    {
        Shape = shape;
        shape.SetUIDisplay(displayID, renderTexture);
        StrategyBase strategy = Shape.m_ElementTurret.Strategy;
        if (strategy.Quality > 1)
        {
            m_LevelDownSelect.SetStrategy(strategy);
            m_LevelDownSelect.gameObject.SetActive(true);
        }
        else
        {
            m_LevelDownSelect.gameObject.SetActive(false);
        }
        m_InfoBtn.SetStrategy(strategy);
        m_ElementPreview.SetStrategy(strategy);
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
        GameManager.Instance.SelectShape(Shape,levelDown);
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


}
