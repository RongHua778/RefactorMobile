using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForcePlace
{
    public Vector2 ForcePos;
    public Vector2 ForceDir;
    public List<Vector2> GuidePos = new List<Vector2>();
    public ForcePlace(Vector2 forcePos, Vector2 forceDir, List<Vector2> guidePoss)
    {
        this.ForcePos = forcePos;
        this.ForceDir = forceDir;
        this.GuidePos = guidePoss;
    }
}
[System.Serializable]
public class ShapeInfo
{
    public int ShapeType;
    public int Element;
    public int Quality;
    public int TurretPos;
    public int TurretDir;


}

[System.Serializable]
public class SwitchInfo
{
    public int SwitchSpend;
    public Vector2Int InitPos;
    public int InitDir;
}
public static class GameRes
{
    public static GameResStruct SaveRes => SetSaveRes();

    private static MainUI m_MainUI;
    private static FuncUI m_FuncUI;
    private static BluePrintShopUI m_BluePrintShop;
    private static WaveSystem m_WaveSystem;

    [Header("动态数据")]
    public static int FreeGroundTileCount = 0;//免费地板数量
    public static Action<StrategyBase> NextCompositeCallback = null;
    public static int DieProtect;

    [Header("统计数据")]
    public static DateTime LevelStart;
    public static DateTime LevelEnd;
    public static int TotalRefactor = 0;
    //public static int TotalCooporative = 0;
    public static long TotalDamage = 0;
    private static int maxPath = 0;
    public static int MaxPath { get => maxPath; set { maxPath = value > maxPath ? value : maxPath; } }
    public static int MaxMark = 0;
    public static int GainGold = 0;

    public static int SkipTimes = 0;

    [Header("全局动态数据")]
    public static float GameGoldIntensify = 0;
    public static float GameWoodIntensify = 0;
    public static float GameWaterIntensify = 0;
    public static float GameFireIntensify = 0;
    public static float GameDustIntensify = 0;
    public static float EnemyFrostTime;
    //public static int GlobalExtraSlot = 0;//全局额外技能槽名额
    //public static int GlobalExtraSlotValue = 0;//全局额外技能槽数量
    //public static int ConstructorExtraSlot = 0;//加农炮技能槽名额
    //public static int CoreExtraSlot = 0;//核心机技能槽名额
    public static float TurretUpgradeDiscount = 0;
    public static int SkillChipInterval = 25;



    private static float turnSpeedAdjust = 1f;
    public static float TurnSpeedAdjust { get => turnSpeedAdjust + 0.3f * (CurrentWave / 30); set => turnSpeedAdjust = value; }
    public static float EnemyFrostResist = 0;//敌人冻结抵挡
    public static float TurretFrostResist = 0;//防御塔冻结抵抗
    public static float EnemyAmoundAdjust;//敌人数量修正
    public static float EnemyIntensifyAdjust;//敌人血量修正


    public static int TotalLandedRefactor = 0;
    public static int GainGoldBattleTurn = 0;//回合中获取的金币
    public static int GainPerfectBattleTurn = 0;//回合中获取的完美元素
    public static int LostLifeBattleTurn = 0;//回合中失去的生命值

    //public static int GlobalExtraSlot = 0;
    //public static int 

    [Header("场地数据")]
    public static int GroundSize = 21;
    public static int TrapDistanceAdjust = 0;


    private static int gameSpeed = 1;
    public static int GameSpeed
    {
        get => gameSpeed;
        set
        {
            if (value > 3)
            {
                gameSpeed = 1;
            }
            else
            {
                gameSpeed = value;
            }
            Time.timeScale = gameSpeed;
            m_MainUI.GameSpeed = gameSpeed;
        }
    }




    //新手引导
    static ShapeInfo[] preSetShape;//预设形状
    public static ShapeInfo[] PreSetShape { get => preSetShape; set => preSetShape = value; }

    static ForcePlace forcePlace;//强制摆位
    public static ForcePlace ForcePlace { get => forcePlace; set => forcePlace = value; }

