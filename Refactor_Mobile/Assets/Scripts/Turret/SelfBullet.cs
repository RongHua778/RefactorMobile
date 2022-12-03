using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBullet : Bullet
{
    public override BulletType BulletType => BulletType.Self;


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
    }

    public override bool GameUpdate()
    {
        return true;
    }

    public override void ReclaimBullet()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<TargetPoint>())
        {
            DamageProcess(collision.GetComponent<TargetPoint>(), true, true);
        }
        else
        {
            ConcreteContent concrete = collision.GetComponent<ConcreteContent>();
            if (concrete == null)
                return;
            if (concrete == turretParent)
                return;
            //if (turretParent != null && turretParent.Strategy.ContainTurretBuffSkill)
            //{
            //    TriggerSplashEffect(concrete);
            //}
            if (!concrete.Activated)
            {
                concrete.UnFrost();
            }
        }

    }


}
