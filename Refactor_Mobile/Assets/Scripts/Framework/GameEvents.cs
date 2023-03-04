using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TutorialType
{
    None,
    DrawBtnClick,
    NextWaveBtnClick,
    TurretSelect,
    BlueprintSelect,
    MouseMove,
    WheelMove,
    NextWaveStart,
    ConfirmShape,
    ShopBtnClick,
    ElementBenefitEnter,
    RefactorBtnClick,
    SelectShape,
    SystemBtnClick,
    GuideBookContinue
}

public enum TempWordType
{
    StandardLose,
    StandardWin,
    EndlessEnd,
    RefreshShop,
    Refactor,
    Demo,
    DieProtect,
    WaveEnd

}

public struct TempWord
{
    public TempWordType WordType;
    public int ID;
    public TempWord(TempWordType type, int id)
    {
        WordType = type;
        ID = id;
    }
}

public struct GameEndStruct
{
    public bool Win;
    public GameEndStruct(bool win)
    {
        this.Win = win;
    }

}

public struct BillBoardInfo
{
    public TurretContent Turret;
    public int Rank;
    public BillBoardInfo(TurretContent content,int rank)
    {
        this.Turret = content;
        this.Rank = rank;
    }
}

public class GameEvents : MySingleton<GameEvents>
{

    public event Action onGuideObjCollect;
    public void GuideObjCollect()
    {
        onGuideObjCollect?.Invoke();
    }

    public event Action<TutorialType> onTutorialTrigger;
    public void TutorialTrigger(TutorialType tutorialType)
    {
        onTutorialTrigger?.Invoke(tutorialType);
    }

    public event Action<TempWord> onTempWord;
    public void TempWordTrigger(TempWord word)
    {
        onTempWord?.Invoke(word);
    }


    public event Action onSeekPath;

    public void SeekPath()
    {
        onSeekPath?.Invoke();
    }

    public event Action onTileClick;
    public void TileClick()
    {
        onTileClick?.Invoke();
    }

    public event Action<TileBase> onTileUp;
    public void TileUp(TileBase tile)
    {
        onTileUp?.Invoke(tile);
    }


    public event Action<Enemy> onEnemyReach;
    public void EnemyReach(Enemy enemy)
    {
        onEnemyReach?.Invoke(enemy);
    }

    public event Action<Enemy> onEnemyDie;
    public void EnemyDie(Enemy enemy)
    {
        onEnemyDie?.Invoke(enemy);
    }

    public event Action<bool> onShowDamageIntensify;
    public void ShowDamageIntensify(bool value)
    {
        onShowDamageIntensify?.Invoke(value);
    }

    public event Action onEndlessLeaderBoardGet;

    public void EndlessLeaderboardGet()
    {
        onEndlessLeaderBoardGet?.Invoke();
    }
    public event Action<bool> onChallengeLeaderBoardGet;

    public void ChallengeLeaderboardGet(bool value)
    {
        onChallengeLeaderBoardGet?.Invoke(value);
    }


    //成就系统

    public event Action<GameEndStruct> onGameEnd;
    public void GameEnd(GameEndStruct endData)
    {
        onGameEnd?.Invoke(endData);
    }

    public event Action<BillBoardInfo> onBillboardSort;
    public void BillBoardSort(BillBoardInfo billboardInfo)
    {
        onBillboardSort?.Invoke(billboardInfo);
    }
    public event Action onPrepareNextWave;
    public void PrepareNextWave()
    {
        onPrepareNextWave?.Invoke();
    }
    //public void Release()//清理战斗事件
    //{
    //    onESlotEquip = null;

    //}
}