    private static int perfectElementCount;
    public static int PerfectElementCount
    {
        get => perfectElementCount;
        set
        {
            perfectElementCount = value;
            m_BluePrintShop.PerfectElementCount = perfectElementCount;
        }
    }
    public static int AutoRefreshInterval;

    private static int nextRefreshTurn;
    public static int NextRefreshTurn
    {
        get => nextRefreshTurn;
        set
        {
            if (value <= 0)
            {
                nextRefreshTurn = AutoRefreshInterval;
                GameManager.Instance.RefreshShop(0);
            }
            else
            {
                nextRefreshTurn = value;
            }
            m_BluePrintShop.NextRefreshTrun = nextRefreshTurn;
        }
    }

    private static bool drawThisTurn;
    public static bool DrawThisTurn
    {
        get => drawThisTurn;
        set
        {
            drawThisTurn = value;
        }
    }

    private static int coin;
    public static int Coin//拥有代币
    {
        get => coin;
        set
        {
            coin = Mathf.Max(0, value);
            m_MainUI.Coin = coin;
        }
    }
    private static bool WontDieThisTurn;
    private static int life;
    public static int Life//当亲生命值
    {
        get => life;
        set
        {
            if (value <= 0)
            {
                if (life <= 0 || WontDieThisTurn)
                    return;
                if (DieProtect > 0)
                {
                    DieProtect--;
                    WontDieThisTurn = true;
                    life = 1;
                    m_MainUI.Life = life;
                    EnemyRemain++;
                    GameManager.Instance.DieProtect();
                    return;
                }
                GameManager.Instance.GameEnd(false);
            }
            life = Mathf.Clamp(value, 0, LevelManager.Instance.CurrentLevel.PlayerHealth);
            m_MainUI.Life = life;
        }
    }
    private static int enemyRemain;
    public static int EnemyRemain
    {
        get => enemyRemain;
        set
        {
            enemyRemain = value;
            if (enemyRemain <= 0 && !m_WaveSystem.RunningSpawn)
            {
                enemyRemain = 0;
                GameManager.Instance.PrepareNextWave();
            }
        }
    }

    public static int currentWave;
    public static int CurrentWave//当前波数
    {
        get => currentWave;
        set
        {
            currentWave = value;
            m_MainUI.CurrentWave = currentWave;
        }
    }

    private static int buildCost;
    public static int BuildCost//构建价格
    {
        get => buildCost;
        set
        {
            buildCost = Mathf.Max(0, value);
            m_FuncUI.BuyShapeCost = buildCost;
        }
    }

    private static int refreshShopCost;
    public static int RefreshShopCost
    {
        get => refreshShopCost;
        set
        {
            refreshShopCost = value;
            m_BluePrintShop.RefreshShopCost = refreshShopCost;
        }
    }

    private static int switchTrapCost;
    public static int SwitchTrapCost
    {
        get => LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge ? 30 : Mathf.Min(120, switchTrapCost);
        set
        {
            switchTrapCost = value;
        }
    }

    private static int switchTurretCost;
    public static int SwitchTurretCost
    {
        get => switchTurretCost;
        set => switchTurretCost = value;
    }

    private static int systemLevel = 1;
    public static int SystemLevel   //模块等级
    {
        get => systemLevel;
        set
        {
            systemLevel = Mathf.Clamp(value, 1, 6);
            SystemUpgradeCost = StaticData.Instance.LevelUpMoney[systemLevel];
            m_FuncUI.SystemLevel = systemLevel;
        }
    }

    private static int systemUpgradeCost;
    public static int SystemUpgradeCost
    {
        get => systemUpgradeCost;
        set
        {
            systemUpgradeCost = value;
            m_FuncUI.SystemUpgradeCost = systemUpgradeCost;
        }

    }

    private static float discountRate;
    public static float BuildDiscount
    {
        get => discountRate;
        set
        {
            discountRate = Mathf.Min(0.5f, value);
            m_FuncUI.DiscountRate = discountRate;
        }
    }

    private static int shopCapacity;
    public static int ShopCapacity { get => Mathf.Min(6, shopCapacity); set => shopCapacity = value; }//商店容量


