using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier2 : AircraftCarrier
{
    public override EnemyType EnemyType => EnemyType.AircraftCarrier2;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        BuffInfo buff = new BuffInfo(EnemyBuffName.Invisible, 1);
        DamageStrategy.ApplyBuff(buff);
    }
    //public override void OnDie()
    //{
    //    base.OnDie();
    //    if (!LevelManager.Instance.LevelEnd)
    //        LevelManager.Instance.SetAchievement("ACH_DRAGON");
    //}
}
