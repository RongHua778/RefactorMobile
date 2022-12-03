using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlobalSkillName
{
    TechCombatSkill,
    TechFireSkill,
    TechMassSkill,
    IceBreak,
    FrostResist,
    LowTech,
    Firerate,
    RuleFrostBuff,
    RapidBuff,
    FirerateLimit,
    RangeLimit,
    ConstructorBuff,
    RapiderBuff,
    ScatterBuff,
    CoordinatorBuff,
    RotaryBuff,
    SnowBuff,
    MortarBuff,
    SniperBuff,
    SuperBuff,
    BoomerrangBuff,
    UltraBuff,
    CoreBuff,

    ChillerBuff,
    FirerBuff,
    LaserBuff,
    BombardBuff,
    MinerBuff,
    NuclearBuff


}

public class GlobalSkillInfo
{
    public GlobalSkillName SkillName;
    public bool IsAbnormal;

    public GlobalSkillInfo(GlobalSkillName skillName, bool isAbnomral)
    {
        this.SkillName = skillName;
        this.IsAbnormal = isAbnomral;
    }
}
public abstract class GlobalSkill : TurretSkill
{
    public abstract GlobalSkillName GlobalSkillName { get; }
    public bool IsAbnormal = false;

}

public class TechCombatSkill : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.TechCombatSkill;

    private float intensifiedValue;

    public override float KeyValue => 0.6f;
    public override float KeyValue2 => 0.4f;
    public override float KeyValue3 => 0.25f;

    private int turretCount;

    public override void Build()
    {
        base.Build();
        if (!IsAbnormal)
        {
            strategy.BaseFixDamageIntensify += KeyValue;
        }
    }
    public override void Detect()
    {
        base.Detect();
        if (IsAbnormal)
        {
            strategy.BaseFixDamageIntensify -= intensifiedValue;
            intensifiedValue = 0;

            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            turretCount = 0;
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null && hit.GetComponent<RefactorTurret>() != null)
                {
                    turretCount++;
                }
            }
            intensifiedValue = turretCount * KeyValue2 - (4 - turretCount) * KeyValue3;
            //intensifiedValue = (turretCount * 2 - 4) * KeyValue2;
            strategy.BaseFixDamageIntensify += intensifiedValue;
        }

    }

}

public class TechFireSkill : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.TechFireSkill;

    public override float KeyValue => 1;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 2;

    private bool detensified = false;
    public override void Build()
    {
        base.Build();
        strategy.BaseFixRange += IsAbnormal ? (int)KeyValue3 : (int)KeyValue;
        if (strategy.Concrete != null)
            strategy.Concrete.GenerateRange();
    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (IsAbnormal)
        {
            float distance = bullet.GetTargetDistance();
            if (distance < 3.5f)
            {
                if (!detensified)
                {
                    strategy.TurnFixDamageBonus -= KeyValue2;
                    detensified = true;
                }
            }
            else
            {
                if (detensified)
                {
                    strategy.TurnFixDamageBonus += KeyValue2;
                    detensified = false;
                }
            }
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        detensified = false;
    }

}


public class TechMassSkill : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.TechMassSkill;
    //public override float KeyValue => 0.05f;
    public override float KeyValue => 1f;

    public override float KeyValue2 => 5;
    public override float KeyValue3 => 0.08f;


    private float intensifiedValue;

    public override void Build()
    {
        base.Build();
        if (!IsAbnormal)
        {
            strategy.AttackIntensify += KeyValue;
        }
        else
        {
            strategy.MaxRange = (int)KeyValue2;
        }
    }

    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (IsAbnormal)
    //    {
    //        if (target == null)
    //            return;
    //        float distance = bullet.GetTargetDistance();
    //        strategy.TurnAttackIntensify -= intensifiedValue;
    //        intensifiedValue = KeyValue3 * distance;
    //        strategy.TurnAttackIntensify += intensifiedValue;
    //    }
    //}
    public override void Detect()
    {
        base.Detect();
        if (IsAbnormal)
        {
            strategy.AttackIntensify -= intensifiedValue;
            intensifiedValue = KeyValue3 * GameRes.TotalLandedRefactor;
            strategy.AttackIntensify += intensifiedValue;
        }

    }
    public override void EndTurn()
    {
        base.EndTurn();
        intensifiedValue = 0;
    }



}

