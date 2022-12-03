using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineIntensify : ElementSkill
{
    //线性增幅
    public override List<int> InitElements => new List<int> { 13, 13, 11 };

    public override float KeyValue => 1;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override void Build()
    {
        base.Build();
        strategy.RangeType = RangeType.Line;
        if (strategy.Concrete != null)
            strategy.Concrete.GenerateRange();
    }
    public override void StartTurn2()
    {
        base.StartTurn2();
        if (strategy.RangeType == RangeType.Line)
            strategy.TurnFixDamageBonus += strategy.FinalBulletDamageIntensify;
    }
}

public class BalanceGrid : ElementSkill
{
    // 平衡矩阵
    public override List<int> InitElements => new List<int> { 10, 10, 11 };

    public override float KeyValue => 4;
    public override float KeyValue2 => 2;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());


    public override void StartTurn()
    {
        base.StartTurn();
        if (strategy.GoldCount < KeyValue)
        {
            strategy.TempGoldCount += (int)KeyValue2;
        }
        if (strategy.WoodCount < KeyValue)
        {
            strategy.TempWoodCount += (int)KeyValue2;
        }
        if (strategy.WaterCount < KeyValue)
        {
            strategy.TempWaterCount += (int)KeyValue2;
        }
        if (strategy.FireCount < KeyValue)
        {
            strategy.TempFireCount += (int)KeyValue2;
        }
        if (strategy.DustCount < KeyValue)
        {
            strategy.TempDustCount += (int)KeyValue2;
        }
    }
}

public class VulunScan : ElementSkill
{
    // 终途子弹
    public override List<int> InitElements => new List<int> { 13, 13, 13 };

    public override float KeyValue => 0.01f;
    public override float KeyValue2 => 0.1f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");

    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        if (target == null)
            return;
        bullet.BulletDamageIntensify += target.DamageStrategy.PathProgress * 100f * KeyValue2 * bullet.BulletEffectIntensify;
    }
}

public class GoldenBullet : ElementSkill
{
    public override List<int> InitElements => new List<int> { 10, 10, 10 };
    public override float KeyValue => 1f;
    public override float KeyValue2 => 5;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    private float initMaxFirerate;
    private int shootCount;
    private float intentsifyValue;
    private bool triggered;
    public override void StartTurn()
    {
        base.StartTurn();
        if (!triggered)
        {
            initMaxFirerate = strategy.MaxFireRate;
            shootCount = 0;
            triggered = true;
        }

    }
    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        shootCount++;
        intentsifyValue = KeyValue * shootCount * bullet.BulletEffectIntensify;
        strategy.TurnFixDamageBonus += intentsifyValue;
        bullet.BulletDamageIntensify += intentsifyValue;
        if (shootCount >= (int)KeyValue2)
        {
            strategy.MaxFireRate = 0;
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
        strategy.MaxFireRate = initMaxFirerate;
    }


  

}







