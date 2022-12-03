using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image techIcon = default;
    private Technology m_Tech;
    public Technology MyTech => m_Tech;
    [SerializeField] GameObject abnormalIcon = default;
    TechAttribute techAtt;
    public void SetTechItem(Technology tech)
    {
        m_Tech = tech;
        techAtt = StaticData.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
        techIcon.sprite = techAtt.Icon;
        abnormalIcon.SetActive(m_Tech.IsAbnormal);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        TipsManager.Instance.ShowTechInfoTips(MyTech, StaticData.LeftTipsPos);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        TipsManager.Instance.HideTips();
    }

}
