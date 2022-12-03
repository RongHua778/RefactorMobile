using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class TrapItemSlot : ItemSlot//, IPointerEnterHandler, IPointerExitHandler
{

    public override void OnItemSelect(bool value)
    {
        base.OnItemSelect(value);

        if (isLock)
            return;
        if (value)
        {
            TipsManager.Instance.ShowTrapTips((TrapAttribute)contenAtt, StaticData.RightTipsPos);
        }
        else
        {
            TipsManager.Instance.HideTips();

        }
    }

}
