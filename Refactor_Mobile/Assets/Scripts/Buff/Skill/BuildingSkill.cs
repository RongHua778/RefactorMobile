using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingSkillName
{
    None, VAULTSKILL, AMPLIFIER, PRISM,TELEPORTOR,BOUNTY

}
public abstract class BuildingSkill : TurretSkill
{
    public abstract BuildingSkillName BuildingSkillName { get; }
    public virtual bool IsAbnormalBuilding { get; set; }
    public virtual void MainFuncCallBack() { }
}

public class VaultSkill : BuildingSkill
{
    public override BuildingSkillName BuildingSkillName => BuildingSkillName.VAULTSKILL;

    public override ElementType IntensifyElement => ElementType.None;
    private int gainedAmount;
    private int successiveTurn = 0;
    public override float KeyValue => 20 + strategy.TotalElementCount;
    public override float KeyValue2 => 5 + strategy.TotalElementCount / 3;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(((int)KeyValue).ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(((int)KeyValue2).ToString());
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(gainedAmount.ToString());

    public override void EndTurn()
    {
        base.EndTurn();

        gainedAmount += (int)KeyValue + (int)KeyValue2 * Mathf.Min(5, successiveTurn);
        successiveTurn++;
    }

    public override void MainFuncCallBack()
    {
        GameRes.Coin += gainedAmount;
        successiveTurn = 0;
    }

}



//public class PrismSkill : BuildingSkill
//{
//    public override BuildingSkillName BuildingSkillName => BuildingSkillName.PRISM;
//    public override string DisplayValue2 =>  "X%";

//    private float FrostInterval => 20f;
//    private float FrostTime => 10f;

//    public override void Build()
//    {
//        base.Build();
//        ElementSkill effect = TurretSkillFactory.GetElementSkill(new List<int> { 11, 11, 11 });
//        strategy.AddElementSkill(effect);
//    }

//    public override void StartTurn2()
//    {
//        base.StartTurn2();
//        if (IsAbnormalBuilding)
//        {
//            Duration += FrostInterval;
//            //FrostTurrets();
//        }
//    }
//    public override void TickEnd()
//    {
//        base.TickEnd();
//        if (IsAbnormalBuilding)
//        {
//            FrostTurrets();
//            Duration += FrostInterval;
//            IsFinish = false;
//        }
//    }

//    private void FrostTurrets()
//    {
//        StaticData.Instance.FrostTurretEffect(strategy.Concrete.transform.position, 1.6f, FrostTime);
//    }




//}


