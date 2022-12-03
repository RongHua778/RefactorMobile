using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SameTarget : ElementSkill
{
    public override List<int> InitElements => new List<int> { 1, 1, 1 };

    public override float KeyValue => 1;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void StartTurn()
    {
        base.StartTurn();
        if(strategy.GoldCount>0)
            strategy.TempGoldCount += (int)KeyValue;
        if(strategy.WoodCount>0)
            strategy.TempWoodCount += (int)KeyValue;
        if(strategy.WaterCount>0)
            strategy.TempWaterCount += (int)KeyValue;
        if(strategy.FireCount>0)
            strategy.TempFireCount += (int)KeyValue;
        if(strategy.DustCount>0)
            strategy.TempDustCount += (int)KeyValue;

    }
}
public class CloseSpeed : ElementSkill
{

    //木之减速
    public override List<int> InitElements => new List<int> { 2, 2, 1 };
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));

    private BuffInfo buffInfo;
    public override void Build()
    {
        base.Build();
        buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1);

    }
    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        buffInfo.Stacks = Mathf.RoundToInt(strategy.WoodCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return damage;
    }

}

public class TimeSpeed : ElementSkill
{
    //木之强化
    public override List<int> InitElements => new List<int> { 0, 0, 1 };
    public override float KeyValue => 0.2f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixDamageBonus += KeyValue * strategy.WoodCount;
    }

}

public class FarSpeed : ElementSkill
{
    //爆发攻速
    public override List<int> InitElements => new List<int> { 3, 3, 1 };

    public override float KeyValue => 1;
    public override float KeyValue2 => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixRange += (int)KeyValue * (strategy.WoodCount / (int)KeyValue2);
        strategy.Concrete.GenerateRange();
    }

    public override void EndTurn()
    {
        base.EndTurn();
        strategy.Concrete.GenerateRange();
    }

}

public class ConFirerate : ElementSkill
{
    //数量攻速
    public override List<int> InitElements => new List<int> { 4, 4, 1 };
    public override float KeyValue => 1;
    public override float KeyValue2 => 1;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    BuffInfo buffInfo;
    public override void Build()
    {
        base.Build();
        buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1);
    }

    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        buffInfo.Stacks = Mathf.RoundToInt(strategy.WoodCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return base.Hit(damage, target, bullet);
    }

}








