using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMarkTrap : TrapContent
{


    public override void OnContentPass(Enemy enemy, GameTileContent content = null,int index=0)
    {
        base.OnContentPass(enemy, content,index);
        BuffInfo buff = new BuffInfo(EnemyBuffName.TileBaseDamageIntensify, (1 + (int)TrapIntensify + enemy.DamageStrategy.TrapIntensify));
        enemy.DamageStrategy.ApplyBuff(buff);

    }


}
