using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BulletType
{
    Ground, Target, Penetrate, Self
}
public abstract class Bullet : ReusableObject, IGameBehavior
{
    [SerializeField] private ParticalControl sputteringEffect = default;

    [SerializeField] private Rigidbody2D m_Rig;
    public abstract BulletType BulletType { get; }
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }

    [HideInInspector]
    public TurretContent turretParent;
    protected List<TurretSkill> turretEffects;
    protected List<GlobalSkill> turretGlobalSkills;

    private Vector2 targetPos;
    protected virtual Vector2 TargetPos
    {
        get => targetPos;
        set => targetPos = value;
    }

    protected readonly float minDistanceToDealDamage = .2f;

    private float bulletEffectIntensify = 0;
    public float BulletEffectIntensify { get => 1 + bulletEffectIntensify; set => bulletEffectIntensify = value; }

    private float bulletSpeed;
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
    private float baseAttack;
    public float BaseAttack { get => baseAttack; set => baseAttack = value; }
    private float attackIntensify;
    public float AttackIntensify { get => attackIntensify; set => attackIntensify = value; }

    private float sputteringRange;
    public float SplashRange { get => sputteringRange; set => sputteringRange = value; }

    private float sputteringPercentage;
    public float SplashPercentage { get => sputteringPercentage; set => sputteringPercentage = value; }
    public float FinalSplashPercentage => SplashPercentage + SplashRange * 0.2f;

    private float slowPercentage;
    public float SlowPercentage { get => slowPercentage; set => slowPercentage = value; }
    public float FinalSlowPercentage => SlowPercentage;


    private float criticalRate;
    public float CriticalRate { get => criticalRate; set => criticalRate = value; }

    private float criticalPercentage;
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }
    private float slowRate;
    public float SlowRate { get => slowRate; set => slowRate = value; }

    private float bulletDamageIntensify = 0;
    public float BulletDamageIntensify { get => bulletDamageIntensify; set => bulletDamageIntensify = value; }

    public float FinalDamage => Mathf.Max(0, BaseAttack * (1 + AttackIntensify) * (1 + Mathf.Max(-1, BulletDamageIntensify)));
    public ParticalControl SputteringEffect { get => sputteringEffect; set => sputteringEffect = value; }

    [HideInInspector]
    public bool isCritical;//是否暴击
    [HideInInspector]
    public int HitSize;//溅射目标数

    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        isCritical = false;
    }

    public virtual void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        this.Target = target;
        this.TargetPos = pos ?? target.Position;
        this.turretParent = turret;
        this.turretEffects = turret.Strategy.TurretSkills;
        this.turretGlobalSkills = turret.Strategy.GlobalSkills;
        TriggerShootEffect(target ? target.Enemy : null);
        SetAttribute(turret);
        TriggerAfterShoot(target ? target.Enemy : null);
    }

    protected void SetAttribute(TurretContent turret)
    {
        this.BaseAttack = turret.Strategy.BaseAttack;
        this.AttackIntensify = turret.Strategy.TotalAttackIntensify;
        this.BulletSpeed = turret.Strategy.Attribute.BulletSpeed;
        this.SplashRange = turret.Strategy.FinalSplashRange;
        this.CriticalRate = turret.Strategy.FinalCriticalRate;
        this.CriticalPercentage = turret.Strategy.FinalCriticalPercentage;
        this.SlowRate = turret.Strategy.FinalSlowRate;
        this.SplashPercentage = turret.Strategy.FinalSplashPercentage;
        this.SlowPercentage = turret.Strategy.FianlSlowPercentageOfSplash;
        this.BulletDamageIntensify = turret.Strategy.FinalBulletDamageIntensify;
        this.BulletEffectIntensify = turret.Strategy.FinalBulletEffectIntensify;
    }

    protected void TriggerShootEffect(IDamage target)
    {
        foreach (TurretSkill effect in turretEffects)
        {
            effect.Shoot(target, this);
        }
        foreach (GlobalSkill skill in turretGlobalSkills)
        {
            skill.Shoot(target, this);
        }
    }

    public void TriggerAfterShoot(IDamage target)//子弹射击后触发
    {
        isCritical = UnityEngine.Random.value <= CriticalRate;//射击时判断是否暴击
        foreach (TurretSkill effect in turretEffects)
        {
            effect.AfterShoot(this, target);
        }
        foreach (GlobalSkill skill in turretGlobalSkills)
        {
            skill.AfterShoot(this, target);
        }
    }

    protected float TriggerHitEffect(IDamage target)
    {
        float damage = FinalDamage;
        foreach (TurretSkill effect in turretEffects)
        {
            damage = effect.Hit(damage, target, this);
        }
        foreach (GlobalSkill skill in turretGlobalSkills)
        {
            damage = skill.Hit(damage, target, this);
        }
        return damage;
    }

    protected void TriggerSplashEffect(ConcreteContent content)
    {
        foreach (TurretSkill effect in turretEffects)
        {
            effect.Splash(content, this);
        }
        foreach (GlobalSkill skill in turretGlobalSkills)
        {
            skill.Splash(content, this);
        }
    }

    protected void TriggerPrehit()
    {
        foreach (TurretSkill effect in turretEffects)
        {
            effect.PreHit(this);
        }
        foreach (GlobalSkill skill in turretGlobalSkills)
        {
            skill.PreHit(this);
        }
    }

    public abstract bool GameUpdate();


    protected bool DistanceCheck(Vector2 pos)
    {
        float distanceToTarget = ((Vector2)transform.position - pos).magnitude;
        if (distanceToTarget < minDistanceToDealDamage)
        {
            TriggerDamage();
            return false;
        }
        return true;
    }

    protected void RotateBullet(Vector2 pos)
    {
        Vector2 targetPos = pos - (Vector2)transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    protected void MoveTowards(Vector2 pos)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            pos, BulletSpeed * Time.deltaTime);
    }

    protected void MoveTowardsRig(Vector2 pos)
    {
        m_Rig.MovePosition(m_Rig.position + (pos - m_Rig.position).normalized * bulletSpeed * Time.fixedDeltaTime);
    }

    public float GetTargetDistance()
    {
        float distanceToTarget = ((Vector2)turretParent.transform.position - TargetPos).magnitude;
        return distanceToTarget;
    }

    public virtual void ReclaimBullet()
    {
        BulletEffectIntensify = 0;
        BulletDamageIntensify = 0;
        ObjectPool.Instance.UnSpawn(this);
    }

    public virtual void TriggerDamage()
    {
        ReclaimBullet();
    }

    private float slowTemp;
    private float finalDamage;
    private float realDamage;
    protected int addDamage;
    public virtual int DealRealDamage(float damage, IDamage target, Vector2 pos, bool showDamage = true, bool isSputtering = false)
    {
        slowTemp = isCritical ? SlowRate * CriticalPercentage : SlowRate;
        target.DamageStrategy.ApplyFrost(isSputtering ? FinalSlowPercentage * slowTemp : slowTemp);
        finalDamage = isCritical ? damage * CriticalPercentage : damage;
        if (isSputtering)
            finalDamage *= FinalSplashPercentage;
        target.DamageStrategy.ApplyDamage(finalDamage, out realDamage, this);
        addDamage = (int)realDamage;
        turretParent.Strategy.TotalDamage += addDamage;//防御塔伤害统计
        turretParent.Strategy.TurnDamage += addDamage;//回合伤害统计
        GameRes.TotalDamage += addDamage;// 总伤害统计
        if (showDamage)
            StaticData.Instance.ShowJumpDamage(pos, addDamage, isCritical);
        return addDamage;
    }

    public int DamageProcess(TargetPoint target, bool showDamage = true, bool isSputtering = false)
    {
        float damage = TriggerHitEffect(target.Enemy);
        return DealRealDamage(damage, target.Enemy, target.Position, showDamage, isSputtering);
    }

}
