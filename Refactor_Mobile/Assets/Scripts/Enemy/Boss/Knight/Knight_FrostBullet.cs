using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_FrostBullet : ReusableObject, IGameBehavior
{
    private Vector2 targetPos;
    private float bulletSpeed = 5f;
    private float frostRange;
    private float frostTime;

    public bool GameUpdate()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            StaticData.Instance.FrostTurretEffect(transform.position, frostRange, frostTime);
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        transform.position = Vector2.MoveTowards(transform.position,
            targetPos, bulletSpeed * Time.deltaTime);
        return true;
    }

    public void SetBullet(Vector2 targetPos, float frostRange, float frostTime)
    {
        this.targetPos = targetPos;
        this.frostRange = frostRange;
        this.frostTime = frostTime;
        this.bulletSpeed = Random.Range(3f, 6f);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }


}
