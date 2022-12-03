using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Tanker
{
    public override EnemyType EnemyType => EnemyType.Leader;
    protected override void SetStrategy()
    {
        DamageStrategy = new LeaderStrategy(this);
    }
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        //DamageStrategy.ApplyBuffDmgIntensify(-Mathf.Min(1f, 0.15f * ((GameRes.CurrentWave + 1) / 20)));
        BuffInfo buff = new BuffInfo(EnemyBuffName.Invisible, 1);
        DamageStrategy.ApplyBuff(buff);
    }

}
