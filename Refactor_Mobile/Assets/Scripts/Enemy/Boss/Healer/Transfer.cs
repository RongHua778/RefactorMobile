using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Transfer : Runner
{
    public override EnemyType EnemyType => EnemyType.Transfer;
    int blink = 1;
    int blinkDistance;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        blink = 1;
        blinkDistance = Mathf.Min(8, 1 + ((GameRes.CurrentWave + 1) / 20));
    }


    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        if (isOutTroing)
            return;
        if (blink >= 1 && DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.5f)
        {
            blink--;
            Blink(blinkDistance);

        }


    }

    protected override void PrepareNextState()
    {
        base.PrepareNextState();
        enemyCol.transform.localPosition = Vector3.zero;
        //model.localScale = Vector3.one;
    }


}
