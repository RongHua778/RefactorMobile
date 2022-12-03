using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomBard : RecipeRefactor
{
    private int bulletCount = 3;
    public int BulletCount { get => bulletCount; set => bulletCount = value; }

    private float bulletOffset = 0.3f;
    public float BulletOffset { get => bulletOffset; set => bulletOffset = value; }

    protected override void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        ShootEffect.Play();
        PlayAudio(ShootClip, false);
        var targets = Target.GetEnumerator();
        while (targets.MoveNext())
        {
            ShootMultiBullets(targets.Current);
        }
    }


    private void ShootMultiBullets(TargetPoint target)
    {
        float distance = Vector2.Distance(target.Position, transform.position);
        for (int i = 0; i < BulletCount; i++)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as Bullet;
            bullet.transform.position = shootPoint.position;
            Vector2 posOffset = Random.insideUnitCircle.normalized * distance * BulletOffset;
            bullet.Initialize(this, target, target.Position + posOffset);
            bullet.BulletSpeed *= Random.Range(0.9f, 1.1f);
        }
    }

    public override void OnUnSpawn()
    {
        BulletCount = 3;
        BulletOffset = 0.2f;
        base.OnUnSpawn();
    }
}