    private static int lockCount = 1;
    public static int LockCount
    {
        get => Mathf.Max(0, lockCount);
        set
        {
            lockCount = value;
            m_BluePrintShop.ShowAllLock(lockCount);
        }

    }//最大锁定量
    private static int buyGroundCost;
    public static int BuyGroundCost
    {
        get => LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge ? 50 : Mathf.Min(100, buyGroundCost);
        set
        {
            buyGroundCost = value;
        }
    }

    private static float abnormalRate;
    public static float AbnormalRate
    {
        get => abnormalRate;
        set
        {
            abnormalRate = Mathf.Clamp(value, 0, 1);
        }
    }

    private static int skillChip;
    public static int SkillChip//元素技能槽芯片
    {
        get => skillChip;
        set
        {
            skillChip = value;
        }
    }


    public static int IntentLineID; //光棱区生成类型0=不生成，1=加强，2=削弱
    public static int RefreashTechCost { get; set; }
    public static int FreeRefreshTech { get; set; }
    public static bool Reverse { get; set; }//是否循环

    public static SwitchInfo SwitchInfo;
    public static float DamageAdjust;
    public static float CoinAdjust;

    public static bool ChallengeChoicePicked;

    public static void Initialize(MainUI mainUI, FuncUI funcUI, WaveSystem waveSystem, BluePrintShopUI bluePrintShop)
    {
        m_MainUI = mainUI;
        m_FuncUI = funcUI;
        m_WaveSystem = waveSystem;
        m_BluePrintShop = bluePrintShop;

        Reverse = false;

        GroundSize = LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge ? 15 : 21;
        TrapDistanceAdjust = 0;
        EnemyFrostResist = 0;
        TurretFrostResist = 0f;
        EnemyAmoundAdjust = 1f;
        EnemyIntensifyAdjust = 1f;
        TurnSpeedAdjust = 1f;
        DamageAdjust = 1f;
        CoinAdjust = 1f;

        DieProtect = 1;
        DrawThisTurn = true;
        TotalRefactor = 0;
        TotalDamage = 0;
        NextRefreshTurn = 4;
        BuildDiscount = 0.1f;
        ShopCapacity = 3;
        SystemLevel = 1;
        CurrentWave = 0;
        SwitchTrapCost = StaticData.Instance.SwitchTrapCost;
        SwitchTurretCost = StaticData.Instance.SwitchTurretCost;

        Coin = LevelManager.Instance.CurrentLevel.StartCoin;
        Life = LevelManager.Instance.CurrentLevel.PlayerHealth;
        BuildCost = StaticData.Instance.BaseShapeCost;
        RefreshShopCost = StaticData.Instance.ShopRefreshCost;
        BuyGroundCost = StaticData.Instance.BuyGroundCost;

        AutoRefreshInterval = 3;
        PerfectElementCount = 0;

        LevelStart = DateTime.Now;
        MaxPath = 0;
        MaxMark = 0;
        GainGold = 0;
        enemyRemain = 0;

        SkipTimes = 0;


        LockCount = 1;

        FreeGroundTileCount = 0;

        GameGoldIntensify = StaticData.Instance.GoldAttackIntensify;
        GameWoodIntensify = StaticData.Instance.WoodFirerateIntensify;
        GameWaterIntensify = StaticData.Instance.WaterSlowIntensify;
        GameFireIntensify = StaticData.Instance.FireCritIntensify;
        GameDustIntensify = StaticData.Instance.DustSplashIntensify;

        EnemyFrostTime = 3f;
        TotalLandedRefactor = 0;
        AbnormalRate = 0.4f;
        RefreashTechCost = 100;
        FreeRefreshTech = 1;

        GainGoldBattleTurn = 0;
        GainPerfectBattleTurn = 0;
        LostLifeBattleTurn = 0;

        PreSetShape = new ShapeInfo[3];
        ForcePlace = null;

        SkillChip = 0;
        SkillChipInterval = 25;

        IntentLineID = 0;

        ChallengeChoicePicked = false;

        //敌人数据重置
        Hamster.isFirstHamster = false;
        StrategyBase.CoordinatorMaxIntensify = 1.2f;

    }


