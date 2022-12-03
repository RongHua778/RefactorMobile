using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingState : BattleOperationState
{
    public override StateName StateName => StateName.PickingState;
    private FuncUI m_FuncUI;
    public PickingState(GameManager gameManager,FuncUI funcUI) : base(gameManager)
    {
        m_FuncUI = funcUI;
    }
    public override IEnumerator EnterState()
    {
        gameManager.OperationState = this;
        m_FuncUI.Hide();
        yield break;
    }
    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