public class IceBreak : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.IceBreak;
    public override float KeyValue => IsAbnormal ? 0.8f : 0.5f;
    public override float KeyValue2 => IsAbnormal ? 0.3f : 0f;
    private bool intensified;
    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (target.DamageStrategy.IsFrost)
        {
            if (!intensified)
            {
                strategy.TurnFixDamageBonus += KeyValue;
                intensified = true;
            }

        }
        else
        {
            if (intensified)
            {
                strategy.TurnFixDamageBonus -= KeyValue;
                intensified = false;
            }

        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensified = false;

    }

}


public class LowTech : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.LowTech;
    public override float KeyValue => IsAbnormal ? 0.8f : 0.5f;
    public override float KeyValue2 => IsAbnormal ? 0.5f : 0;

    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.Rare <= 3)
        {
            strategy.BaseFixDamageIntensify += KeyValue;
        }
        else
        {
            strategy.BaseFixDamageIntensify -= KeyValue2;
        }
    }

}

public class Firerate : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.Firerate;
    public override float KeyValue => 1f;

    public override void Build()
    {
        base.Build();
        strategy.FirerateIntensify += KeyValue;
    }

}

public class RuleFrostBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.RuleFrostBuff;
    public override float KeyValue => 15f;
    public override float KeyValue2 => 5f;

    private float timeCounter;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration = 9999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        timeCounter += delta;
        if (timeCounter > KeyValue)
        {
            timeCounter = 0;
            StaticData.Instance.FrostTurretEffect(strategy.Concrete.transform.position, 0.1f, KeyValue2);
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        timeCounter = 0;
    }



}

public class RapidBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.RapidBuff;
    public override float KeyValue => 0.6f;
    public override float KeyValue2 => 0.25f;
    public override float KeyValue3 => 0.15f;



    float intensifyValue;
    float intensifyDistance;

    public override void Build()
    {
        base.Build();
        if (IsAbnormal)
            strategy.BaseFixFrostResist -= KeyValue2;
        else
            strategy.FirerateIntensify += KeyValue;
    }

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    if (!IsAbnormal)
    //        strategy.TurnFireRateIntensify += KeyValue;
    //}
    public override void Shoot(IDamage target, Bullet bullet = null)
    {
        if (IsAbnormal)
        {
            strategy.TurnFireRateIntensify -= intensifyValue;
            intensifyDistance = bullet.GetTargetDistance();
            intensifyValue = intensifyDistance * KeyValue3;
            strategy.TurnFireRateIntensify += intensifyValue;
        }

    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensifyValue = 0;
        intensifyDistance = 0;
    }
}

public class FirerateLimit : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.FirerateLimit;
    public override float KeyValue => 1f;


    public override void Build()
    {
        base.Build();
        strategy.MaxFireRate = KeyValue;
    }

}

public class RangeLimit : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.RangeLimit;
    public override float KeyValue => 4;


    public override void Build()
    {
        base.Build();
        strategy.MaxRange = (int)KeyValue;
    }

}

public class ConstructorBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.ConstructorBuff;
    public override float KeyValue => 0.6f;
    public override float KeyValue2 => 1;
    public override float KeyValue3 => 3.6f;

    private List<RefactorTurret> intensifiedTurrets=new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Constructor)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            ((ConstructorSkill)strategy.TurretSkills[0]).RangeValue -= KeyValue2;
            ((ConstructorSkill)strategy.TurretSkills[0]).AttackValue += KeyValue3;
        }

    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.AttackIntensify -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.AttackIntensify += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                    }
                }
            }
        }
    }

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    if (!IsAbnormal)
    //    {
    //        List<Vector2Int> points = StaticData.GetCirclePoints(1);
    //        foreach (var point in points)
    //        {
    //            Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
    //            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
    //            if (hit != null)
    //            {
    //                if (hit.GetComponent<RefactorTurret>() != null)
    //                {
    //                    hit.GetComponent<RefactorTurret>().Strategy.TurnAttackIntensify += KeyValue;
    //                }
    //            }
    //        }
    //    }
    //}




}




