using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bipolar : Boss
{
    public override EnemyType EnemyType => EnemyType.Bipolar;
    public static Bipolar FirstBipolar;

    private Bipolar brother;
    private float synCounter;
    private float synInterval = 8f;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        if (FirstBipolar == null)
        {
            FirstBipolar = this;
            Bipolar bipolar = GameManager.Instance.SpawnEnemy(this.EnemyType, 0, intensify, dmgResist, BoardSystem.GetReversePath()) as Bipolar;
            bipolar.pathTiles = bipolar.pathTiles.ToList();
            bipolar.pathTiles.Reverse();
            brother = bipolar;
            bipolar.brother = this;
        }
        ShowBossText(1);
    }

    public override bool GameUpdate()
    {
        if (brother != null)
        {
            synCounter += Time.deltaTime;
            if (synCounter >= synInterval)
            {
                float newHealth = Mathf.Max(brother.DamageStrategy.CurrentHealth, DamageStrategy.CurrentHealth);
                DamageStrategy.CurrentHealth = newHealth;
                synCounter = 0;
            }
        }


        return base.GameUpdate();

    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (brother == null)
        {
            FirstBipolar = null;
        }
        else
        {
            brother.brother = null;
            brother = null;
        }
    }
}
