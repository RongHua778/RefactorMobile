using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheetah : Boss
{
    public override EnemyType EnemyType => EnemyType.Cheetah;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        ShowBossText(1);
    }
    protected override void SetStrategy()
    {
        DamageStrategy = new CheetahStrateygy(this);
    }
}
