using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelDownSelect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//,IPointerEnterHandler,IPointerExitHandler
{
    public StrategyBase m_Strategy;
    private TileSelect m_TileSelect;
    private float holdCounter;
    private bool isHolding = false;
    private bool isPreview = false;
    public void SetStrategy(StrategyBase strategy, TileSelect tileSelect)
    {
        m_TileSelect = tileSelect;
        m_Strategy = new StrategyBase(strategy.Attribute, strategy.Quality - 1);
        m_Strategy.SetQualityValue();
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
        Debug.Log("Down");
        GameManager.Instance.PreviewComposition(true, m_Strategy.Attribute.element, m_Strategy.Quality);
        isPreview = false;
        isHolding = true;
        holdCounter = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        if (!isPreview)
        {
            m_TileSelect.OnShapeClick(true);
        }
        GameManager.Instance.PreviewComposition(false);
        isHolding = false;
        isPreview = false;
    }


}
