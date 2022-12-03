using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirerArea : Bullet
{
    public override BulletType BulletType => BulletType.Self;
    private BoxCollider2D hitAreaCol;
    private Collider2D[] hitResults;
    private ContactFilter2D filter;
    [SerializeField] private ParticleSystem fireEffect;

    private void Awake()
    {
        hitAreaCol = this.GetComponent<BoxCollider2D>();
        hitResults = new Collider2D[10];
        filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = StaticData.EnemyLayerMask
        };
        SetAngle(30f);
    }

    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        this.Target = target;
        this.TargetPos = pos ?? target.Position;
        this.turretParent = turret;
        this.turretEffects = turret.Strategy.TurretSkills;
        this.turretGlobalSkills = turret.Strategy.GlobalSkills;


        TriggerShootEffect(target.Enemy);
        SetAttribute(turret);
        TriggerAfterShoot(target.Enemy);
        TriggerPrehit();
        TriggerHit();
    }

    public override bool GameUpdate()
    {
        return true;
    }

    public override void ReclaimBullet()
    {

    }

    private void TriggerHit()
    {
        int hits = Physics2D.OverlapCollider(hitAreaCol, filter, hitResults);

        //Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(0.5f, 1.5f), hitResults);
        for (int i = 0; i < hits; i++)
        {
            if (hitResults[i].GetComponent<TargetPoint>())
            {
                DamageProcess(hitResults[i].GetComponent<TargetPoint>(), true, true);
            }
        }
    }

    public void SetAngle(float value)
    {
        var shape = fireEffect.shape;
        shape.angle = value;
        hitAreaCol.size = new Vector2(value/3f, 1.5f);
    }



}
