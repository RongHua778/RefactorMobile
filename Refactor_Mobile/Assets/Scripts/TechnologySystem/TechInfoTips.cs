using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TechInfoTips : TileTips
{
    [SerializeField] GameObject abnormalText = default;
    [SerializeField] Material normalMat = default;
    [SerializeField] Material abnormalMat = default;
    [SerializeField] GameObject abnormalBtn = default;
    TechAttribute techAtt;
    Technology m_Tech;
    public void SetInfo(Technology tech,bool preview)
    {
        abnormalBtn.SetActive(preview);
        m_Tech = tech;
        techAtt = StaticData.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
        Name.text = GameMultiLang.GetTraduction(m_Tech.TechName);

        abnormalText.SetActive(tech.IsAbnormal);
        ShowInfo();
    }

    public void SwtichPreview()
    {
        m_Tech.IsAbnormal = !m_Tech.IsAbnormal;
        ShowInfo();
    }

    private void ShowInfo()
    {
        abnormalText.SetActive(m_Tech.IsAbnormal);
        Icon.material = m_Tech.IsAbnormal ? abnormalMat : normalMat;
        Icon.sprite = techAtt.Icon;
        Description.text = string.Format(m_Tech.TechnologyDes,
            "<b>" + m_Tech.DisplayValue1 + "</b>", "<b>" + m_Tech.DisplayValue2 + "</b>", "<b>" + m_Tech.DisplayValue3 + "</b>", "<b>" + m_Tech.DisplayValue4 + "</b>", "<b>" + m_Tech.DisplayValue5 + "</b>");

    }
}
