using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapider : RecipeRefactor
{
    public float BulletOffset => 0.02f;
    protected override void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        ShootEffect.Play();
        PlayAudio(ShootClip, false);
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        float offset = Random.Range(-BulletOffset, BulletOffset);
        bullet.transform.position = (Vector2)(shootPoint.position + offset * shootPoint.right);
        Vector2 dir = bullet.transform.position - transform.position;
        Vector2 pos = (Vector2)shootPoint.position + dir.normalized * (Strategy.FinalRange + 0.25f);
        bullet.Initialize(this, Target[0], pos);
    }


}
