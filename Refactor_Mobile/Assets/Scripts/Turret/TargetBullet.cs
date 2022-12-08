using UnityEngine;

public class TargetBullet : Bullet
{
    public override BulletType BulletType => BulletType.Target;
    private Collider2D[] hits = new Collider2D[20];
    TargetPoint cTarget;
    ConcreteContent tContent;
    ParticalControl effect;

    //private void Start()
    //{
    //    hits = new Collider2D[20];
    //}
    protected override Vector2 TargetPos
    {
        get => Target == null ? base.TargetPos : Target.Position;
        set => base.TargetPos = value;
    }

    public override bool GameUpdate()
    {
        if (Target != null && !Target.gameObject.activeSelf)//飞到地面
        {
            TargetPos = Target.transform.position;
            Target = null;
        }
        RotateBullet(TargetPos);
        MoveTowards(TargetPos);
        return DistanceCheck(TargetPos);
    }



    public override void TriggerDamage()
    {
        int totalDamage = 0;
        if (SplashRange > 0)
        {
            HitSize = Physics2D.OverlapCircleNonAlloc(transform.position, SplashRange, hits, StaticData.EnemyLayerMask);
        }
        TriggerPrehit();
        if (SplashRange > 0)
        {
            for (int i = 0; i < HitSize; i++)
            {
                cTarget = hits[i].GetComponent<TargetPoint>();
                if (cTarget == Target)
                {
                    totalDamage += DamageProcess(cTarget, true);
                }
                else
                {
                    totalDamage += DamageProcess(cTarget, i < 9, true);//溅射前8个目标显示伤害跳字
                }

            }
        }
        else
        {
            if (Target != null)
            {
                totalDamage += DamageProcess(Target, true);
            }
        }
        GameRes.MaxSingleDamage = totalDamage;

        effect = ObjectPool.Instance.Spawn(SputteringEffect) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, SplashRange * 2) * Vector3.one;
        effect.PlayEffect();
        base.TriggerDamage();
    }

}
