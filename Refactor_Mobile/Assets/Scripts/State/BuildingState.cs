using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : BattleOperationState
{
    public override StateName StateName => StateName.BuildingState;
    private BoardSystem m_Board;
    private FuncUI m_FuncUI;
    private ShapeSelectUI m_ShapeUI;
    private TechSelectUI m_TechUI;
    public BuildingState(GameManager gameManager, BoardSystem gameBoard,FuncUI funcUI,ShapeSelectUI shapeUI,TechSelectUI techUI) : base(gameManager)
    {
        m_Board = gameBoard;
        m_FuncUI = funcUI;
        m_ShapeUI = shapeUI;
        m_TechUI = techUI;
    }

    public override IEnumerator EnterState()
    {
        m_Board.TransparentPath(new Color(0.2f, 1f, 1f, 0.5f), 0.3f);
        Sound.Instance.PlayBg("Music_Preparing");
        yield return new WaitForSeconds(0.3f);
        gameManager.OperationState = this;

        if (!m_ShapeUI.IsVisible() && !m_TechUI.IsVisible())
            m_FuncUI.Show();
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
