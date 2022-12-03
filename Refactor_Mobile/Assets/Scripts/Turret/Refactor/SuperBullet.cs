using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SuperBullet : TargetBullet
{
    Collider2D[] detectTarget;
    private List<Collider2D> listedTargets;
    public int BonuceTimes = -1;
    public bool BonuceSplash { get; set; }
    public float BounceSplashValue { get; set; }


    public void SetAtt(SuperBullet uBullet, TargetPoint target)
    {
        //弹射不触发射击和射击后效果
        this.Target = target;
        this.TargetPos = target.Position;
        this.turretParent = uBullet.turretParent;
        this.turretEffects = uBullet.turretParent.Strategy.TurretSkills;
        this.turretGlobalSkills = uBullet.turretParent.Strategy.GlobalSkills;


        this.BaseAttack = uBullet.BaseAttack;
        this.AttackIntensify = uBullet.AttackIntensify;
        this.BulletSpeed = uBullet.BulletSpeed;
        this.SplashRange = uBullet.SplashRange;
        this.CriticalRate = uBullet.CriticalRate;
        this.CriticalPercentage = uBullet.CriticalPercentage;
        this.SlowRate = uBullet.SlowRate;
        this.SplashPercentage = uBullet.SplashPercentage;
        this.SlowPercentage = uBullet.SlowPercentage;
        this.BulletDamageIntensify = uBullet.BulletDamageIntensify;
        this.BulletEffectIntensify = uBullet.BulletEffectIntensify;

    }
    public override void TriggerDamage()
    {
        if (BonuceTimes > 0)
        {
            //int hit = Physics2D.OverlapCircleNonAlloc(transform.position, 2.5f, detectTarget, StaticData.EnemyLayerMask);
            //if (hit == 1 && Target != null)
            //{
            //    //弹到自身啥也不做
            //}
            //else if (hit > 0)
            //{
            //    TargetPoint bounceTarget = null;
            //    if (Target != null)//如果当前目标没死，则将其排除
            //    {
            //        for (int i = 0; i < hit; i++)
            //        {
            //            if (detectTarget[i] == Target.GetComponent<Collider2D>())
            //            {
            //                bounceTarget = detectTarget[1 - i].GetComponent<TargetPoint>();
            //                break;
            //            }

            //        }
            //    }
            //    else
            //    {
            //        bounceTarget = detectTarget[0].GetComponent<TargetPoint>();
            //    }
            //    SuperBullet bullet = ObjectPool.Instance.Spawn(this) as SuperBullet;
            //    bullet.transform.position = transform.position;

            //    bullet.BonuceTimes = BonuceTimes - 1;
            //    bullet.SetAtt(this, bounceTarget);
            //    if (BonuceSplash)
            //    {
            //        bullet.SplashPercentage += BounceSplashValue;
            //    }
            //}

            //if (hit > 0)
            //{
            //    TargetPoint bounceTarget = null;
            //    if (Target != null)//如果当前目标没死，则将其排除
            //    {
            //        if (hit == 1)   //只有目标自身
            //        {

            //        }
            //        for (int i = 0; i < hit; i++)
            //        {
            //            if (detectTarget[i] == Target.GetComponent<Collider2D>())
            //            {
            //                bounceTarget = detectTarget[hit - i].GetComponent<TargetPoint>();
            //                break;
            //            }

            //        }
            //    }
            //    else
            //    {
            //        bounceTarget = detectTarget[0].GetComponent<TargetPoint>();
            //    }
            //    SuperBullet bullet = ObjectPool.Instance.Spawn(this) as SuperBullet;
            //    bullet.transform.position = transform.position;

            //    bullet.BonuceTimes = BonuceTimes - 1;
            //    bullet.SetAtt(this, bounceTarget);
            //    if (BonuceSplash)
            //    {
            //        bullet.SplashPercentage += BounceSplashValue;
            //    }


            //    //listedTargets = detectTarget.ToList();
            //    //if (Target != null)//飞行过程中敌人死亡
            //    //    listedTargets.Remove(Target.GetComponent<Collider2D>());
            //    //if (listedTargets.Count > 0)
            //    //{
            //    //    TargetPoint target = listedTargets[0].GetComponent<TargetPoint>();
            //    //    SuperBullet bullet = ObjectPool.Instance.Spawn(this) as SuperBullet;
            //    //    bullet.transform.position = transform.position;

            //    //    bullet.BonuceTimes = BonuceTimes - 1;
            //    //    bullet.SetAtt(this, target);
            //    //    if (BonuceSplash)
            //    //    {
            //    //        bullet.SplashPercentage += BounceSplashValue;
            //    //    }
            //    //}
            //}

            //暂时关闭内容
            detectTarget = Physics2D.OverlapCircleAll(transform.position, 2.5f, StaticData.EnemyLayerMask);

            listedTargets = detectTarget.ToList();
            if (listedTargets.Count > 0)
            {
                if (Target != null)//飞行过程中敌人死亡
                    listedTargets.Remove(Target.GetComponent<Collider2D>());
                if (listedTargets.Count > 0)
                {
                    TargetPoint target = listedTargets[0].GetComponent<TargetPoint>();
                    SuperBullet bullet = ObjectPool.Instance.Spawn(this) as SuperBullet;
                    bullet.transform.position = transform.position;

                    bullet.BonuceTimes = BonuceTimes - 1;
                    bullet.SetAtt(this, target);
                    bullet.BonuceSplash = this.BonuceSplash;
                    bullet.BounceSplashValue = this.BounceSplashValue;
                    if (BonuceSplash)
                    {
                        bullet.SplashRange += BounceSplashValue*this.BulletEffectIntensify;
                    }
                }
            }


        }
        base.TriggerDamage();

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        BonuceTimes = -1;
        BonuceSplash = false;
        BounceSplashValue = 0f;
    }
}