public class RapiderBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.RapiderBuff;
    public override float KeyValue => 0.4f;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 800;


    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Rapider)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixDamageIntensify -= KeyValue2;
            ((RapiderSkill)(strategy.TurretSkills[0])).MaxValue = KeyValue3;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (!IsAbnormal)
            strategy.TurnFireRateIntensify += KeyValue;
    }

}

public class ScatterBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.ScatterBuff;
    public override float KeyValue => 1f;
    public override float KeyValue2 => 0.65f;
    public override float KeyValue3 => 2f;


    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Scatter)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
            strategy.AttackIntensify -= KeyValue2;
        else
            strategy.BaseFixBulletEffectIntensify += KeyValue;
    }
    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (!IsAbnormal)
    //        bullet.BulletEffectIntensify += KeyValue;
    //}

    public override void StartTurn()
    {
        base.StartTurn();
        if (IsAbnormal)
            strategy.TurnFixTargetCount += (int)KeyValue3;
    }


}

public class CoordinatorBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.CoordinatorBuff;
    public override float KeyValue => 0.03f;
    public override float KeyValue2 => 0.35f;
    public override float KeyValue3 => 1.6f;

    //private bool intensified;
    //private bool triggered;
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Coordinator)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixFrostResist -= KeyValue2;
            //((CoordinatorSkill)(strategy.TurretSkills[0])).IncreaseValue -= KeyValue2;
        }
        else
        {
            ((CoordinatorSkill)(strategy.TurretSkills[0])).IncreaseValue += KeyValue;
        }
    }

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    if (IsAbnormal&&!triggered)
    //    {
    //        Duration = 9999;
    //        triggered = true;
    //    }
    //}

    //public override void Tick(float delta)
    //{
    //    base.Tick(delta);
    //    if (IsAbnormal)
    //    {
    //        if (StrategyBase.CooporativeAttackIntensify > 1.5f)
    //        {
    //            if (!intensified)
    //            {
    //                strategy.TurnFireRateIntensify += KeyValue3;
    //                intensified = true;
    //            }
    //        }
    //        else
    //        {
    //            if (intensified)
    //            {
    //                strategy.TurnFireRateIntensify -= KeyValue3;
    //                intensified = false;

    //            }
    //        }
    //    }
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    triggered = false;
    //    intensified = false;
    //}


}

public class RotaryBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.RotaryBuff;
    public override float KeyValue => 0.45f;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 0.15f;

    private float maxValue => 10f;
    private float intensifiedValue;

    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Rotary)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.FirerateIntensify -= KeyValue2;
        }
    }


    public override void OnEnter(IDamage target)
    {
        base.OnEnter(target);
        if (IsAbnormal)
        {
            if (intensifiedValue < maxValue)
            {
                strategy.TurnFixSplashRange += KeyValue3;
            }
            intensifiedValue += KeyValue3;
        }
    }

    public override void OnExit(IDamage target)
    {
        base.OnExit(target);
        if (IsAbnormal)
        {
            if (intensifiedValue < maxValue)
            {
                strategy.TurnFixSplashRange -= KeyValue3;
            }
            intensifiedValue -= KeyValue3;
        }
    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.FirerateIntensify -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.FirerateIntensify += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                    }
                }
            }
        }
    }


}

