using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snow : RecipeRefactor
{
    protected override void Shoot()
    {
        PlayAudio(ShootClip, false);
        turretAnim.SetTrigger("Attack");
        SnowBullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as SnowBullet;
        bullet.transform.position = shootPoint.position;
        Vector2 pos;
        if (Strategy.RangeType == RangeType.Line)
        {
            pos = (Vector2)shootPoint.position + (Vector2)transform.up * Strategy.FinalRange;
        }
        else
        {
            Vector2 dir = Target[0].transform.position - transform.position;
            pos = (Vector2)shootPoint.position + dir.normalized * Strategy.FinalRange;
        }
        bullet.Initialize(this, Target[0], pos);

    }


}
