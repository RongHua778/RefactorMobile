using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : ISceneState
{
    public BattleState(SceneStateController Controller) : base(Controller)
    {
        this.StateName = "BattleState";
    }

    // �_ʼ
    public override void StateBegin()
    {
        LevelManager.Instance.Initialize();
        GameManager.Instance.Initinal();
        LevelManager.Instance.StartingGame = false;
        TipsManager.Instance.SetCanvasCam();
    }

    // �Y��
    public override void StateEnd()
    {
        DraggingShape.PickingShape = null;

        GameManager.Instance.Release();
        //RuleFactory.Release();

        EnemyBuffFactory.Release();
        TurretSkillFactory.Release();
        GuideGirlSystem.Instance.Release();
        GuideGirlSystem.Instance.Hide();
        TipsManager.Instance.HideTips();

        //GameEvents.Instance.Release();
    }

    // ����
    public override void StateUpdate()
    {
        GameManager.Instance.GameUpdate();
    }

}
