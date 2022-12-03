using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TurretContent : ConcreteContent
{


    private float nextAttackTime;
    public float NextAttackTime { get => nextAttackTime; set => nextAttackTime = value; }
    protected Quaternion look_Rotation;


    protected Bullet bulletPrefab;

    //**********美术，动画及音效
    protected Animator turretAnim;
    protected AudioSource audioSource;
    protected AudioClip ShootClip;
    [SerializeField] protected ParticleSystem ShootEffect = default;

    private const float invisibleDistance = 3.5f;


    protected override void Awake()
    {
        base.Awake();
        turretAnim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = Sound.Instance.effectMixer;
    }

    public virtual void InitializeTurret(StrategyBase strategy)
    {
        this.Strategy = strategy;
        this.Strategy.Concrete = this;

        rotTrans.localRotation = Quaternion.identity;
        bulletPrefab = Strategy.Attribute.Bullet;
        ShootClip = Strategy.Attribute.ShootSound;
        SetGraphic();
        GenerateRange();
        Activated = true;

    }


    protected virtual void PlayAudio(AudioClip clip, bool isAuto)
    {
        audioSource.volume = StaticData.EnvrionmentBaseVolume;
        audioSource.clip = clip;
        if (isAuto)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }
    }


    //设置不同等级的美术资源
    public override void SetGraphic()
    {
        shootPoint.transform.localPosition = Strategy.Attribute.TurretLevels[Strategy.Quality - 1].ShootPointOffset;
        CannonSprite.sprite = Strategy.Attribute.TurretLevels[Strategy.Quality - 1].CannonSprite;
    }

    //在塔被激活后每一帧都会调用的方法
    protected override void OnActivating()
    {
        base.OnActivating();
        if (TrackTarget() || AcquireTarget())
        {
            if (!Activated)
                return;
            RotateTowards();
            FireProjectile();
        }
    }




    protected virtual bool TrackTarget()
    {
        for (int i = 0; i < Target.Count; i++)
        {
            if (!Target[i].gameObject.activeSelf)
            {
                targetList.Remove(Target[i]);
                Target.Remove(Target[i]);
            }
            //else if (Target[i].Enemy.DamageStrategy.InVisible)
            //{
            //    if (Vector2.Distance(Target[i].Position, transform.position) > invisibleDistance)
            //    {
            //        Target.Remove(Target[i]);
            //    }
            //}
        }
        if (Target.Count < Strategy.FinalTargetCount)
            return false;

        return true;
    }
    private bool AcquireTarget()
    {
        if (targetList.Count <= 0)
            return false;
        else
        {
            Target.Clear();
            List<int> returnInt = StaticData.SelectNoRepeat(targetList.Count, Strategy.FinalTargetCount);
            var ints = returnInt.GetEnumerator();
            while (ints.MoveNext())
            {
                //if (targetList[ints.Current].Enemy.DamageStrategy.InVisible)
                //{
                //    if (Vector2.Distance(targetList[ints.Current].Position, transform.position) > invisibleDistance)
                //        continue;
                //}
                Target.Add(targetList[ints.Current]);
            }
            return Target.Count > 0;

        }
    }


    protected virtual void RotateTowards()
    {
        if (Target.Count == 0)
            return;
        var dir = Target[0].transform.position - rotTrans.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotTrans.rotation = Quaternion.LerpUnclamped(rotTrans.rotation, look_Rotation, Strategy.RotSpeed * Time.deltaTime);
    }

    protected virtual bool AngleCheck()
    {
        var angleCheck = Quaternion.Angle(rotTrans.rotation, look_Rotation);
        if (angleCheck < Strategy.CheckAngle)
        {
            return true;
        }
        return false;
    }

    protected virtual void FireProjectile()
    {
        if (Time.time - NextAttackTime > 1 / Strategy.FinalFireRate)
        {
            if (Target != null && AngleCheck())
            {
                Shoot();
            }
            else
            {
                return;
            }
            NextAttackTime = Time.time;
        }
    }

    protected virtual void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        ShootEffect.Play();
        PlayAudio(ShootClip, false);
        var targets = Target.GetEnumerator();
        while (targets.MoveNext())
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as Bullet;
            bullet.transform.position = shootPoint.position;
            bullet.Initialize(this, targets.Current);
        }
    }


    //content类重载*************


    public override void SaveContent(out ContentStruct contentSruct)
    {
        base.SaveContent(out contentSruct);
        contentSruct = m_ContentStruct;
        m_ContentStruct.TotalDamage = Strategy.TotalDamage.ToString();
    }


    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        if (value)
        {
            TipsManager.Instance.ShowTurreTips(this.Strategy, StaticData.LeftTipsPos, 0);
            GameEvents.Instance.TutorialTrigger(TutorialType.TurretSelect);
        }

    }





}
