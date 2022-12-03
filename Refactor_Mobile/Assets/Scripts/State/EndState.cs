using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BattleOperationState
{
    public EndState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.LoseState;

    public override IEnumerator EnterState()
    {
        gameManager.OperationState = this;
        Sound.Instance.PlayBg("lastwave");
        yield return null;
    }
}