    private static GameResStruct SetSaveRes()
    {
        GameResStruct resStruct = new GameResStruct();
        resStruct.Mode = LevelManager.Instance.CurrentLevel.ModeID;
        resStruct.Coin = Coin - GainGoldBattleTurn;
        resStruct.Wave = CurrentWave;
        resStruct.CurrentLife = Life + LostLifeBattleTurn;
        resStruct.MaxLife = LevelManager.Instance.CurrentLevel.PlayerHealth;
        resStruct.BuildCost = BuildCost;
        resStruct.SwitchTrapCost = SwitchTrapCost;
        resStruct.SystemLevel = SystemLevel;
        resStruct.SystemUpgradeCost = systemUpgradeCost;
        resStruct.TotalDamage = TotalDamage;
        resStruct.TotalRefactor = TotalRefactor;
        resStruct.ShopCapacity = ShopCapacity;
        resStruct.NextRefreshTurn = NextRefreshTurn;
        resStruct.PefectElementCount = PerfectElementCount - GainPerfectBattleTurn;
        resStruct.DrawThisTurn = DrawThisTurn;
        resStruct.DieProtect = DieProtect;
        resStruct.BuyGroundCost = BuyGroundCost;
        resStruct.GainGold = GainGold;
        resStruct.StartTime = LevelStart;
        resStruct.RefreashTechCost = RefreashTechCost;
        resStruct.SkillChip = SkillChip;
        resStruct.FreeRefreshTech = FreeRefreshTech;

        resStruct.SkipTimes = SkipTimes;
        return resStruct;

    }

    public static void LoadSaveRes()
    {
        GameResStruct saveRes = LevelManager.Instance.LastGameSave.SaveRes;
        Coin = saveRes.Coin;
        CurrentWave = saveRes.Wave;//prepareNextWave导致+1
        Life = saveRes.CurrentLife;
        BuildCost = saveRes.BuildCost;
        SwitchTrapCost = saveRes.SwitchTrapCost;
        SystemLevel = saveRes.SystemLevel;
        SystemUpgradeCost = saveRes.SystemUpgradeCost;
        TotalRefactor = saveRes.TotalRefactor;
        ShopCapacity = saveRes.ShopCapacity;
        NextRefreshTurn = saveRes.NextRefreshTurn;
        PerfectElementCount = saveRes.PefectElementCount;
        DrawThisTurn = saveRes.DrawThisTurn;
        DieProtect = saveRes.DieProtect;
        BuyGroundCost = saveRes.BuyGroundCost;

        GainGold = saveRes.GainGold;
        TotalDamage = saveRes.TotalDamage;
        LevelStart = saveRes.StartTime;
        RefreashTechCost = saveRes.RefreashTechCost;
        FreeRefreshTech = saveRes.FreeRefreshTech;
        SkillChip = saveRes.SkillChip;

        SkipTimes = saveRes.SkipTimes;
    }

    public static bool CheckForcePlacement(Vector2 pos, Vector2 dir)
    {
        if (ForcePlace == null)
            return true;
        if (Vector2.SqrMagnitude(pos - ForcePlace.ForcePos) < 0.1f
            && (ForcePlace.ForceDir == Vector2.zero || Vector2.Dot(dir, ForcePlace.ForceDir) > 0.99f))
            return true;
        else
            return false;
    }

    public static void PrepareNextWave()
    {
        CurrentWave++;
        GainGoldBattleTurn = 0;
        GainPerfectBattleTurn = 0;
        LostLifeBattleTurn = 0;
        if (CurrentWave % SkillChipInterval == 0)
        {
            GainSkillChip(1);
        }
        if (LevelManager.Instance.CurrentLevel.ModeType != ModeType.Challenge)
        {
            NextRefreshTurn--;
            DrawThisTurn = false;
        }
        WontDieThisTurn = false;
        //获得回合金币
        GameManager.Instance.GainMoney(Mathf.Min(300, (StaticData.Instance.BaseWaveIncome +
        StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1))));
        BuildCost = Mathf.RoundToInt(BuildCost * (1 - BuildDiscount));
    }

    public static void GainSkillChip(int amount)
    {
        SkillChip += amount;
        TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("GAINCHIP"));
    }

}