public class SnowBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.SnowBuff;
    public override float KeyValue => 0.06f;
    public override float KeyValue2 => 0.65f;
    public override float KeyValue3 => 10f;

    private bool triggered = false;
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Snow)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (!IsAbnormal)
        {
            ((SnowSkill)(strategy.TurretSkills[0])).FreezeEffect += KeyValue;
        }
        else
        {
            strategy.AttackIntensify -= KeyValue2;

        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (!IsAbnormal && !triggered)
        {
            Duration = KeyValue3;
            triggered = true;
        }
    }
    public override void TickEnd()
    {
        base.TickEnd();
        if (!IsAbnormal)
        {
            Duration = KeyValue3;
            IsFinish = false;
            UnFrostTurrets();
        }
    }

    private void UnFrostTurrets()
    {
        List<Vector2Int> points = StaticData.GetCirclePoints(1);
        foreach (var point in points)
        {
            Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
            if (hit != null)
            {
                RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                if (rTurret != null)
                {
                    rTurret.UnFrost();
                }
            }
        }
        //StaticData.Instance.FrostTurretEffect(strategy.Concrete.transform.position, 0.75f, 5f);
    }


    //public override float Hit(float damage, IDamage target, Bullet bullet = null)
    //{
    //    if (!IsAbnormal)
    //    {
    //        if (target.DamageStrategy.CurrentFrost / target.DamageStrategy.MaxFrost <= 0.65f)
    //        {
    //            target.DamageStrategy.ApplyFrost(target.DamageStrategy.MaxFrost * KeyValue);
    //        }
    //    }

    //    return base.Hit(damage, target, bullet);
    //}



    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
    }

}

public class MortarBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.MortarBuff;
    public override float KeyValue => 0.8f;
    public override float KeyValue2 => 0.5f;
    //public override float KeyValue3 => 0.25f;
    private bool intensified;
    private bool triggered;
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Mortar)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixDamageIntensify -= KeyValue2;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (IsAbnormal && !triggered)
        {
            Duration = Mathf.Min(60, GameRes.CurrentWave);
            triggered = true;
        }

    }
    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (!IsAbnormal)
        {
            float distance = bullet.GetTargetDistance();
            if (distance < 3.5f)
            {
                if (!intensified)
                {
                    strategy.TurnFireRateIntensify += KeyValue;
                    intensified = true;
                }
            }
            else
            {
                if (intensified)
                {
                    strategy.TurnFireRateIntensify -= KeyValue;
                    intensified = false;
                }
            }
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensified = false;
    }
    public override void TickEnd()
    {
        base.TickEnd();
        if (IsAbnormal)
        {
            strategy.TurnFixSplashRange += strategy.FinalSplashRange;
        }
    }


    //public override void Shoot(IDamage target = null, Bullet bullet = null)
    //{
    //    base.Shoot(target, bullet);
    //    if (IsAbnormal)
    //        return;
    //    if (target == null)
    //        return;
    //    distance = bullet.GetTargetDistance();
    //    if (distance < 3.5f)
    //    {
    //        strategy.TurnFireRateIntensify -= intensifiedValue;
    //        intensifiedValue = KeyValue;
    //        strategy.TurnFireRateIntensify += intensifiedValue;
    //    }
    //}

    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    if (!IsAbnormal)
    //        return;
    //    if (target == null)
    //        return;
    //    distance = bullet.GetTargetDistance();
    //    bullet.SplashRange += distance * KeyValue3 * bullet.BulletEffectIntensify;
    //}

    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    strategy.TurnFixSplashRange += strategy.FinalSplashRange * KeyValue3;
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    intensifiedValue = 0;
    //}

}

public class SniperBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.SniperBuff;
    public override float KeyValue => 0.02f;
    public override float KeyValue2 => 0.35f;
    public override float KeyValue3 => 1f;

    //float intensifiedValue;
    bool triggered;
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Sniper)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixFrostResist -= KeyValue2;
        }

    }
    public override void StartTurn()
    {
        base.StartTurn();
        if (IsAbnormal && !triggered)
        {
            Duration = Mathf.Min(60, GameRes.CurrentWave);
            triggered = true;
        }
    }

    public override void TickEnd()
    {
        base.TickEnd();
        if (IsAbnormal)
        {
            strategy.TurnFixRange += strategy.FinalRange;
            strategy.Concrete.GenerateRange();
        }
    }

    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        if (target == null)
            return;
        if (!IsAbnormal)
        {
            bullet.AttackIntensify += (1f - target.DamageStrategy.HealthPercent) * 100f * KeyValue * bullet.BulletEffectIntensify;
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        triggered = false;
        strategy.Concrete.GenerateRange();
    }

}

