using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class TurretSkill
{
    public virtual RefactorTurretName EffectName { get; }
    public virtual string SkillDescription { get; set; }
    public StrategyBase strategy;
    //public Bullet bullet;

    public virtual string SkillName { get; set; }
    public virtual string DisplayValue { get; }
    public virtual string DisplayValue2 { get; }
    public virtual string DisplayValue3 { get; }
    public virtual string DisplayValue4 { get; }
    public virtual string DisplayValue5 { get; }
    public virtual float KeyValue { get; set; }
    public virtual float KeyValue2 { get; set; }
    public virtual float KeyValue3 { get; set; }

    public virtual ElementType IntensifyElement => ElementType.None;
    public float Duration;
    public bool IsFinish = true;

    public virtual void Composite()
    {

    }

    public virtual void Detect()
    {

    }

    public virtual void Build()
    {

    }

    public virtual void Frost()
    {

    }

    public virtual void Shoot(IDamage target = null, Bullet bullet = null)
    {

    }
    public virtual void AfterShoot(Bullet bullet = null, IDamage target = null)
    {

    }

    public virtual void PreHit(Bullet bullet = null)
    {

    }

    public virtual float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        return damage;
    }

    public virtual void Splash(ConcreteContent content, Bullet bullet = null)
    {

    }

    public virtual void Draw()
    {

    }

    public virtual void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            IsFinish = true;
            TickEnd();
        }
    }

    public virtual void TickEnd()
    {

    }

    public virtual void StartTurn()
    {

    }

    public virtual void StartTurn2()
    {
        IsFinish = false;
    }

    public virtual void StartTurn3()
    {

    }

    public virtual void EndTurn()
    {
        IsFinish = true;
        Duration = 0;
    }

    public virtual void OnEquip()
    {

    }

    public virtual void OnEquipped()//当有其他技能装备时
    {

    }

    public virtual void OnEnter(IDamage target)
    {

    }

    public virtual void OnExit(IDamage target)
    {

    }

}

public abstract class InitialSkill : TurretSkill
{

}
public class RotarySkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Rotary;

    public override float KeyValue => 0.1f;
    private float maxValue => 15;
    public override float KeyValue2 => 2f;
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(maxValue.ToString());

    public override string DisplayValue4 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * KeyValue * 100 + "%");



}
public class SniperSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Sniper;

    public override float KeyValue => 0.25f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");


    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        bullet.BulletDamageIntensify += KeyValue * bullet.GetTargetDistance() * bullet.BulletEffectIntensify;
    }
}

public class ScatterSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Scatter;

    public override float KeyValue => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());


    public override void Build()
    {
        base.Build();
        strategy.BaseFixTargetCount += (int)KeyValue;
    }

}
public class UltraSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Ultra;

    public override float KeyValue => 5f;
    public override float KeyValue2 => 1f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));


}
public class MortarSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Mortar;

    public override float KeyValue => 0.3f;
    public override float KeyValue2 => 0.25f;
    public override float KeyValue3 => 5f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));

    public override string DisplayValue4 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue3 * 100 + "%");


    public override void PreHit(Bullet bullet = null)
    {
        base.PreHit(bullet);
        bullet.SplashPercentage += Mathf.Min(KeyValue3, bullet.HitSize * KeyValue2 * bullet.BulletEffectIntensify);
    }


}



public class RapiderSkill : InitialSkill
{
    //持续攻击时会加大弹道偏移
    public override RefactorTurretName EffectName => RefactorTurretName.Rapider;
    public override float KeyValue => 10f;
    public override float KeyValue2 => 1f;
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(MaxValue.ToString());

    public float MaxValue;
    float intensifiedValue;
    public override void Build()
    {
        base.Build();
        MaxValue = 300f;
    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (intensifiedValue < MaxValue)
        {
            strategy.TurnFixAttack += KeyValue;
            intensifiedValue += KeyValue;
        }
    }
    public override void EndTurn()
    {
        base.EndTurn();
        intensifiedValue = 0;
    }

}

