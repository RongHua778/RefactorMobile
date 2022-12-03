using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boomerrang : RecipeRefactor
{
    float rotSpeed = -360;
    // public float FlyTime => 1.5f / (Strategy.FinalFireRate / 0.2f);
    public float FlyTime => (Strategy.FinalRange / (Strategy.FinalRange + 4f)) * 4f;//根据攻击范围决定飞行时间
    private bool bulletOut;
    float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    private SelfBullet cannonBullet;
    private Vector3 bulletInitScale;
    //public float SplashDmgValue;

    protected override void RotateTowards()
    {

    }

    protected override bool AngleCheck()//回旋镖必须360度确认
    {
        var angleCheck = Quaternion.Angle(rotTrans.rotation, look_Rotation);
        if (angleCheck < 360f)
        {
            return true;
        }
        return false;
    }

    protected override void FireProjectile()
    {
        if (bulletOut)
            return;
        base.FireProjectile();
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
        CannonSprite.transform.rotation = Quaternion.identity;
        cannonBullet = CannonSprite.GetComponent<SelfBullet>();
    }

    private void SelfRotateControl()
    {
        cannonBullet.transform.Rotate(Vector3.forward * RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootCor());
    }
    Vector2 dir;
    Vector3 pos;
    Vector3 initPos;

    private IEnumerator ShootCor()
    {
        bulletOut = true;
        cannonBullet.Initialize(this, Target[0]);
        bulletInitScale = cannonBullet.transform.localScale;
        PlayAudio(ShootClip, false);

        initPos = cannonBullet.transform.position;
        if (Strategy.RangeType == RangeType.Line)
        {
            pos = initPos + transform.up * Strategy.FinalRange;
        }
        else
        {
            dir = Target[0].transform.position - transform.position;
            pos = initPos + (Vector3)dir.normalized * Strategy.FinalRange;
        }

        DOTween.To(() => RotSpeed, x => RotSpeed = x, -720, FlyTime).SetEase(Ease.OutCubic);
        cannonBullet.transform.DOMove(pos, FlyTime).SetEase(Ease.OutCubic);
        cannonBullet.transform.DOScale(bulletInitScale * (1 + (Strategy.FinalSplashRange / (Strategy.FinalSplashRange + 8)) * 5) * (1 + Strategy.FinalBulletSize), FlyTime).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(FlyTime);
        //返回阶段,科技技能效果
        cannonBullet.BulletDamageIntensify += ((BoomerangStrategy)Strategy).DmgBonusWhileReturn;
        DOTween.To(() => RotSpeed, x => RotSpeed = x, -360, FlyTime).SetEase(Ease.InCubic);
        cannonBullet.transform.DOMove(initPos, FlyTime).SetEase(Ease.InCubic);
        cannonBullet.transform.DOScale(bulletInitScale, FlyTime).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(FlyTime);
        bulletOut = false;

    }

}