public class SuperBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.SuperBuff;
    public override float KeyValue => 0.3f;
    public override float KeyValue2 => 0.65f;
    public override float KeyValue3 => 2;

    private bool intensified;

    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Super)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.AttackIntensify -= KeyValue2;
            //((SuperSkill)strategy.TurretSkills[0]).ChargeInterval -= KeyValue3;
        }

    }

    public override void Frost()
    {
        base.Frost();
        if (IsAbnormal)
        {
            Duration = 5f;
            IsFinish = false;
            if (!intensified)
            {
                strategy.TurnFireRateIntensify += KeyValue3;
                intensified = true;
            }
        }
    }

    public override void TickEnd()
    {
        base.TickEnd();
        if (intensified)
        {
            strategy.TurnFireRateIntensify -= KeyValue3;
            intensified = false;
        }
    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (!IsAbnormal)
        {
            ((SuperBullet)bullet).BonuceSplash = true;
            ((SuperBullet)bullet).BounceSplashValue = KeyValue;
        }
    }
    public override void EndTurn()
    {
        base.EndTurn();
        intensified = false;
    }


}

public class BoomerrangBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.BoomerrangBuff;
    public override float KeyValue => 0.4f;

    public override float KeyValue2 => 0.35f;
    public override float KeyValue3 => 3f;

    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Boomerrang)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixFrostResist -= KeyValue2;
            ((BoomerangStrategy)strategy).DmgBonusWhileReturn += KeyValue3;
        }
        else
        {
            strategy.BaseFixFrostResist += KeyValue;
        }

    }

}

public class UltraBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.UltraBuff;
    public override float KeyValue => 0.15f;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 0.6f;
    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Ultra)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.FirerateIntensify -= KeyValue2;
        }
    }
    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixCritRate -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixCritRate += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                    }
                }
            }
        }
    }




    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (IsAbnormal)
            ((UltraBullet)bullet).DmgBonusIncreasePersecond = KeyValue3;
    }



}

public class CoreBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.CoreBuff;
    public override float KeyValue => 1f;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 1;


    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Core)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.FirerateIntensify -= KeyValue2;
            ((CoreSkill)strategy.TurretSkills[0]).DetectRange += (int)KeyValue3;
        }

    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (!IsAbnormal)
        {
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.GainRandomTempElement((int)KeyValue);
                    }
                }
            }
        }
    }




}

public class ChillerBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.ChillerBuff;
    public override float KeyValue => 0.6f;
    public override float KeyValue2 => 0.65f;
    public override float KeyValue3 => 0.2f;

    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Chiller)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.AttackIntensify -= KeyValue2;
        }
    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixSlow -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixSlow += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                    }
                }
            }
        }
    }

    public override void OnEnter(IDamage target)
    {
        base.OnEnter(target);
        if (IsAbnormal)
            strategy.TurnFixSlowRate += KeyValue3;
    }

    public override void OnExit(IDamage target)
    {
        base.OnExit(target);
        if (IsAbnormal)
            strategy.TurnFixSlowRate -= KeyValue3;
    }

}

