using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhinoceros : Boss
{
    public override EnemyType EnemyType => EnemyType.Rhinoceros;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        BuffInfo buffInfo = new BuffInfo(EnemyBuffName.TileDamageResistBuff, 1);
        DamageStrategy.ApplyBuff(buffInfo);
        ShowBossText(1);

    }
}
