using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotary : RecipeRefactor
{
    float rotSpeed = 0;
    float RotSpeed { get => Mathf.Max(-720, rotSpeed * Strategy.FinalFireRate); set => rotSpeed = value; }

    public override void InitializeTurret(StrategyBase strategy)
    {
        base.InitializeTurret(strategy);
        Strategy.CheckAngle = 360f;
    }
    protected override void RotateTowards()
    {

    }


    public override bool GameUpdate()
    {
        base.GameUpdate();
        SelfRotateControl();
        return true;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        rotTrans.rotation = Quaternion.identity;
        RotSpeed = 120f;
    }

    private void SelfRotateControl()
    {
        rotTrans.Rotate(Vector3.forward * -RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        //bullet.transform.position = 0.3f * Random.insideUnitCircle.normalized + (Vector2)transform.position;
        //Vector2 targetPos = (bullet.transform.position - transform.position).normalized * (Strategy.FinalRange + 0.5f) + transform.position;
        PlayAudio(ShootClip, false);
        bullet.transform.position = shootPoint.position;
        bullet.Initialize(this, Target[0], transform.position);
    }

}
