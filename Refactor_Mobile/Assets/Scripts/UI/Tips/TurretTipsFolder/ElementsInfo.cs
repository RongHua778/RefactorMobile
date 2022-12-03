using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementsInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public StrategyBase m_Strategy;
    [SerializeField] ElementBenefitPanel benefitPanel = default;
    public void OnPointerEnter(PointerEventData eventData)
    {
        benefitPanel.InitializePanel(m_Strategy);
        benefitPanel.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        benefitPanel.Hide();
    }
}
