using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Frost : Weapon
{
    [SerializeField] private Knight_FrostBullet frostBulletPrefab = default;
    protected override void TriggerWeapon()
    {
        base.TriggerWeapon();

        int bulletCount = Mathf.Min(3, 1 + ((GameRes.CurrentWave + 1) / 20));
        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * Mathf.Min(8f, 1.5f + 1f * ((GameRes.CurrentWave + 1) / 20));
            Knight_FrostBullet bullet = ObjectPool.Instance.Spawn(frostBulletPrefab) as Knight_FrostBullet;
            bullet.transform.position = transform.position;
            float freezeTime = Mathf.Min(10f, 1f + ((GameRes.CurrentWave + 1) / 20));
            float freezeRange = Mathf.Min(8f, 1f + ((GameRes.CurrentWave + 1) / 20));
            bullet.SetBullet(pos, freezeRange, freezeTime);
        }
    }

}
