using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTrap : TrapContent
{
    private int Distance;

    //public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    //{
    //    if (content == null)
    //        content = this;
    //    if (enemy.PassedTraps.Contains((TrapContent)content))
    //        return;
    //    base.OnContentPass(enemy, content, index);
    //    Distance = Mathf.RoundToInt(2 * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify));
    //    enemy.Flash(Distance);
    //    enemy.DamageStrategy.TrapIntensify = 0;
    //}
    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        if (content == null)
            content = this;
        if (((TrapContent)content).BlinkedEnemy.Contains(enemy))
            return;
        base.OnContentPass(enemy, content, index);
        if (!BlinkedEnemy.Contains(enemy))
            BlinkedEnemy.Add(enemy);
        Distance = Mathf.RoundToInt(2 * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify));
        enemy.DamageStrategy.TrapIntensify = 0;
        enemy.Flash(Distance);

    }

}
