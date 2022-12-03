using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null,int index=0)
    {
        base.OnContentPass(enemy, content,index);
        enemy.DamageStrategy.ApplyBuffDmgIntensify(0.05f * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify));
        enemy.DamageStrategy.TrapIntensify = 0;
    }


}
