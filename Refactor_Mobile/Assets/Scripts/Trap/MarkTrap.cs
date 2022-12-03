using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null,int index=0)
    {
        base.OnContentPass(enemy, content, index);
        BuffInfo buff = new BuffInfo(EnemyBuffName.TileCountStun, (1 + enemy.DamageStrategy.TrapIntensify + (int)TrapIntensify));
        enemy.DamageStrategy.ApplyBuff(buff);
        enemy.DamageStrategy.TrapIntensify = 0;
        //Debug.LogWarning("Œ¥ µœ÷");

    }


}
