using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinker : Boss
{
    public override EnemyType EnemyType => EnemyType.Blinker;
    int blink;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        blink = 3;
    }

    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.75f && blink >= 3)
        {
            blink--;
            ShowBossText(0.5f);
            Blink(4);

        }
        else if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.5f && blink >= 2)
        {
            blink--;

            ShowBossText(0.5f);
            Blink(4);


        }
        else if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.25f && blink >= 1)
        {
            blink--;

            ShowBossText(0.5f);
            Blink(4);
        }
    }

    public override void OnDie()
    {
        base.OnDie();
        LevelManager.Instance.SetAchievement("ACH_BAT");
    }



}
