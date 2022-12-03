using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        base.OnContentPass(enemy, content, index);
        float stunTime = (0.5f + (enemy.PassedTraps.Count - 1) * 0.2f) * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify);
        enemy.DamageStrategy.StunTime += stunTime;
        enemy.DamageStrategy.TrapIntensify = 0;
    }

}