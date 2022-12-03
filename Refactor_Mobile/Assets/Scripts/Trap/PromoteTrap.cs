using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        base.OnContentPass(enemy, content, index);
        enemy.DamageStrategy.TrapIntensify += Mathf.RoundToInt(1 * (1 + TrapIntensify));
    }

}