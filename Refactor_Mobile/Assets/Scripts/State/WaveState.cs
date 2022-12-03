using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState : BattleOperationState
{
    WaveSystem m_WaveSystem;
    BoardSystem m_BoardSystem;
    public WaveState(GameManager gameManager, WaveSystem waveSystem, BoardSystem boardSystem) : base(gameManager)
    {
        m_BoardSystem = boardSystem;
        m_WaveSystem = waveSystem;
    }

    public override StateName StateName => StateName.WaveState;

    public override IEnumerator EnterState()
    {
        gameManager.OperationState = this;
        //计算陷阱列表及效果
        StrategyBase.CooporativeAttackIntensify = 0;
        m_BoardSystem.GetPathTiles();
        switch (m_WaveSystem.RunningSequence[0].EnemyType)
        {
            case EnemyType.Soilder:
            case EnemyType.Runner:
            case EnemyType.Restorer:
            case EnemyType.Tanker:
            case EnemyType.Fission:
            case EnemyType.Froster:
            case EnemyType.Transfer:
            case EnemyType.Leader:
            case EnemyType.GoldKeeper:
                Sound.Instance.PlayBg("Music_Normal");
                break;
            default:
                Sound.Instance.PlayBg("Music_Boss");
                break;
        }
        //m_BoardSystem.TransparentPath(new Color(0.2f, 1f, 1f, 0f), 0.3f);
        //yield return new WaitForSeconds(0.3f);
        m_BoardSystem.TransparentPath(new Color(1f, 0.4f, 0.2f, 0.35f), 0.5f);

        foreach (var turret in GameManager.Instance.refactorTurrets.behaviors)
        {
            ((ConcreteContent)turret).Strategy.ClearTurnAnalysis();
            ((ConcreteContent)turret).Strategy.StartTurnSkills();
            ((ConcreteContent)turret).Strategy.StartTurnSkill2();
            ((ConcreteContent)turret).Strategy.StartTurnSkill3();

        }
        foreach (var turret in GameManager.Instance.elementTurrets.behaviors)
        {
            ((ConcreteContent)turret).Strategy.ClearTurnAnalysis();
            //((ConcreteContent)turret).Strategy.StartTurnSkills();
            //((ConcreteContent)turret).Strategy.StartTurnSkill2();
        }
        foreach (var building in GameManager.Instance.Buildings.behaviors)
        {
            ((ConcreteContent)building).Strategy.StartTurnSkills();
            ((ConcreteContent)building).Strategy.StartTurnSkill2();
            ((ConcreteContent)building).Strategy.StartTurnSkill3();

        }
        GameRes.MaxPath = BoardSystem.shortestPath.Count;
        yield return new WaitForSeconds(0.5f);
        m_WaveSystem.RunningSpawn = true;

        yield break;
    }



    public override IEnumerator ExitState(BattleOperationState newState)
    {
        yield return new WaitForSeconds(0.2f);

        GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.WaveEnd, GameRes.CurrentWave));

        StrategyBase.CooporativeAttackIntensify = 0;
        //重置所有防御塔的回合临时加成
        GameManager.Instance.nonEnemies.RemoveAll();//清除所有子弹，放上面，Ultralava需要Debuff其他塔
        foreach (var turret in GameManager.Instance.elementTurrets.behaviors)
        {
            ((ConcreteContent)turret).Strategy.ClearTurnIntensify();
            ((ConcreteContent)turret).TurnClear();
        }
        foreach (var turret in GameManager.Instance.refactorTurrets.behaviors)
        {
            ((ConcreteContent)turret).Strategy.ClearTurnIntensify();
            ((ConcreteContent)turret).TurnClear();
        }
        foreach (var building in GameManager.Instance.Buildings.behaviors)
        {
            ((ConcreteContent)building).Strategy.ClearTurnIntensify();
            ((ConcreteContent)building).TurnClear();
        }

        yield return new WaitForSeconds(0.1f);
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