public class ConstructorSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Constructor;

    public override float KeyValue => RangeValue;
    public override float KeyValue2 => AttackValue;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");

    public float RangeValue = 3f;
    public float AttackValue = 1.2f;

    private bool intensifed;
    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        float distance = bullet.GetTargetDistance();
        if (distance < KeyValue)
        {
            if (!intensifed)
            {
                strategy.TurnAttackIntensify += KeyValue2;
                intensifed = true;
            }
        }
        else
        {
            if (intensifed)
            {
                strategy.TurnAttackIntensify -= KeyValue2;
                intensifed = false;
            }
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensifed = false;
    }


    // private float timeCounter;
    //private bool charged;
    //private int hitCount;

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    Duration = 9999;
    //}

    //public override void Tick(float delta)
    //{
    //    base.Tick(delta);
    //    if (charged)
    //        return;
    //    timeCounter += delta;
    //    if (timeCounter > KeyValue)
    //    {
    //        charged = true;
    //        timeCounter = 0;
    //    }
    //}


    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    charged = false;
    //}

    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (hitCount < (int)RangeValue)
    //    {
    //        hitCount++;
    //        charged = false;
    //    }
    //    else
    //    {
    //        charged = true;
    //        hitCount = 0;
    //    }
    //}

    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    if (charged)
    //    {
    //        bullet.AttackIntensify += KeyValue2 * bullet.BulletEffectIntensify;
    //        charged = false;
    //    }
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    charged = false;
    //    hitCount = 0;
    //}


    //bool attackMode;
    //bool firerateMode;
    //float distance;

    //float fireRateIntensifyValue;
    //float attackIntensifyValue;



    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (target == null)
    //        return;
    //    distance = bullet.GetTargetDistance();
    //    if (distance < KeyValue2)
    //    {
    //        if (!firerateMode)
    //        {
    //            fireRateIntensifyValue = KeyValue;
    //            strategy.TurnFireRateIntensify += fireRateIntensifyValue;
    //            firerateMode = true;
    //        }
    //    }
    //    else
    //    {
    //        if (firerateMode)
    //        {
    //            strategy.TurnFireRateIntensify -= fireRateIntensifyValue;
    //            firerateMode = false;
    //        }
    //    }
    //    if (distance > KeyValue2)
    //    {
    //        if (!attackMode)
    //        {
    //            attackIntensifyValue = attackValue;
    //            strategy.TurnAttackIntensify += attackIntensifyValue;
    //            attackMode = true;
    //        }
    //    }
    //    else
    //    {
    //        if (attackMode)
    //        {
    //            strategy.TurnAttackIntensify -= attackIntensifyValue;
    //            attackMode = false;
    //        }
    //    }

    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    attackMode = false;
    //    firerateMode = false;

    //    attackIntensifyValue = 0;
    //    fireRateIntensifyValue = 0;
    //}

}



public class SnowSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Snow;
    public override float KeyValue => FreezeEffect;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    //public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("SNOWDOWN"));
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));

    public float FreezeEffect = 0.06f;
    //private BuffInfo buffInfo;
    //public override void Build()
    //{
    //    base.Build();
    //    buffInfo = new BuffInfo(EnemyBuffName.SlowDown, 1);

    //}
    //public override float Hit(float damage, IDamage target, Bullet bullet = null)
    //{
    //    buffInfo.Stacks = Mathf.RoundToInt(bullet.BulletEffectIntensify);
    //    target.DamageStrategy.ApplyBuff(buffInfo);
    //    return damage;
    //}


    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        target.DamageStrategy.ApplyFrost(target.DamageStrategy.MaxFrost * KeyValue * bullet.BulletEffectIntensify);
        return base.Hit(damage, target, bullet);
    }

}

