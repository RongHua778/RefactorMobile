using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameManager : Singleton<GameManager>
{
    [Header("系统")]
    [SerializeField] private BoardSystem m_BoardSystem = default;//版图系统
    [SerializeField] private WaveSystem m_WaveSystem = default;//波次系统
    [SerializeField] private TechnologySystem m_TechnologySystem = default;//科技系统
    [SerializeField] private ScaleAndMove m_CamControl = default;//摄像机控制系统
    [SerializeField] private ChallengeSystem m_ChallengeSystem = default;//挑战模式系统


    [Header("UI")]
    [SerializeField] private BluePrintShopUI m_BluePrintShopUI = default;
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;

    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private UISetting m_SettingPanel = default;
    [SerializeField] private TechSelectUI m_TechSelectUI = default;


    [Header("其他")]
    [SerializeField] private RangeContainer m_RangeContainer = default;
    [SerializeField] private HealthWarning m_HealthWarning = default;

    [Header("集合")]
    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection elementTurrets = new GameBehaviorCollection();
    public GameBehaviorCollection refactorTurrets = new GameBehaviorCollection();
    public GameBehaviorCollection Buildings = new GameBehaviorCollection();

    [Header("流程")]
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; set => operationState = value; }

    private BuildingState buildingState;
    private PickingState pickingState;
    private WaveState waveState;
    private EndState endState;
    private WonState wonState;

    private Dictionary<StateName, BattleOperationState> StateDIC = new Dictionary<StateName, BattleOperationState>();
    private InputManager inputManager;

    //初始化设定
    public void Initinal()
    {
        LevelManager.Instance.LevelEnd = false;//每次进入战斗场景就标识为未结束
        LevelManager.Instance.LevelWin = false;
        //设置参战配方
        //StaticData.Instance.ContentFactory.SetRareLists();//设置参战配方

        //形状生成外观类
        ConstructHelper.Initialize();
        //初始化科技系统
        TechnologyFactory.SetBattleTechs();
        //初始化全局数据
        GameRes.Initialize(m_MainUI, m_FuncUI, m_WaveSystem, m_BluePrintShopUI);


        m_MainUI.Initialize();//主界面顶部UI//要在wavesystem之前，因为敌人入侵事件，需要先掉血再判定下一波
        m_WaveSystem.Initialize();//波次系统
        m_CamControl.Initialize(m_MainUI);//摄像机控制
        m_BoardSystem.Initialize();//版图系统
        m_TechnologySystem.Initialize();//科技系统
        m_ChallengeSystem.Initialize();//挑战系统

        //初始化UI
        m_FuncUI.Initialize();//主界面功能UI
        m_BluePrintShopUI.Initialize();//配方系统UI
        m_ShapeSelectUI.Initialize();//抽模块UI
        m_GameEndUI.Initialize();//游戏结束UI
        m_TechSelectUI.Initialize();//科技选择界面


        m_SettingPanel.Initialize();
        m_RangeContainer.Initialize();
        m_HealthWarning.Initialize();


        inputManager = InputManager.Instance;
        //设置操作流程
        buildingState = new BuildingState(this, m_BoardSystem, m_FuncUI, m_ShapeSelectUI, m_TechSelectUI);
        waveState = new WaveState(this, m_WaveSystem, m_BoardSystem);
        pickingState = new PickingState(this, m_FuncUI);
        endState = new EndState(this);
        wonState = new WonState(this);

        StateDIC.Add(buildingState.StateName, buildingState);
        StateDIC.Add(waveState.StateName, waveState);
        StateDIC.Add(pickingState.StateName, pickingState);
        StateDIC.Add(endState.StateName, endState);
        StateDIC.Add(wonState.StateName, wonState);



        m_MainUI.Show();
        //关闭显示强制摆放位置
        m_BoardSystem.SetTutorialPoss(false);

        GuideGirlSystem.Instance.Initialize();

        if (LevelManager.Instance.LastGameSave.HasLastGame)
        {
            LoadGame();//只读取场景内已选元素，如防御塔，科技，配方等
        }
        else
        {
            StartNewGame();
        }

    }

    private void LoadGame()
    {
        GameRes.LoadSaveRes();
        StaticData.Instance.ContentFactory.LoadRareList();
        TechnologyFactory.SetRecipeTechs();//这个在设置了战斗配方battlerecipes后面，因为必须根据战斗配方设置可参战科技
        RuleFactory.LoadSaveRules();
        m_MainUI.SetRules();
        m_WaveSystem.LoadSaveWave();
        m_BoardSystem.LoadSaveGame();
        m_TechnologySystem.LoadSaveGame();
        m_BluePrintShopUI.LoadSaveGame();
        m_ShapeSelectUI.LoadSaveGame();
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge)
        {
            m_ChallengeSystem.LoadSaveGame();
            if (!LevelManager.Instance.LastGameSave.ChallengeChoicePicked)
                ShowChoices(true);
        }
        else
        {
            m_TechSelectUI.LoadSaveGame();
        }
        //继续保存的波
        ContinueWave();

    }

    private void StartNewGame()
    {
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge)//挑战模式
        {
            m_BluePrintShopUI.SetShopBtnActive(false);
            //if (m_ChallengeSystem.ChoiceRemained > 0)
            //    ShowChoices(true);
            //ShowChoices(true);
            if (LevelManager.Instance.CurrentLevel.SaveSequences.Count <= 0)
                m_WaveSystem.LevelInitialize();
            else
                m_WaveSystem.LoadChallengeWave();

        }
        else
        {
            m_BluePrintShopUI.SetShopBtnActive(true);

            StaticData.Instance.ContentFactory.SetRareLists();
            TechnologyFactory.SetRecipeTechs();
            RuleFactory.TriggerRules();
            RefreshShop(0);//必须Guidegirl前
            GuideGirlSystem.Instance.PrepareTutorial();
            m_WaveSystem.LevelInitialize();
        }

        m_BoardSystem.FirstGameSet();
        //开局准备下一波
        PrepareNextWave();
    }


    //释放游戏系统
    public void Release()
    {
        m_BoardSystem.Release();
        m_WaveSystem.Release();
        m_CamControl.Release();

        m_MainUI.Release();
        m_FuncUI.Release();

        m_BluePrintShopUI.Release();
        m_ShapeSelectUI.Release();
        m_GameEndUI.Release();

        m_TechSelectUI.Release();

        GameRes.GameSpeed = 1;

    }

    public void GameUpdate()
    {
        m_CamControl.GameUpdate();
        m_BoardSystem.GameUpdate();
        m_WaveSystem.GameUpdate();

        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        elementTurrets.GameUpdate();
        refactorTurrets.GameUpdate();
        Buildings.GameUpdate();
        nonEnemies.GameUpdate();
        OperationState.StateUpdate();

        KeyboardControl();
    }



    private void KeyboardControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (StaticData.LockKeyboard)//教学期间无法加速
        {
            return;
        }
        if (inputManager.GetKeyDown(KeyBindingActions.ChangeSpeed))
        {
            GameRes.GameSpeed++;
        }
        else if (inputManager.GetKeyDown(KeyBindingActions.Build))
        {
            m_FuncUI.DrawBtnClick();
        }
        else if (inputManager.GetKeyDown(KeyBindingActions.Refresh))
        {
            RefreshShop(GameRes.RefreshShopCost);
        }
        else if (inputManager.GetKeyDown(KeyBindingActions.OpenShop))
        {
            m_BluePrintShopUI.ShopBtnClick();
        }
        else if (inputManager.GetKeyDown(KeyBindingActions.NextWave))
        {
            m_FuncUI.NextWaveBtnClick();
        }
    }

    #region 阶段控制
    public void StartNewWave()
    {
        if (OperationState.StateName == StateName.BuildingState)
        {
            m_FuncUI.Hide();
            TransitionToState(StateName.WaveState);

            GameEvents.Instance.TutorialTrigger(TutorialType.NextWaveBtnClick);
            m_TechnologySystem.OnTurnEnd();
        }
        //参数设置

    }
    public void PrepareNextWave()
    {
        if (GameRes.Life <= 0)//游戏失败
        {
            TransitionToState(StateName.LoseState);
            return;
        }
        if (GameRes.CurrentWave >= LevelManager.Instance.CurrentLevel.Wave)
        {
            TransitionToState(StateName.WonState);
            GameEnd(true);
            return;
        }

        GameRes.PrepareNextWave();
        m_WaveSystem.PrepareNextWave();
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence, m_WaveSystem.NextBoss, m_WaveSystem.NextBossWave);
        GameEvents.Instance.TutorialTrigger(TutorialType.NextWaveStart);

        if (LevelManager.Instance.CurrentLevel.ModeType != ModeType.Challenge)
        {
            if (LevelManager.Instance.CurrentLevel.ModeID > 1)
            {
                if (GameRes.CurrentWave <= 99 && (GameRes.CurrentWave + 4) % 10 == 0)
                {
                    ShowTechSelect(true);
                }
            }
        }
        else
            ShowChoices(true);

        m_TechnologySystem.OnTurnStart();
        TransitionToState(StateName.BuildingState);

    }

    public void ContinueWave()
    {
        TransitionToState(StateName.BuildingState);
        m_WaveSystem.PrepareNextWave();
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence, m_WaveSystem.NextBoss, m_WaveSystem.NextBossWave);
    }

    public void GameEnd(bool win)
    {
        m_GameEndUI.Show();
        m_GameEndUI.SetGameResult(win);
    }

    public void TransitionToState(StateName stateName)
    {
        BattleOperationState state = StateDIC[stateName];
        if (OperationState == null)
        {
            OperationState = state;
            StartCoroutine(OperationState.EnterState());
        }
        else
        {
            StartCoroutine(OperationState.ExitState(state));
            //operationState = state;
        }
    }

    public void RestartGame()
    {
        if (Game.Instance.OnTransition)
            return;
        LevelManager.Instance.StartNewGame(LevelManager.Instance.CurrentLevel.ModeID);
    }

    public void ReturnToMenu()
    {
        if (Game.Instance.OnTransition)
            return;
        LevelManager.Instance.SaveAll();
        Game.Instance.LoadScene(0);

    }


    public void QuitGame()
    {
        if (Game.Instance.OnTransition)
            return;
        Game.Instance.QuitGame();
    }
    #endregion

    #region 形状控制
    public void DrawShapes()
    {
        GameEvents.Instance.TutorialTrigger(TutorialType.DrawBtnClick);
        //TransitionToState(StateName.PickingState);
        m_ShapeSelectUI.ShowThreeShapes();
        m_FuncUI.Hide();
    }

    public void SelectShape(TileShape shape, bool leveldown)//选择了一个模块
    {
        TileShape newShape = ConstructHelper.GetTutorialShape(shape.m_ShapeInfo, leveldown);
        newShape.SetPreviewPlace();
        m_ShapeSelectUI.Hide();
        m_BoardSystem.SetTutorialPoss(true);//显示强制摆放位置

        TransitionToState(StateName.PickingState);
    }



    public void ConfirmShape()//放下了一个模块
    {
        TransitionToState(StateName.BuildingState);
        m_BoardSystem.CheckPathTrap();
        CheckAllBlueprints();
        m_BoardSystem.SetTutorialPoss(false);//关闭显示强制摆放位置
        //m_ShapeSelectUI.ClearAllSelections();
        GameRes.ForcePlace = null;
        GameRes.PreSetShape = new ShapeInfo[3];
        //新手引导
        GameEvents.Instance.TutorialTrigger(TutorialType.ConfirmShape);
    }

    public void UndoShape()
    {
        m_ShapeSelectUI.Show();
    }

    public void CompositeShape(BluePrintGrid grid)//合成了一个防御塔
    {
        if (operationState.StateName == StateName.PickingState)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
            return;
        }
        if (operationState.StateName == StateName.WaveState)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        if (grid.Strategy.CheckBuildable())
        {
            TransitionToState(StateName.PickingState);
            m_BluePrintShopUI.RefactorBluePrint(grid);
            m_BoardSystem.SetTutorialPoss(true);//教学：显示强制摆放位置
            GameEvents.Instance.TutorialTrigger(TutorialType.RefactorBtnClick);
        }
        else
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMATERIAL"));
        }

    }

    public void RefactorLanded(RefactorTurret turret)
    {
        refactorTurrets.Add(turret);
        //m_BoardSystem.HighlightEmptySlotTurrets(false);
    }

    public void PreviewComposition(bool value, ElementType element = ElementType.DUST, int quality = 1)
    {
        m_BluePrintShopUI.PreviewComposition(value, element, quality);
    }
    #endregion

    #region 通用功能

    public void PauseGame()
    {
        if (!m_SettingPanel.IsVisible())
            m_SettingPanel.Show();
        else
            m_SettingPanel.ClosePanel();
    }
    public bool ConsumeMoney(int cost)
    {
        return m_MainUI.ConsumeMoney(cost);
    }


    public void GainMoney(int amount)
    {
        int final = Mathf.RoundToInt(amount * GameRes.CoinAdjust);
        GameRes.Coin += final;
        GameRes.GainGold += final;
    }

    public Enemy SpawnEnemy(EnemyType eType, int pathIndex, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        return m_WaveSystem.SpawnEnemy(eType, pathIndex, intensify, dmgResist, pathPoints);
    }

    public void RefreshShop(int cost)
    {
        m_BluePrintShopUI.RefreshShop(cost);
    }



    //public void ShowGuideVideo(int index)
    //{
    //    m_GuideVideo.Show();
    //    m_GuideVideo.ShowPage(index);
    //}


    public void BuyOneGround()
    {
        m_BoardSystem.BuyOneEmptyTile();
    }



    public void SwitchConcrete(GameTileContent content, int cost)
    {
        if (operationState.StateName == StateName.PickingState)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
            return;
        }
        if (operationState.StateName != StateName.BuildingState)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        if (ConsumeMoney(cost))
        {
            SwitchInfo swichInfo = new SwitchInfo();
            swichInfo.SwitchSpend = cost;
            swichInfo.InitPos = new Vector2Int(Mathf.RoundToInt(content.transform.position.x), Mathf.RoundToInt(content.transform.position.y));
            swichInfo.InitDir = (int)content.m_GameTile.TileDirection;
            GameRes.SwitchInfo = swichInfo;
            m_BoardSystem.SwitchContent(content);
            TransitionToState(StateName.PickingState);
        }
    }

    public void GainPerfectElement(int amount)
    {
        GameRes.PerfectElementCount += amount;
        CheckAllBlueprints();
    }



    #endregion

    #region TIPS

    //public void ShowBonusTips(GameLevelInfo info)
    //{
    //    m_BonusTips.Show();
    //    m_BonusTips.SetBouns(info);
    //}



    #endregion

    #region URL链接
    public void AddtoWishList()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
    }

    public void JoinDiscord()
    {
        Application.OpenURL("https://discord.gg/bPgMZ6kgBH");

    }

    public void JoinQQ()
    {
        Application.OpenURL("https://jq.qq.com/?_wv=1027&k=wuuN4Bll");
    }
    #endregion


    #region 镜头控制
    public void LocateCamPos(Vector2 pos)
    {
        m_CamControl.LocatePos(pos);
    }

    public void SetCamMovable(bool value)
    {
        m_CamControl.CanControl = value;
    }

    public void ShakeCam()
    {
        m_CamControl.ShakeCam();
    }
    #endregion

    #region 教程相关

    public void SetSizeTutorial(bool value)
    {
        m_CamControl.SizeTutorial = value;
    }

    public void SetMoveTutorial(bool value)
    {
        m_CamControl.MoveTurorial = value;
    }


    public void ManualSetSequence(EnemyType type, float stage, int wave)
    {
        m_WaveSystem.ManualSetSequence(type, stage, wave);
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence, m_WaveSystem.NextBoss, m_WaveSystem.NextBossWave);
    }


    #endregion

    #region 配方管理
    public void CheckAllBlueprints()
    {
        m_BluePrintShopUI.CheckAllBluePrint();
    }
    public void AddBluePrint(RefactorStrategy strategy)
    {
        m_BluePrintShopUI.AddBluePrint(strategy);
    }

    public void RemoveBluePrint(int id)
    {
        m_BluePrintShopUI.RemoveGrid(BluePrintShopUI.ShopBluePrints[id]);
    }

    public void RemoveUnlockedRecipes()
    {
        m_BluePrintShopUI.RemoveUnlockedRecipes();
    }
    #endregion

    #region 挑战模式
    public void ShowChoices(bool show, bool picking = false, bool newChoice = true)
    {
        if (show)
        {
            m_TechSelectUI.Show();
            if (newChoice)
            {
                ChallengeChoice choices = m_ChallengeSystem.GetCurrentChoice();
                if (choices != null)
                    m_TechSelectUI.GetCurrentChoices(choices);
                else
                    ConfirmChoice();
            }
        }
        else
        {
            m_TechSelectUI.Hide();
            if (picking)//是否有建筑要放置
                TransitionToState(StateName.PickingState);
            else
            {
                //m_FuncUI.Show();
                ConfirmChoice();
            }
        }
    }

    public void ConfirmChoice()
    {
        //if (m_ChallengeSystem.ChoiceRemained > 0)
        //    ShowChoices(true);
        //else
        GameRes.ChallengeChoicePicked = true;
        m_FuncUI.Show();
    }

    #endregion

    #region 科技相关
    public void ShowTechSelect(bool show, bool picking = false)
    {
        if (show)
        {
            m_TechSelectUI.Show();
            m_TechSelectUI.GetRandomTechs();
        }
        else
        {
            m_TechSelectUI.Hide();

            if (picking)//是否有建筑要放置
                TransitionToState(StateName.PickingState);
            else
            {
                m_FuncUI.Show();
                ConfirmTechSelect();
            }
        }
    }

    public void GetTech(Technology tech)
    {
        m_TechnologySystem.AddTech(tech);
    }

    public void RemoveTech(Technology tech)
    {
        m_TechnologySystem.RemoveTech(tech);
    }

    public void ConfirmTechSelect()
    {
        m_TechnologySystem.ConfirmTechSelect();
    }
    #endregion

    #region 技能触发
    public void TriggerDetectSkills()
    {
        foreach (var turret in elementTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.DetectSkills();
        }
        foreach (var turret in refactorTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.DetectSkills();
        }
    }

    #endregion


    #region 待加入功能
    public void DieProtect()
    {
        GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.DieProtect, 0));
        Sound.Instance.PlayUISound("Sound_Warning");

        foreach (var enemy in enemies.behaviors.ToList())
        {
            ((Enemy)enemy).DamageStrategy.IsDie = true;
        }
        m_HealthWarning.Show();
    }


    public List<ShapeInfo> GetCurrentPickingShapes()
    {
        return m_ShapeSelectUI.GetCurrent3ShapeInfos();
    }

    public void ShowConcreteRange(ConcreteContent concrete, bool value)
    {
        m_RangeContainer.SetRange(concrete, value);
    }


    #endregion
}
