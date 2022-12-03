using System.Collections.Generic;
using UnityEngine;

public class UltraLava : Bullet
{


    private float bulletLastTime = 5f;
    private float triggerInterval = 1f;
    //private float spalshDmgIncreasePerSecond = 0f;
    public float DmgBonusIncreasePersecond { get; set; }
    private float timeCounter;
    private float triggerCounter;

    private Collider2D[] hits;
    private int hitSize;
    ConcreteContent tContent;

    public override BulletType BulletType => BulletType.Self;

    public float BulletLastTime { get => bulletLastTime; set => bulletLastTime = value; }

    private void Awake()
    {
        hits = new Collider2D[20];
    }
    //public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    //{
    //    base.Initialize(turret, target, pos);
    //    //不触发射击和射击后效果
    //    //this.Target = target;
    //    //this.TargetPos = pos ?? target.Position;
    //    //this.turretParent = turret;
    //    //this.turretEffects = turret.Strategy.TurretSkills;
    //    //this.turretGlobalSkills = turret.Strategy.GlobalSkills;

    //    transform.localScale = Vector3.one * SplashRange * 2f;
    //    timeCounter = 0;
    //    triggerCounter = 0;
    //}
    public void SetAtt(UltraBullet uBullet)
    {
        this.Target = uBullet.Target;
        this.TargetPos = uBullet.Target.Position;
        this.turretParent = uBullet.turretParent;
        this.turretEffects = uBullet.turretParent.Strategy.TurretSkills;
        this.turretGlobalSkills = uBullet.turretParent.Strategy.GlobalSkills;


        this.BaseAttack = uBullet.BaseAttack;
        this.SplashRange = uBullet.SplashRange;
        this.CriticalRate = uBullet.CriticalRate;
        this.CriticalPercentage = uBullet.CriticalPercentage;
        this.SlowRate = uBullet.SlowRate;
        this.SplashPercentage = uBullet.SplashPercentage;
        this.SlowPercentage = uBullet.SlowPercentage;
        this.BulletDamageIntensify = uBullet.BulletDamageIntensify;
        this.BulletEffectIntensify = uBullet.BulletEffectIntensify;

        transform.localScale = Vector3.one * SplashRange * 2f;
        timeCounter = 0;
        triggerCounter = 0;
    }

    public override void TriggerDamage()
    {
        TriggerPrehit();
        if (SplashRange > 0)
        {
            hitSize = Physics2D.OverlapCircleNonAlloc(transform.position, SplashRange, hits, StaticData.EnemyLayerMask);
            for (int i = 0; i < hitSize; i++)
            {
                DamageProcess(hits[i].GetComponent<TargetPoint>(), true, true);
            }
        }

        //if (turretParent.Strategy.ContainTurretBuffSkill)
        //{
        //    HitSize = Physics2D.OverlapCircleNonAlloc(transform.position, SplashRange, hits, StaticData.TurretLayerMask);
        //    for (int i = 0; i < HitSize; i++)
        //    {
        //        tContent = hits[i].GetComponent<ConcreteContent>();
        //        TriggerSplashEffect(tContent);
        //    }
        //}
    }

    public override bool GameUpdate()
    {
        timeCounter += Time.deltaTime;
        triggerCounter += Time.deltaTime;
        //SplashPercentage+= Time.deltaTime * spalshDmgIncreasePerSecond;
        BulletDamageIntensify += Time.deltaTime * DmgBonusIncreasePersecond;
        if (triggerCounter > triggerInterval)
        {
            TriggerDamage();
            triggerCounter = 0;
        }
        if (timeCounter > BulletLastTime)
        {
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        return true;
    }


}
