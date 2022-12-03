using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorStrategy : RefactorStrategy
{


    public ConstructorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }

    //public int ConstructorEslot => GameRes.ConstructorExtraSlot > 0 ? 1 : 0;
    //public override int TempExtraSlot { get => Mathf.Max(base.TempExtraSlot, ConstructorEslot); }


    //public override void OnEquipped()
    //{
    //    if (TurretSkills.Count > 5) //5技能塔
    //    {
    //        PrivateExtraSlot++;//获得第5个额外技能槽
    //        GameRes.ConstructorExtraSlot++;//恢复1个加农炮4技能名额
    //        GameRes.GlobalExtraSlot--;//消耗5技能名额
    //    }
    //    else
    //    if (TurretSkills.Count > 4) //4技能塔
    //    {
    //        if (GameRes.ConstructorExtraSlot > 0)//优先消耗加农炮技能
    //        {
    //            GameRes.ConstructorExtraSlot--;//消耗1个加农炮名额
    //            PrivateExtraSlot++;
    //        }
    //        else if (GameRes.GlobalExtraSlot > 0)//没有加农炮名额，消耗全局名额
    //        {
    //            GameRes.GlobalExtraSlot--;
    //            PrivateExtraSlot+= GameRes.GlobalExtraSlotValue;//获得1或2个额外技能槽
    //        }
    //    }

    //}


}
