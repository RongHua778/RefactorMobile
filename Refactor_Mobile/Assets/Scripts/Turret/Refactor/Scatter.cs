using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scatter : RecipeRefactor
{
    private const float ShootPointSideOffset = 0.16f;
    private float fireInterval;
    //private const float ShootInterval = 0.12f;
    [SerializeField] ParticleSystem shootEffect2 = default;
    [SerializeField] Transform shootPoint1 = default;
    [SerializeField] Transform shootPoint2 = default;

    bool shootDir = false;
    //同时攻击多个目标
    protected override void Shoot()
    {
        StartCoroutine(ShootCor());
    }

    IEnumerator ShootCor()
    {
        fireInterval = 0.25f / (Strategy.FinalFireRate * Strategy.FinalTargetCount);
        foreach (TargetPoint target in Target.ToList())
        {
            if (!target.gameObject.activeSelf)
                continue;
            turretAnim.SetTrigger("Attack");
            PlayAudio(ShootClip, false);
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = shootDir ? shootPoint1.position : shootPoint2.position;
            ParticleSystem playParticle = shootDir ? ShootEffect : shootEffect2;
            playParticle.Play();
            shootDir = !shootDir;
            bullet.Initialize(this, target);
            yield return new WaitForSeconds(fireInterval);
        }
    }

    public override void SetGraphic()
    {
        base.SetGraphic();
        shootPoint1.position = (Vector2)shootPoint.position + (Vector2)shootPoint.right * ShootPointSideOffset;
        shootPoint2.position = (Vector2)shootPoint.position - (Vector2)shootPoint.right * ShootPointSideOffset;

    }
}
