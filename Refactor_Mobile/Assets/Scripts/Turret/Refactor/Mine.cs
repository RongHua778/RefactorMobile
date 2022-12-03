using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine :Bullet
{
    public override BulletType BulletType => BulletType.Self;

    private BasicTile mineTile;
    private ParticalControl effect;

    private Collider2D[] hits;
    private int hitSize;

    private bool isDmgIncrease = false;
    public bool IsDmgIncrease { get => isDmgIncrease; set => isDmgIncrease = value; }
    private float timeCounter;
    private float dmgIncreaseInterval=1f;
    private float dmgIncreased;
    private float maxDmgIncreased = 10f;
    private float dmgIncreasePerSecond = 0.1f;
    private void Awake()
    {
        hits = new Collider2D[20];
    }
    public void SetAtt(MinerBullet uBullet)
    {
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
        this.mineTile = uBullet.TargetTile;
        //transform.localScale = Vector3.one * SplashRange * 2f;

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
        effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, SplashRange * 2) * Vector3.one;
        effect.PlayEffect();
        ObjectPool.Instance.UnSpawn(this);
    }

    public override bool GameUpdate()
    {
        DmgIncrease();
        return true;
    }

    private void DmgIncrease()
    {
        if (!IsDmgIncrease)
            return;
        if (dmgIncreased > maxDmgIncreased)
            return;
        timeCounter += Time.deltaTime;
        if (timeCounter > dmgIncreaseInterval)
        {
            BulletDamageIntensify += dmgIncreasePerSecond;
            dmgIncreased += dmgIncreasePerSecond;
            timeCounter = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.GetComponent<TargetPoint>())
        {
            TriggerDamage();
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        IsDmgIncrease = false;
        dmgIncreased = 0f;
        mineTile.hasMine = false;

    }

}
