using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner2 : Borner
{
    public override EnemyType EnemyType => EnemyType.Borner2;

    public override void OnDie()
    {
        base.OnDie();
        if (!LevelManager.Instance.LevelEnd)
            LevelManager.Instance.SetAchievement("ACH_NEST");
    }
}