public class CoordinatorSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Coordinator;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(IncreaseValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(StrategyBase.CoordinatorMaxIntensify * 100 + "%");

    public float IncreaseValue = 0.05f;


    private float intensifyValue;

    public override void StartTurn()
    {
        base.StartTurn();
        Duration = 9999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        float valuePerDelta = IncreaseValue * delta;
        if (strategy.Concrete.IsAttacking)
        {
            if (intensifyValue < StrategyBase.CoordinatorMaxIntensify)
            {
                StrategyBase.CooporativeAttackIntensify += valuePerDelta;
                intensifyValue += valuePerDelta;
            }
        }
        else
        {
            if (intensifyValue > 0)
            {
                StrategyBase.CooporativeAttackIntensify -= intensifyValue;
                intensifyValue = 0;
            }

        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensifyValue = 0;
    }


}

public class BoomerrangSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Boomerrang;
    public override float KeyValue => 0.35f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    //public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("BREAK"));
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));

    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        if (target.DamageStrategy.IsFrost)
        {
            target.DamageStrategy.UnFrost();
            damage *= (1 + KeyValue * bullet.BulletEffectIntensify);
        }
        return damage;
    }
}

public class SuperSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Super;

    public override float KeyValue => 5;
    public override float KeyValue2 => 3;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    //public float ChargeInterval = 8f;
    //public float JumpTime = 3;

    //private float timeCounter;
    //private bool charged;


    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    Duration = 9999;
    //}

    //public override void Tick(float delta)
    //{
    //    base.Tick(delta);
    //    if (charged)
    //        return;
    //    timeCounter += delta;
    //    if (timeCounter > KeyValue)
    //    {
    //        charged = true;
    //        timeCounter = 0;
    //    }
    //}

    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    if (charged)
    //    {
    //        ((SuperBullet)bullet).BonuceTimes = Mathf.RoundToInt((int)KeyValue2 * bullet.BulletEffectIntensify);
    //        charged = false;
    //    }
    //}
    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    charged = false;
    //}
    private int hitCount;
    private bool charged;

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (hitCount < (int)KeyValue)
        {
            hitCount++;
            charged = false;
        }
        else
        {
            charged = true;
            hitCount = 0;
        }
    }
    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        if (charged)
        {
            ((SuperBullet)bullet).BonuceTimes = Mathf.RoundToInt((int)KeyValue2 * bullet.BulletEffectIntensify);
            charged = false;
        }
    }



    public override void EndTurn()
    {
        base.EndTurn();
        hitCount = 0;
        charged = false;
    }

}

public class CoreSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Core;
    public override float KeyValue => 0.5f;
    public override float KeyValue2 => 0.25f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(DetectRange.ToString());

    public int DetectRange = 1;

    public override void StartTurn()
    {
        base.StartTurn();
        List<Vector2Int> points = StaticData.GetCirclePoints(DetectRange);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
            {
                RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                if (rTurret != null)
                {
                    rTurret.Strategy.TurnAttackIntensify -= KeyValue;
                    strategy.TurnAttackIntensify += KeyValue2;
                }
            }
        }
    }

}

public class PrismSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Prism;
    public override float KeyValue => 2f;
    public override float KeyValue2 => 8f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    //public override void Build()
    //{
    //    base.Build();
    //    ElementSkill effect = TurretSkillFactory.GetElementSkill(new List<int> { 11, 11, 11 });
    //    strategy.AddElementSkill(effect);
    //}

}

public class AmplifierSkill : InitialSkill
{
    // public override BuildingSkillName BuildingSkillName => BuildingSkillName.AMPLIFIER;
    public override RefactorTurretName EffectName => RefactorTurretName.Amplifier;

    public override float KeyValue => 1f;
    public override float KeyValue2 => 10f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    private List<TrapContent> intensifiedTraps = new List<TrapContent>();
    float intensifiedValue;

