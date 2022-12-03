using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : RecipeRefactor
{

    protected override void Shoot()
    {
        PlayAudio(ShootClip, false);
        turretAnim.SetTrigger("Attack");
        LaserBullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as LaserBullet;
        bullet.transform.position = shootPoint.position;
        Vector2 pos;
        if (Strategy.RangeType == RangeType.Line)
        {
            pos = (Vector2)shootPoint.position + (Vector2)transform.up * Strategy.FinalRange * 2;
        }
        else
        {
            Vector2 dir = Target[0].transform.position - transform.position;
            pos = (Vector2)shootPoint.position + dir.normalized * Strategy.FinalRange * 2;
        }
        bullet.Initialize(this, Target[0], pos);

    }


}
