using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretItemSlot : ItemSlot
{
    StrategyBase m_Strategy;

    public override void SetContent(ContentAttribute attribute, ToggleGroup group)
    {
        base.SetContent(attribute, group);
        switch (((TurretAttribute)attribute).StrategyType)
        {
            case StrategyType.Element:
                m_Strategy = new ElementStrategy((TurretAttribute)attribute, 5);
                break;
            case StrategyType.Composite:
                m_Strategy = new RefactorStrategy((TurretAttribute)attribute, 3);
                break;
            //case StrategyType.Building:
            //    m_Strategy = new BuildingStrategy((TurretAttribute)attribute, 1, false);
            //    break;
        }
    }
    public override void OnItemSelect(bool value)
    {
        base.OnItemSelect(value);
        if (isLock)
            return;
        if (value)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            TipsManager.Instance.ShowTurreTips(m_Strategy, screenPoint.x > Screen.width / 2 ?
                StaticData.LeftTipsPos : StaticData.RightTipsPos, 1);
        }
        else
        {
            TipsManager.Instance.HideTips();

        }
    }


}