    public override void StartTurn()
    {
        base.StartTurn();
        Duration = KeyValue2;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        Duration = KeyValue2;
        IsFinish = false;
        StaticData.Instance.FrostTurretEffect(strategy.Concrete.transform.position, 1.6f, 5f);
    }
    public override void StartTurn3()
    {
        base.StartTurn3();
        List<Vector2Int> points;
        intensifiedValue = KeyValue;
        switch (strategy.RangeType)
        {
            case RangeType.Line:
                float up = Vector2.Dot(Vector2.up, strategy.Concrete.transform.up);
                Vector3 right = Vector3.Cross(Vector2.up, (Vector2)strategy.Concrete.transform.up);
                points = StaticData.GetLinePoints(strategy.FinalRange);
                Vector2 pos;
                foreach (var point in points)
                {
                    if (right.z < -0.1f)//右边
                    {
                        pos = new Vector2Int(point.y, point.x);
                    }
                    else if (right.z > 0.1f)//左边
                    {
                        pos = new Vector2Int(-point.y, point.x);
                    }
                    else if (up < -0.1f)//下边
                    {
                        pos = new Vector2Int(point.x, -point.y);
                    }
                    else
                    {
                        pos = point;//上边
                    }
                    Vector2 contentpos = pos + (Vector2)strategy.Concrete.transform.position;
                    Collider2D hit = StaticData.RaycastCollider(contentpos, LayerMask.GetMask(StaticData.TrapMask));
                    if (hit != null)
                    {
                        TrapContent trap = hit.GetComponent<TrapContent>();
                        trap.TrapIntensify += intensifiedValue;
                        intensifiedTraps.Add(trap);
                    }
                }
                break;
            case RangeType.Circle:
                points = StaticData.GetCirclePoints(strategy.FinalRange);
                foreach (var point in points)
                {
                    Vector2 contentpos = point + (Vector2)strategy.Concrete.transform.position;
                    Collider2D hit = StaticData.RaycastCollider(contentpos, LayerMask.GetMask(StaticData.TrapMask));
                    if (hit != null)
                    {
                        TrapContent trap = hit.GetComponent<TrapContent>();
                        trap.TrapIntensify += intensifiedValue;
                        intensifiedTraps.Add(trap);
                    }
                }
                break;
        }

    }

    public override void EndTurn()
    {
        foreach (var trap in intensifiedTraps)
        {
            trap.TrapIntensify -= intensifiedValue;
        }
        intensifiedValue = 0;
        intensifiedTraps.Clear();
        base.EndTurn();
    }

}

public class TeleportorSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Teleportor;

    public override float KeyValue => 5f;
    public override float KeyValue2 => 10f;
    public override float KeyValue3 => 5f;


    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue3.ToString());

    private bool triggered;
    private List<IDamage> HitList;

    public override void Build()
    {
        base.Build();
        HitList = new List<IDamage>();
        strategy.BaseFixFrostResist += 100;
    }
    public override void StartTurn()
    {
        base.StartTurn();
        if (!triggered)
        {
            Duration = KeyValue2;
            triggered = true;
        }
    }
    public override void TickEnd()
    {
        base.TickEnd();

        Duration = KeyValue2;
        IsFinish = false;
        FrostTurrets();
    }

    private void FrostTurrets()
    {
        StaticData.Instance.FrostTurretEffect(strategy.Concrete.transform.position, 1.5f, KeyValue3);
    }
    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        if (target.DamageStrategy.IsEnemy)
        {
            if (!HitList.Contains(target))
            {
                ((Enemy)target).Flash(Mathf.RoundToInt((GameRes.CurrentWave / KeyValue) * bullet.BulletEffectIntensify));
                HitList.Add(target);
            }
        }
        return base.Hit(damage, target, bullet);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        HitList.Clear();
        triggered = false;
    }


}

public class BountySkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Bounty;

    public override float KeyValue => 5;
    public override float KeyValue2 => 50;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    private bool triggered;
    private int shootCount;
    private float initMaxFirerate;
    public override void Build()
    {
        base.Build();
        strategy.BaseFixFrostResist += 100;
    }
    public override void StartTurn()
    {
        base.StartTurn();
        if (!triggered)
        {
            initMaxFirerate = strategy.MaxFireRate;
            shootCount = 0;
        }
    }


    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        shootCount++;
        if (shootCount >= (int)KeyValue)
        {
            strategy.MaxFireRate = 0;
        }
        //intensifyValue = timeCounter * KeyValue * bullet.BulletEffectIntensify;
        //bullet.BulletDamageIntensify += intensifyValue;
        //intensifyValue = 0;
        StaticData.Instance.GainMoneyEffect(strategy.Concrete.transform.position, Mathf.RoundToInt((KeyValue2 + GameRes.CurrentWave) * bullet.BulletEffectIntensify));

    }
    public override void EndTurn()
    {
        base.EndTurn();
        strategy.MaxFireRate = initMaxFirerate;
        shootCount = 0;
        triggered = false;
    }



}

