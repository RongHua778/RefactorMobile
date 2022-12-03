using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ElementInfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    StrategyBase m_Strategy;
    [SerializeField] TextMeshProUGUI m_Txt = default;

    public void SetStrategy(StrategyBase strategy)
    {
        m_Strategy = strategy;
        m_Txt.text = StaticData.FormElementName(m_Strategy.Attribute.element, m_Strategy.Quality);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(true, m_Strategy.Attribute.element, m_Strategy.Quality);
        TipsManager.Instance.ShowTurreTips(m_Strategy, StaticData.LeftMidTipsPos, 2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.PreviewComposition(false);
        TipsManager.Instance.HideTips();
    }
}
