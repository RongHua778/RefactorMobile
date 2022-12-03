using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class TechSlot : ItemSlot
{

    private Technology m_Tech;


    public override void SetContent(ContentAttribute attribute, ToggleGroup group)
    {
        base.SetContent(attribute, group);
        m_Tech = TechnologyFactory.GetTech((int)((TechAttribute)attribute).TechName);

    }
    public override void OnItemSelect(bool value)
    {
        base.OnItemSelect(value);
        if (isLock)
            return;
        if (value)
        {
            TipsManager.Instance.ShowTechInfoTips(m_Tech, StaticData.RightTipsPos,true);
        }
        else
        {
            TipsManager.Instance.HideTips();

        }
    }
}
