using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restorer : Enemy
{
    public override EnemyType EnemyType => EnemyType.Restorer;
    protected override void SetStrategy()
    {
        DamageStrategy = new RestorerStrategy(this);
    }

}
