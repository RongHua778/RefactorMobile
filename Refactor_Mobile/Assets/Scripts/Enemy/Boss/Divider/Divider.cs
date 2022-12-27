using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Boss
{
    [SerializeField] protected int springs = default;
    [SerializeField] protected EnemyType SpawnEnemyType = default;
    protected override bool ShakeCam => false;
    public override EnemyType EnemyType => EnemyType.Divider;

    public override void OnDie()
    {
        if (!isOutTroing)
            GetSprings();
        base.OnDie();
        //LevelManager.Instance.SetAchievement("ACH_DIVIDER");
    }

    protected void GetSprings()
    {
        for (int i = 0; i < springs; i++)
        {
            SpawnEnemy();
        }
    }

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        ShowBossText(0.5f);
    }

    private void SpawnEnemy()
    {
        if (SpawnEnemyType == EnemyType.None)
            return;
        Divider divider = GameManager.Instance.SpawnEnemy(SpawnEnemyType, PointIndex, Intensify / 2,DmgResist, BoardSystem.shortestPoints) as Divider;
        divider.Progress = Mathf.Clamp((Progress + Random.Range(-0.2f, 0.2f)), 0, 1);
    }
}
