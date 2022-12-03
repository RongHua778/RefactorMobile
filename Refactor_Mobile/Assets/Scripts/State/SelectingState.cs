using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingState : BattleOperationState
{
    public override StateName StateName => StateName.SelectingState;

    public SelectingState(GameManager gameManager) : base(gameManager)
    {
    }

    public override IEnumerator EnterState()
    {
        gameManager.OperationState = this;
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
