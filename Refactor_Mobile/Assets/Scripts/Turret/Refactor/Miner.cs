using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : RecipeRefactor
{
    public List<BasicTile> PathTiles;
    public float DeployInterval { get; set; }

    protected override bool TrackTarget()
    {
        return true;
    }

    protected override void FireProjectile()
    {
        if (Time.time - NextAttackTime > DeployInterval)
        {
            Deploy();
            NextAttackTime = Time.time;
        }
    }
    public void Deploy()
    {
        Shoot();
    }
    protected override void Shoot()
    {
        //TODO:多重射击同时部署多个地雷
        List<BasicTile> noMineTiles = new List<BasicTile>();
        foreach (BasicTile tile in PathTiles)
        {
            if (tile.hasMine == false)
            {
                noMineTiles.Add(tile);
            }
        }
        if (noMineTiles.Count > 0)
        {
            BasicTile tile = noMineTiles[Random.Range(0, noMineTiles.Count)];
            turretAnim.SetTrigger("Attack");
            ShootEffect.Play();
            PlayAudio(ShootClip, false);
            MinerBullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as MinerBullet;
            bullet.transform.position = shootPoint.position;
            bullet.Initialize(this, null, tile.transform.position);
            bullet.TargetTile = tile;
            tile.hasMine = true;
        }

    }

}
