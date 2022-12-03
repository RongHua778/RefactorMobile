using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImitateTrap : TrapContent
{
    public override long DamageAnalysis { get => 0; set => base.DamageAnalysis = value; }
    public override int CoinAnalysis { get => 0; set => base.CoinAnalysis = value; }
    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        if (content == null)
            content = this;

        if (enemy.PassedTraps.Count > 0)
        {
            index++;
            if (index > enemy.PassedTraps.Count)
            {
                Debug.Log("¸´ÖÆÏİÚåÒç³ö");
                return;
            }
            TrapContent trap = enemy.PassedTraps[enemy.PassedTraps.Count - index];
            if (trap != content)
                trap.OnContentPass(enemy, content, index);

        }
        if (content.GetComponent<BlinkTrap>())
        {
            if (!BlinkedEnemy.Contains(enemy))
                BlinkedEnemy.Add(enemy);
        }

    }

    public override void ClearTurnData()
    {
        base.ClearTurnData();

    }
}
