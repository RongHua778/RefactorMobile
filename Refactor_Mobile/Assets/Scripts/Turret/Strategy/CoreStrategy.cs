using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreStrategy : RefactorStrategy
{
    public CoreStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }

    //public int CoreExtraSlot => GameRes.CoreExtraSlot > 0 ? 1 : 0;
    //public override int TempExtraSlot { get => Mathf.Max(base.TempExtraSlot, CoreExtraSlot); }


    //public override void OnEquipped()
    //{
    //    if (TurretSkills.Count > 5) //5技能塔
    //    {
    //        PrivateExtraSlot++;//获得第5个额外技能槽
    //        GameRes.CoreExtraSlot++;//恢复1个核心机4技能名额
    //        GameRes.GlobalExtraSlot--;//消耗5技能名额
    //    }
    //    else
    //    if (TurretSkills.Count > 4) //4技能塔
    //    {
    //        if (GameRes.CoreExtraSlot > 0)//优先消耗核心机技能
    //        {
    //            GameRes.CoreExtraSlot--;//消耗1个核心机名额
    //            PrivateExtraSlot++;
    //        }
    //        else if (GameRes.GlobalExtraSlot > 0)//没有核心机名额，消耗全局名额
    //        {
    //            GameRes.GlobalExtraSlot--;
    //            PrivateExtraSlot += GameRes.GlobalExtraSlotValue;//获得1或2个额外技能槽
    //        }
    //    }

    //}

}
