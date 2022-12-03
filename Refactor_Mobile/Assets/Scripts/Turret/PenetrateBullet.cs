using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrateBullet : Bullet
{
    //private bool showDmg;
    //private float showDmgCounter;

    Vector3 initScale;
    public override BulletType BulletType => BulletType.Penetrate;
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
        initScale = transform.localScale;
        transform.localScale *= (1 + (SplashRange / (SplashRange + 8)) * 5) * (1 + turretParent.Strategy.FinalBulletSize);

        TriggerPrehit();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        transform.localScale = initScale;
    }
    public override bool GameUpdate()
    {

        return DistanceCheck(TargetPos);
    }


    public void FixedUpdate()
    {
        MoveTowardsRig(TargetPos);
    }


    TargetPoint tTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tTarget = collision.GetComponent<TargetPoint>();
        if (tTarget)
            DamageProcess(collision.GetComponent<TargetPoint>(), true, true);

    }


}
