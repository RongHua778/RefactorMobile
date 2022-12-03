using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RangeSplash : ElementSkill
{
    public override List<int> InitElements => new List<int> { 4, 4, 4 };
    public override float KeyValue => 2;
    public override float KeyValue2 => 0.8f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    //public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized("30%");

    public override void Build()
    {
        base.Build();
        strategy.AttackIntensify -= KeyValue2;
    }

    public override void StartTurn()
    {
        base.StartTurn();
        //strategy.TurnAttackIntensify-= KeyValue2;
        strategy.TurnFixTargetCount += (int)KeyValue;
        //strategy.TurnBulletSize += 0.3f;
    }


    //public override float KeyValue => 2;
    //public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    List<Vector2Int> points = StaticData.GetCirclePoints(1);
    //    foreach (var point in points)
    //    {
    //        Vector2 pos = point + (Vector2)strategy.Turret.transform.position;
    //        Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
    //        if (hit != null)
    //        {
    //            StrategyBase tStrategt = hit.GetComponent<TurretContent>().Strategy;
    //            strategy.GainRandomTempElement(Mathf.RoundToInt(KeyValue * tStrategt.PoloIntensify));
    //            //RefactorTurret turret = hit.GetComponent<RefactorTurret>();
    //            //if (turret != null)
    //            //{
    //            //    RefactorStrategy tStrategy = turret.Strategy as RefactorStrategy;
    //            //    tStrategy.GainRandomTempElement(Mathf.RoundToInt(strategy.PoloIntensify * KeyValue));
    //            //}

    //            //strategy.GainRandomTempElement(Mathf.RoundToInt(KeyValue * tStrategy.PoloIntensify));
    //            //if (tStrategy.TotalElementCount > 0)
    //            //{
    //            //    List<ElementType> addElements = tStrategy.GetRandomElementsOfthisTurret(Mathf.RoundToInt(tStrategy.PoloIntensify * KeyValue));
    //            //    foreach (var item in addElements)
    //            //    {
    //            //        switch (item)
    //            //        {
    //            //            case ElementType.GOLD:
    //            //                strategy.TempGoldCount++;
    //            //                break;
    //            //            case ElementType.WOOD:
    //            //                strategy.TempWoodCount++;
    //            //                break;
    //            //            case ElementType.WATER:
    //            //                strategy.TempWaterCount++;
    //            //                break;
    //            //            case ElementType.FIRE:
    //            //                strategy.TempFireCount++;
    //            //                break;
    //            //            case ElementType.DUST:
    //            //                strategy.TempDustCount++;
    //            //                break;

    //            //        }
    //            //    }
    //            //}
    //        }
    //    }
    //}

    //List<int> ElementsCount;
    //int minValue;
    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    minValue = 999;
    //    ElementsCount = new List<int>();
    //    ElementsCount.Add(strategy.GoldCount);
    //    ElementsCount.Add(strategy.WoodCount);
    //    ElementsCount.Add(strategy.WaterCount);
    //    ElementsCount.Add(strategy.FireCount);
    //    ElementsCount.Add(strategy.DustCount);
    //    for (int i = 0; i < ElementsCount.Count; i++)
    //    {
    //        if (ElementsCount[i] != 0 && ElementsCount[i] < minValue)
    //        {
    //            minValue = ElementsCount[i];
    //        }
    //    }
    //    for (int i = 0; i < ElementsCount.Count; i++)
    //    {
    //        if (ElementsCount[i] == minValue)
    //        {
    //            switch (i)
    //            {
    //                case 0:
    //                    strategy.TempGoldCount += 2;
    //                    break;
    //                case 1:
    //                    strategy.TempWoodCount += 2;
    //                    break;
    //                case 2:
    //                    strategy.TempWaterCount += 2;
    //                    break;
    //                case 3:
    //                    strategy.TempFireCount += 2;
    //                    break;
    //                case 4:
    //                    strategy.TempDustCount += 2;
    //                    break;
    //            }
    //        }
    //    }
    //}
}

public class CloseSplash : ElementSkill
{
    //近战溅射
    public override List<int> InitElements => new List<int> { 1, 1, 4 };
    public override float KeyValue => 0.15f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFrostResist += KeyValue * strategy.DustCount;
    }
}
public class HitSplash : ElementSkill
{
    //重型溅射
    public override List<int> InitElements => new List<int> { 2, 2, 4 };
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
        buffInfo.Stacks = Mathf.RoundToInt(strategy.DustCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return damage;
    }
}
public class TimeSplash : ElementSkill
{
    //专注溅射
    public override List<int> InitElements => new List<int> { 0, 0, 4 };
    public override float KeyValue => 0.2f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixDamageBonus += KeyValue * strategy.DustCount;
    }
}
public class FarSplash : ElementSkill
{
    //爆发溅射
    public override List<int> InitElements => new List<int> { 3, 3, 4 };
    public override float KeyValue => 1;
    public override float KeyValue2 => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixRange += (int)KeyValue * (strategy.DustCount / (int)KeyValue2);
        strategy.Concrete.GenerateRange();

    }

    public override void EndTurn()
    {
        base.EndTurn();
        strategy.Concrete.GenerateRange();
    }

}
//public class PoloGetter : ElementSkill
//{
//    //受到的所有光环效果翻倍
//    public override List<int> Elements => new List<int> { 4, 4, 4 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSputteringRangeIntensify -= 0.5f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSputteringRangeIntensify += 0.5f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}


//public class AttackPolo : ElementSkill
//{
//    //相邻防御塔攻击力提高50%
//    public override List<int> Elements => new List<int> { 4, 4, 0 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitAttackIntensify -= 0.3f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitAttackIntensify += 0.3f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }

//}

//public class SpeedPolo : ElementSkill
//{
//    //相邻防御塔攻速提高50%
//    public override List<int> Elements => new List<int> { 4, 4, 1 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSpeedIntensify -= 0.3f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSpeedIntensify += 0.3f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

//public class SlowAdjacent : ElementSkill
//{
//    //相邻每个防御塔提高0.5减速
//    public override List<int> Elements => new List<int> { 4, 4, 2 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitSlowRateIntensify -= 0.5f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitSlowRateIntensify += 0.5f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}

//public class CriticalPolo : ElementSkill
//{
//    //相邻防御塔提高20%暴击
//    public override List<int> Elements => new List<int> { 4, 4, 3 };

//    private List<StrategyBase> intensifiedStrategies = new List<StrategyBase>();
//    public override void Detect()
//    {
//        foreach (var strategy in intensifiedStrategies)
//        {
//            strategy.InitCriticalRateIntensify -= 0.2f * strategy.PoloIntensifyModify;
//        }
//        intensifiedStrategies.Clear();
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//            {
//                StrategyBase strategy = hit.GetComponent<TurretContent>().Strategy;
//                strategy.InitCriticalRateIntensify += 0.2f * strategy.PoloIntensifyModify;
//                intensifiedStrategies.Add(strategy);
//            }
//        }
//    }
//}
