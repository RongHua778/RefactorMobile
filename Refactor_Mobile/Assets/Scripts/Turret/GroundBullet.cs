using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBullet : Bullet
{
    public override BulletType BulletType => BulletType.Ground;
    Collider2D[] hits = new Collider2D[20];
    ParticalControl effect;
    ConcreteContent tContent;

    protected virtual float SplashBaseValue => SplashRange;

    public override bool GameUpdate()
    {
        RotateBullet(TargetPos);
        MoveTowards(TargetPos);
        return DistanceCheck(TargetPos);
    }

    public override void TriggerDamage()
    {
        if (SplashBaseValue > 0)
        {
            HitSize = Physics2D.OverlapCircleNonAlloc(transform.position, SplashBaseValue, hits, StaticData.EnemyLayerMask);

        }
        TriggerPrehit();
        if (SplashBaseValue > 0)
        {
            for (int i = 0; i < HitSize; i++)
            {
                DamageProcess(hits[i].GetComponent<TargetPoint>(), i < 9, true);//溅射前8个目标显示伤害跳字
            }
            //溅射施加BUFF功能
            //if (turretParent.Strategy.ContainTurretBuffSkill)
            //{
            //    HitSize = Physics2D.OverlapCircleNonAlloc(transform.position, SplashBaseValue, hits, StaticData.TurretLayerMask);
            //    for (int i = 0; i < HitSize; i++)
            //    {
            //        tContent = hits[i].GetComponent<ConcreteContent>();
            //        TriggerSplashEffect(tContent);
            //    }
            //}


        }
        effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, SplashBaseValue * 2) * Vector3.one;
        effect.PlayEffect();
        base.TriggerDamage();
    }
}