public class FirerBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.FirerBuff;
    public override float KeyValue => 0.3f;
    public override float KeyValue2 => 0.65f;
    public override float KeyValue3 => 0.25f;
    private float maxValue => 1.5f;

    private float intensifiedValue;

    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Firer)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.AttackIntensify -= KeyValue2;
        }

    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixFrostResist -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixFrostResist += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                    }
                }
            }
        }
    }
    public override void StartTurn()
    {
        base.StartTurn();

        if (IsAbnormal)
        {
            Duration = 9999;
        }
        else
        {
            ((Firer)strategy.Concrete).SetFireAngle(60f);
        }
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        if (strategy.Concrete.IsAttacking)
        {
            if (intensifiedValue < maxValue)
            {
                float intent = KeyValue3 * delta;
                intensifiedValue += intent;
                strategy.TurnFixDamageBonus += intent;
            }

        }
        else
        {
            if (intensifiedValue > 0)
            {
                strategy.TurnFixDamageBonus -= intensifiedValue;
                intensifiedValue = 0;
            }
        }
    }
    public override void EndTurn()
    {
        base.EndTurn();
        intensifiedValue = 0;
    }


}

public class LaserBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.LaserBuff;
    public override float KeyValue => 1f;
    public override float KeyValue2 => 0.5f;
    public override float KeyValue3 => 0.4f;
    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();

    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Laser)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.FirerateIntensify -= KeyValue2;
        }

    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (IsAbnormal)
            ((LaserBullet)bullet).SetTravelIncrease(true);
    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixRange -= (int)KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixRange += (int)KeyValue;
                        intensifiedTurrets.Add(rTurret);
                        rTurret.GenerateRange();
                    }
                }
            }
        }
    }

}

public class BombardBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.BombardBuff;
    public override float KeyValue => 0.45f;
    public override float KeyValue2 => 1f;
    public override float KeyValue3 => 3;
    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();

    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Bombard)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            ((BombardSkill)strategy.TurretSkills[0]).BulletOffset *= (1 + KeyValue2);
            ((BombardSkill)strategy.TurretSkills[0]).BulletCount += (int)KeyValue3;
        }

    }


    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixSplash -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixSplash += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                        rTurret.GenerateRange();
                    }
                }
            }
        }
    }
}

public class MinerBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.MinerBuff;
    public override float KeyValue => 1;
    public override float KeyValue2 => 5f;
    public override float KeyValue3 => 0.1f;


    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Miner)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            ((MinerSkill)strategy.TurretSkills[0]).DeployInterval += KeyValue2;
        }

    }

    public override void StartTurn()
    {
        base.StartTurn();
        if (!IsAbnormal)
            ((Miner)strategy.Concrete).Deploy();
    }

    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        if (IsAbnormal)
        {
            ((MinerBullet)bullet).IsDmgIncrease = true;
        }
    }
}

public class NuclearBuff : GlobalSkill
{
    public override GlobalSkillName GlobalSkillName => GlobalSkillName.NuclearBuff;
    public override float KeyValue => 0.5f;
    public override float KeyValue2 => 0.35f;
    public override float KeyValue3 => 2.5f;

    private List<RefactorTurret> intensifiedTurrets = new List<RefactorTurret>();
    public override void Build()
    {
        base.Build();
        if (strategy.Attribute.RefactorName != RefactorTurretName.Nuclear)
        {
            strategy.GlobalSkills.Remove(this);
            return;
        }
        if (IsAbnormal)
        {
            strategy.BaseFixFrostResist -= KeyValue2;
            ((NuclearSkill)(strategy.TurretSkills[0])).IntentValue += KeyValue3;
        }

    }

    public override void Detect()
    {
        base.Detect();
        if (!IsAbnormal)
        {
            foreach (RefactorTurret turret in intensifiedTurrets)
            {
                turret.Strategy.BaseFixBulletEffectIntensify -= KeyValue;
            }
            intensifiedTurrets.Clear();
            List<Vector2Int> points = StaticData.GetCirclePoints(1);
            foreach (var point in points)
            {
                Vector2 pos = point + (Vector2)strategy.Concrete.transform.position;
                Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
                if (hit != null)
                {
                    RefactorTurret rTurret = hit.GetComponent<RefactorTurret>();
                    if (rTurret != null)
                    {
                        rTurret.Strategy.BaseFixBulletEffectIntensify += KeyValue;
                        intensifiedTurrets.Add(rTurret);
                        rTurret.GenerateRange();
                    }
                }
            }
        }
    }

  
}


