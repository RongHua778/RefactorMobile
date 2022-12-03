using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fission : Solider
{

    public override EnemyType EnemyType => EnemyType.Fission;

    public override void OnDie()
    {
        if (!isOutTroing)//离开时不分裂
        {
            Divide();
        }
        base.OnDie();
    }

    private void Divide()
    {
        for (int i = 0; i < 2; i++)
        {
            Enemy fissionSmall = GameManager.Instance.SpawnEnemy(EnemyType.Fisson_Small, PointIndex, Intensify / 3, DmgResist, BoardSystem.shortestPoints);
            fissionSmall.Progress = Mathf.Clamp((Progress + Random.Range(-0.2f, 0.2f)), 0, 1);
        }
    }

}
