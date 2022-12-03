using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementSelectPreview : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image FrameSprite = default;
    [SerializeField] Color NormalColor = default;
    [SerializeField] Color HandleColor = default;
    StrategyBase m_Strategy;

    public void SetStrategy(StrategyBase strategy)
    {
        FrameSprite.color = NormalColor;
        m_Strategy = strategy;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, m_Strategy.Attribute.element, m_Strategy.Quality);
        //GameManager.Instance.ShowTurretTips(m_Strategy, StaticData.RightMidTipsPos);
        FrameSprite.color = HandleColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
        //GameManager.Instance.HideTips();
        FrameSprite.color = NormalColor;
    }
}
