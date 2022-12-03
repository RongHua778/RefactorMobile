using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froster : Restorer
{
    public override EnemyType EnemyType => EnemyType.Froster;
    private float freezeRange;//20��-180����2-10
    private float freezeTime;//20��-80����2-6

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        freezeTime = Mathf.Min(10f, 1f + ((GameRes.CurrentWave + 1) / 20));
        freezeRange = Mathf.Min(6f, 1f + ((GameRes.CurrentWave + 1) / 20));
    }
    public override void OnDie()
    {
        base.OnDie();
        StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);// FrostTurrets();

    }

}
