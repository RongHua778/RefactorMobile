using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null,int index=0)
    {
        base.OnContentPass(enemy, content,index);
        if (content == null)
            content = this;
        if (((TrapContent)content).CoinGainThisTurn >= StaticData.BonusTrapMaxCoinPerTurn)
            return;
        int amount = Mathf.RoundToInt(2 * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify));
        ((TrapContent)content).CoinAnalysis += amount;
        ((TrapContent)content).CoinGainThisTurn += amount;
        StaticData.Instance.GainMoneyEffect(enemy.model.position, amount);
        enemy.DamageStrategy.TrapIntensify = 0;
    }

}
