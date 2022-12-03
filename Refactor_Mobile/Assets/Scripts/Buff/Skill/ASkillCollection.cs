using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoubleAttack : ElementSkill
{
    //24K»ù×ù
    public override List<int> InitElements => new List<int> { 0, 0, 0 };

    public override float KeyValue => GameRes.CurrentWave;
    public override float KeyValue2 => 60f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("X");

    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("ATBATTLESTART"));
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());


    private bool triggered = false;
    public override void StartTurn2()
    {
        if (!triggered)
        {
            base.StartTurn2();
            Duration = Mathf.Min(KeyValue, KeyValue2);
            triggered = true;
        }

    }

    public override void TickEnd()
    {
        base.TickEnd();

        strategy.StartTurnSkills();
        strategy.StartTurnSkill2();
        strategy.StartTurnSkill3();

    }

    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
    }

    //private bool attacking => strategy.Concrete.targetList.Count > 0;
    //private bool intensified;
    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    Duration = 9999;
    //}

    //public override void Tick(float delta)
    //{
    //    if (attacking)
    //    {
    //        if (!intensified)
    //        {
    //            StrategyBase.AllDamageBonusCount += 1;
    //            intensified = true;
    //        }

    //    }
    //    else
    //    {
    //        if (intensified)
    //        {
    //            StrategyBase.AllDamageBonusCount -= 1;
    //            intensified = false;
    //        }

    //    }
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    intensified = false;
    //}

    //public override float KeyValue => 4;
    //public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    //public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized("1");

    //public override void StartTurn2()
    //{
    //    strategy.TurnFixRange += strategy.TotalElementCount / (int)KeyValue;
    //    strategy.Turret.GenerateRange();
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();

    //    strategy.Turret.GenerateRange();
    //}
}

public class HitAttack : ElementSkill
{
    //
    public override List<int> InitElements => new List<int> { 1, 1, 0 };
    public override float KeyValue => 0.15f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFrostResist += KeyValue * strategy.GoldCount;
    }


}

public class TimeAttack : ElementSkill
{
    //´¢ÄÜ¹¥»÷
    public override List<int> InitElements => new List<int> { 2, 2, 0 };
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
        buffInfo.Stacks = Mathf.RoundToInt(strategy.GoldCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return damage;
    }

}

public class CloseAttack : ElementSkill
{

    public override List<int> InitElements => new List<int> { 3, 3, 0 };

    public override float KeyValue => 1;
    public override float KeyValue2 => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixRange += (int)KeyValue * (strategy.GoldCount / (int)KeyValue2);
        strategy.Concrete.GenerateRange();

    }

    public override void EndTurn()
    {
        base.EndTurn();
        strategy.Concrete.GenerateRange();
    }


}
public class ConAttack : ElementSkill
{

    public override List<int> InitElements => new List<int> { 4, 4, 0 };
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
        buffInfo.Stacks = Mathf.RoundToInt(strategy.GoldCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return base.Hit(damage, target, bullet);
    }


}








