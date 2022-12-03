using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlot : ItemSlot
{
    public override void OnItemSelect(bool value)
    {
        base.OnItemSelect(value);

        if (isLock)
            return;
        if (value)
        {
            TipsManager.Instance.ShowEnemyTips(new List<EnemyAttribute>() { (EnemyAttribute)this.contenAtt },StaticData.RightTipsPos);
        }
        else
        {
            TipsManager.Instance.HideTips();

        }
    }
}