public class ChillerSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Chiller;

    public override float KeyValue => DoubleValue;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    //public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(ShootCount.ToString());
    //public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());
    //public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("SNOWDOWN"));
    public float DoubleValue = 1;
    public override void StartTurn3()
    {
        base.StartTurn3();
        strategy.TurnFixSlowRate += KeyValue * strategy.FinalSlowRate;
    }

    //public int ShootCount = 3;

    //private int hitCount;
    //private BuffInfo buffInfo;
    //private bool charged;
    //public override void Build()
    //{
    //    base.Build();
    //    buffInfo = new BuffInfo(EnemyBuffName.SlowDown, 1);
    //}
    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (hitCount < ShootCount)
    //    {
    //        hitCount++;
    //        charged = false;
    //    }
    //    else
    //    {
    //        charged = true;
    //        hitCount = 0;
    //    }
    //}


    //public override float Hit(float damage, IDamage target, Bullet bullet = null)
    //{
    //    if (charged)
    //    {
    //        buffInfo.Stacks = Mathf.RoundToInt(bullet.BulletEffectIntensify);
    //        target.DamageStrategy.ApplyBuff(buffInfo);
    //    }
    //    return damage;
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    hitCount = 0;
    //    charged = false;
    //}

}

public class FirerSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Firer;

    public override float KeyValue => 0.35f;
    public override float KeyValue2 => 1;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    public override void Build()
    {
        base.Build();
        strategy.BaseFixFrostResist += KeyValue;
    }

}


public class LaserSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Laser;

    public override float KeyValue => 2;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATIONBULLET"));



}

public class BombardSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Bombard;

    public override float KeyValue => BulletCount;
    public override float KeyValue2 => BulletOffset;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");

    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
    public int BulletCount = 3;
    public float BulletOffset = 0.3f;

    private bool triggered = false;
    public override void StartTurn()
    {
        base.StartTurn();
        if (!triggered)
        {
            ((BomBard)strategy.Concrete).BulletCount = (int)KeyValue;
            ((BomBard)strategy.Concrete).BulletOffset = KeyValue2;
            triggered = true;
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
    }


}

public class MinerSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Miner;

    private List<BasicTile> pathTiles;
    public override float KeyValue => DeployInterval;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());

    public float DeployInterval = 7f;

    public override void Build()
    {
        base.Build();
        pathTiles = new List<BasicTile>();
    }

    public override void StartTurn2()
    {
        base.StartTurn2();
        GetPathTiles();
        ((Miner)strategy.Concrete).DeployInterval = KeyValue;

    }

    public override void EndTurn()
    {
        base.EndTurn();
        pathTiles.Clear();
        ((Miner)(strategy.Concrete)).PathTiles = pathTiles;
    }

    private void GetPathTiles()
    {
        pathTiles.Clear();
        List<Vector2Int> points = StaticData.GetCirclePoints(strategy.FinalRange);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.ConcreteTileMask));
            if (hit != null)
            {
                BasicTile tile = hit.GetComponent<BasicTile>();
                if (tile.isPath)
                {
                    pathTiles.Add(tile);
                }
            }
        }
        ((Miner)(strategy.Concrete)).PathTiles = pathTiles;
    }
}

public class NuclearSkill : InitialSkill
{
    public override RefactorTurretName EffectName => RefactorTurretName.Nuclear;


    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(ChargedTime.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(IntentValue * 100 + "%");
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("GROUNDBULLET"));
    private bool charged;

    public float ChargedTime = 20;
    public float IntentValue = 4f;




    public override void StartTurn()
    {
        base.StartTurn();
        charged = false;
        Duration = ChargedTime;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        charged = true;
        IsFinish = false;
        Duration = ChargedTime;
    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (charged)
        {
            bullet.BulletEffectIntensify += IntentValue;
            charged = false;
        }
    }



}



