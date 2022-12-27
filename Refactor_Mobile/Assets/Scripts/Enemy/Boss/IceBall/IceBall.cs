using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceBall : Boss
{
    public override EnemyType EnemyType => EnemyType.IceBall;
    public float freezeRange;
    public float freezeTime;
    private Tween tween;
    private float frostCounter;
    [SerializeField] float frostCD = default;
    [SerializeField] Transform rotateObj = default;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        frostCounter = frostCD;
        Vector3 rot = new Vector3(0, 0, 360);
        tween = rotateObj.DORotate(rot, 4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();
    }
    //public override DirectionChange DirectionChange
    //{
    //    get => base.DirectionChange;
    //    set
    //    {
    //        base.DirectionChange = value;
    //        if (value != DirectionChange.None)
    //        {
    //            StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);
    //        }
    //    }
    //}

    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        frostCounter -= Time.deltaTime;
        if (frostCounter <= 0)
        {
            frostCounter = frostCD;
            StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);
            ShowBossText(0.15f);
        }

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        tween.Kill();
    }

    //public override void OnDie()
    //{
    //    base.OnDie();
    //    LevelManager.Instance.SetAchievement("ACH_BEAR");
    //}

}
