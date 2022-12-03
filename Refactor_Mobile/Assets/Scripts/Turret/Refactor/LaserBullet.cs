using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : LineBullet
{
    private float distanceCounter = 0f;
    private bool travelIncrease = false;
    private float increaseValue = 0.4f;
    public void SetTravelIncrease(bool value)
    {
        travelIncrease = value;
    }
    public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        base.Initialize(turret, target, pos);
        distanceCounter = 0;
    }
    public override bool GameUpdate()
    {
        IncreaseDmgWhileTravel();
        return base.GameUpdate();
    }

    private void IncreaseDmgWhileTravel()
    {
        if (!travelIncrease)
            return;
        float distance = Vector2.Distance(transform.position, turretParent.transform.position);
        if ((distance - distanceCounter) >= 1)
        {
            BulletDamageIntensify += increaseValue;
            distanceCounter += 1;
        }
    }

}
