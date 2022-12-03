using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExtraSkill : ElementSkill
{
    public override List<int> InitElements => new List<int> { 2, 2, 2 };
    public override float KeyValue => 1f;
    public override float KeyValue2 => 0.15f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    private bool triggered = false;
    public override void StartTurn2()
    {
        base.StartTurn2();
        if (!triggered)
        {
            Duration = KeyValue;
            triggered = true;
        }

    }

    public override void TickEnd()
    {
        base.TickEnd();
        FrostEnemies();
        Duration += KeyValue;
        IsFinish = false;
    }

    private void FrostEnemies()
    {
        foreach (var target in strategy.Concrete.targetList)
        {
            target.Enemy.DamageStrategy.ApplyFrost(strategy.FinalSlowRate * KeyValue2);
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
    }

    //private List<IDamage> Enterlist;
    //public override float KeyValue => 5f;
    //public override float KeyValue2 => 30;

    //public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    //public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    //private bool triggered = false;
    //public override void StartTurn2()
    //{
    //    base.StartTurn2();
    //    if (!triggered)
    //    {
    //        Duration = KeyValue;
    //        triggered = true;
    //    }

    //}

    //public override void TickEnd()
    //{
    //    base.TickEnd();
    //    Duration = KeyValue;
    //    IsFinish = false;
    //    Enterlist.Clear();
    //}
    //public override void Build()
    //{
    //    base.Build();
    //    Enterlist = new List<IDamage>();
    //}

    //public override void OnEnter(IDamage target)
    //{
    //    base.OnEnter(target);
    //    if (Enterlist.Contains(target))
    //        return;
    //    Enterlist.Add(target);
    //    target.DamageStrategy.ApplyFrost(strategy.FinalSlowRate * KeyValue);
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    Enterlist.Clear();
    //}

}

public class CloseSlow : ElementSkill
{
    //近战冰冻
    public override List<int> InitElements => new List<int> { 1, 1, 2 };
    public override float KeyValue => 0.15f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFrostResist += KeyValue * strategy.WaterCount;
    }

}
public class HitSlow : ElementSkill
{
    //水之强化
    public override List<int> InitElements => new List<int> { 0, 0, 2 };
    public override float KeyValue => 0.2f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixDamageBonus += KeyValue * strategy.WaterCount;
    }
}

public class LongSlow : ElementSkill
{
    //爆发减速
    public override List<int> InitElements => new List<int> { 3, 3, 2 };
    public override float KeyValue => 1;
    public override float KeyValue2 => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixRange += (int)KeyValue * (strategy.WaterCount / (int)KeyValue2);
        strategy.Concrete.GenerateRange();

    }

    public override void EndTurn()
    {
        base.EndTurn();
        strategy.Concrete.GenerateRange();
    }
}

public class StartSlow : ElementSkill
{
    //数量冰冻
    public override List<int> InitElements => new List<int> { 4, 4, 2 };
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
        buffInfo.Stacks = Mathf.RoundToInt(strategy.WaterCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return base.Hit(damage, target, bullet);
    }

}







