using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : Bullet
{
    RaycastHit2D[] hitTargets = new RaycastHit2D[10];
    protected float hitInterval = 0.05f;
    private float hitTimeCounter;
    public override BulletType BulletType => BulletType.Penetrate;
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
        hitInterval = 6f / BulletSpeed * 0.05f;
        TriggerPrehit();
    }
    public override bool GameUpdate()
    {
        DealDamageForward();
        return DistanceCheck(TargetPos);
    }


    public void FixedUpdate()
    {
        MoveTowardsRig(TargetPos);
    }

    private void DealDamageForward()
    {
        hitTimeCounter += Time.deltaTime;
        if (hitTimeCounter > hitInterval)
        {
            int hit = Physics2D.RaycastNonAlloc(transform.position, transform.forward, hitTargets, 0.1f, StaticData.EnemyLayerMask);
            for (int i = 0; i < hit; i++)
            {
                TargetPoint target = hitTargets[i].transform.GetComponent<TargetPoint>();
                DamageProcess(target, true, true);

            }
            hitTimeCounter = 0;
        }
    }
}
