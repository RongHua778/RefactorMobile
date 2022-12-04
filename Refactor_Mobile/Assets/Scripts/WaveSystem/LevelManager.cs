using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using LitJson;
//using Steamworks;
using System.Security.Cryptography;
using System.Text;
using System;


[System.Serializable]
public struct GameLevelInfo
{
    public int ExpRequire;
    public ContentAttribute[] UnlockItems;
}

public class LevelManager : Singleton<LevelManager>
{
    #region 成就
    public bool LevelWin { get; set; }

    //public void ClearAllSteamStats()
    //{
    //    SteamUserStats.ResetAllStats(true);
    //}
    //private void SetAchievements()
    //{
    //    if (!SteamManager.Initialized)
    //        return;
    //    foreach (var ach in achievements)
    //    {
    //        SteamUserStats.GetAchievement(ach, out bool achieve);
    //        if (!achieve && PlayerPrefs.GetInt(ach, 0) == 1)
    //        {
    //            SteamUserStats.SetAchievement(ach);
    //            SteamUserStats.StoreStats();
    //        }
    //    }
    //}
    public void SetAchievement(string achievement)
    {
        PlayerPrefs.SetInt(achievement, 1);
        //if (SteamManager.Initialized)
        //{
        //    SteamUserStats.GetAchievement(achievement, out bool achieve);
        //    if (!achieve)
        //    {
        //        SteamUserStats.SetAchievement(achievement);
        //        SteamUserStats.StoreStats();
        //    }
        //}
    }


    #endregion

    #region 基础保存

    private string key = "123456789";

    [SerializeField] private GameLevelInfo[] GameLevels = default;
    [Header("允许最大等级")]
    [SerializeField] int permitGameLevel = default;
    public int PermitDifficulty = default;
    public int MaxGameLevel => Mathf.Min(GameLevels.Length - 1, permitGameLevel);

    public int GameLevel
    {
        get => PlayerPrefs.GetInt("GameLevel", 0);
        set
        {
            PlayerPrefs.SetInt("GameLevel", Mathf.Min(value, MaxGameLevel));
            //if (SteamManager.Initialized)
            //{
            //    SteamUserStats.RequestCurrentStats();
            //    int steamLevel;
            //    if (SteamUserStats.GetStat("Player_Level", out steamLevel))
            //    {
            //        if (value > steamLevel)
            //            SteamUserStats.SetStat("Player_Level", value);
            //    }
            //}
        }
    }

    public int GameExp
    {
        get => PlayerPrefs.GetInt("GameExp", 0);
        set
        {
            PlayerPrefs.SetInt("GameExp", value);
            //if (SteamManager.Initialized)
            //{
            //    SteamUserStats.RequestCurrentStats();
            //    int steamExp;
            //    if (SteamUserStats.GetStat("Player_EXP", out steamExp))
            //    {
            //        if (value > steamExp)
            //            SteamUserStats.SetStat("Player_EXP", value);
            //    }
            //}
        }
    }
    public List<ContentAttribute> AllContent;

    public LevelAttribute[] StandardLevels = default;

    public LevelAttribute[] ChallengeLevels = default;

    private Dictionary<int, LevelAttribute> LevelDIC;

    public int PassDiifcutly
    {
        get => Mathf.Min(PermitDifficulty, PlayerPrefs.GetInt("MaxDiff", 0));
        set
        {
            PlayerPrefs.SetInt("MaxDiff", Mathf.Min(9, value));
            //if (SteamManager.Initialized)
            //{
            //    SteamUserStats.RequestCurrentStats();
            //    int steamDiff;
            //    if (SteamUserStats.GetStat("Player_Diff", out steamDiff))
            //    {
            //        if (value > steamDiff)
            //            SteamUserStats.SetStat("Player_Diff", Mathf.Min(7, value));
            //    }
            //}
        }
    }
    public int PassChallengeLevel
    {
        get => PlayerPrefs.GetInt("PassChallengeLevel", 0);
        set
        {
            PlayerPrefs.SetInt("PassChallengeLevel", Mathf.Min(ChallengeLevels.Length, value));
            //if (SteamManager.Initialized)
            //{
            //    SteamUserStats.RequestCurrentStats();
            //    int steamChallengeLevel;
            //    if (SteamUserStats.GetStat("PassChallengeLevel", out steamChallengeLevel))
            //    {
            //        if (value > steamChallengeLevel)
            //            SteamUserStats.SetStat("PassChallengeLevel", Mathf.Min(ChallengeLevels.Length, value));
            //    }
            //}
        }
    }

    // 关卡最高记录
    public int GetChallengeScore(int level)
    {
        return PlayerPrefs.GetInt("ChallengeScore" + level, 0);
    }

    public void SetChallengeScore(int level, int value)
    {
        PlayerPrefs.SetInt("ChallengeScore" + level, value);
        //if (SteamManager.Initialized)
        //{
        //    SteamUserStats.RequestCurrentStats();
        //    int steamChallengeLevel;
        //    if (SteamUserStats.GetStat("ChallengeScore" + level, out steamChallengeLevel))
        //    {
        //        if (value > steamChallengeLevel)
        //            SteamUserStats.SetStat("ChallengeScore" + level, value);
        //    }
        //}
    }

    public int LifeTotalRefactor//历史总重构次数，成就
    {
        get => PlayerPrefs.GetInt("LifeTotalRefactor", 0);
        set
        {
            //if (value > PlayerPrefs.GetInt("LifeTotalRefactor", 0))
            //{
            //    PlayerPrefs.SetInt("LifeTotalRefactor", value);
            //    if (SteamManager.Initialized)
            //    {
            //        SteamUserStats.SetStat("ACH_TotalRefactor", value);
            //    }
            //}
        }

    }
    public LevelAttribute CurrentLevel;
    #endregion

    private string SaveGameFilePath;


    //防连续开始游戏
    [HideInInspector]
    public bool StartingGame = false;


    #region 临时游戏保存
    [HideInInspector] public List<GameTileContent> GameSaveContents;
    [Header("是否读取存档")]
    [SerializeField] bool NeedLoadGame = default;
    [Header("是否有未完成游戏")]
    public GameSave LastGameSave;
    public bool LevelEnd = true;//游戏是否结束
    #endregion

    public void Initialize()
    {
        LitJsonRegister.Register();
        SaveGameFilePath = Application.persistentDataPath + "/GameSave.json";

        GetSteamStat();
        //SetAchievements();
        LevelDIC = new Dictionary<int, LevelAttribute>();

        foreach (var level in StandardLevels)
        {
            LevelDIC.Add(level.ModeID, level);
        }
        foreach (var level in ChallengeLevels)
        {
            LevelDIC.Add(level.ModeID, level);
        }

        //mobileTest
        PassDiifcutly = 9;
    }

    private void GetSteamStat()
    {
        //int steamLevel;
        //int steamExp;
        //int difficulty;
        //int challengeLevel;
        //int challengeScore;
        //if (SteamManager.Initialized)
        //{
        //    SteamUserStats.RequestCurrentStats();
        //    if (SteamUserStats.GetStat("Player_Level", out steamLevel))
        //        GameLevel = steamLevel;
        //    if (SteamUserStats.GetStat("Player_EXP", out steamExp))
        //        GameExp = steamExp;
        //    if (SteamUserStats.GetStat("Player_Diff", out difficulty))
        //        PassDiifcutly = difficulty;
        //    if (SteamUserStats.GetStat("PassChallengeLevel", out challengeLevel))
        //        PassChallengeLevel = challengeLevel;
        //    //for (int i = 0; i < ChallengeLevels.Length; i++)
        //    //{
        //    //    if (SteamUserStats.GetStat("ChallengeScore" + i, out challengeScore))
        //    //        SetChallengeScore(i, challengeScore);
        //    //}
        //}

    }



    private void DeleteGameSave()//删除对局存档文件
    {
        if (File.Exists(SaveGameFilePath))
            File.Delete(SaveGameFilePath);
    }

    public LevelAttribute GetLevelAtt(int mode)
    {
        if (LevelDIC.ContainsKey(mode))
        {
            return LevelDIC[mode];
        }
        else
        {
            Debug.LogWarning("错误的模式代码");
            return null;
        }
    }


    public void SetGameLevel(int level)
    {
        GameLevel = level;

        foreach (var item in AllContent)
        {
            item.isLock = item.initialLock;
        }
        for (int i = 0; i < GameLevel + 1; i++)//解锁低于当前等级的奖励
        {
            foreach (var item in GameLevels[i].UnlockItems)
            {
                item.isLock = false;
            }
        }


    }

    public int GainExp(int wave)
    {
        int exp = Mathf.RoundToInt(CurrentLevel.ExpIntensify * 5 * wave * (1 + wave / 10 * 0.25f));
        AddExp(exp);
        return exp;
    }

    private void UnlockBonus(string bo)
    {
        foreach (var item in AllContent)
        {
            if (item.Name == bo)
            {
                item.isLock = false;
                break;
            }
        }

    }

    private void AddExp(int exp)
    {
        if (GameLevel >= MaxGameLevel)//达到最大等级后，只加经验不升级
        {
            GameExp += exp;
            return;
        }
        int need = GameLevels[GameLevel].ExpRequire - GameExp;
        if (exp >= need)
        {
            GameLevel++;
            foreach (var item in GameLevels[GameLevel].UnlockItems)
            {
                UnlockBonus(item.Name);
            }
            GameExp = 0;
            AddExp(exp - need);
        }
        else
        {
            GameExp += exp;
        }
    }

    public GameLevelInfo GetLevelInfo(int level)
    {
        return GameLevels[level];
    }

    //存档管理

    public void StartNewGame(int modeID)
    {
        if (StartingGame)
            return;
        StartingGame = true;//防止连续点击

        CurrentLevel = GetLevelAtt(modeID);
        if (CurrentLevel == null)
        {
            Debug.LogWarning("没有找到对应的关卡文件");
            return;
        }

        LastGameSave.ClearGame();//清除保存战斗
        GameSaveContents.Clear();//清除保存数据
        DeleteGameSave();//删除存档文件
        Game.Instance.LoadScene(1);
    }


    public void ContinueLastGame()
    {
        if (StartingGame)
            return;
        StartingGame = true;

        //缺少一个LoadRateList

        DeleteGameSave();
        Game.Instance.LoadScene(1);
    }

    public List<ContentStruct> SaveContens()
    {
        List<ContentStruct> SaveContents = new List<ContentStruct>();
        ContentStruct contentStruct;
        foreach (var content in GameSaveContents)
        {
            content.SaveContent(out contentStruct);
            SaveContents.Add(contentStruct);
        }
        return SaveContents;
    }

    private List<EnemySequenceStruct> SaveSequences()
    {
        List<EnemySequenceStruct> EStructs = new List<EnemySequenceStruct>();
        foreach (var sequences in WaveSystem.LevelSequence)
        {
            EnemySequenceStruct eStruct = new EnemySequenceStruct();
            eStruct.SequencesList = sequences;
            EStructs.Add(eStruct);
        }
        return EStructs;
    }

    private List<BlueprintStruct> SaveAllBlueprints()
    {
        List<BlueprintStruct> bluePrintStructList = new List<BlueprintStruct>();
        foreach (var grid in BluePrintShopUI.ShopBluePrints)
        {

            BlueprintStruct blueprintStruct = new BlueprintStruct();
            blueprintStruct.Name = grid.Strategy.Attribute.Name;
            blueprintStruct.ElementRequirements = new List<int>();
            blueprintStruct.QualityRequirements = new List<int>();
            for (int i = 0; i < grid.Strategy.Compositions.Count; i++)
            {
                blueprintStruct.ElementRequirements.Add(grid.Strategy.Compositions[i].elementRequirement);
                blueprintStruct.QualityRequirements.Add(grid.Strategy.Compositions[i].qualityRequeirement);

            }

            bluePrintStructList.Add(blueprintStruct);
        }
        return bluePrintStructList;
    }

    private List<TechnologyStruct> SaveAllTechs()
    {
        List<TechnologyStruct> techList = new List<TechnologyStruct>();
        foreach (var tech in TechnologySystem.GetTechnologies)
        {
            TechnologyStruct techStruct = new TechnologyStruct();
            techStruct.TechName = (int)tech.TechnologyName;
            techStruct.IsAbnormal = tech.IsAbnormal;
            techStruct.TechSaveValue = tech.SaveValue;
            techStruct.CanAbnormal = tech.CanAbnormal;
            techList.Add(techStruct);
        }
        return techList;
    }

    private List<int> SaveRules()
    {
        List<int> saveRules = new List<int>();
        foreach (var rule in RuleFactory.BattleRules)
        {
            saveRules.Add((int)rule.RuleName);
        }
        return saveRules;
    }

    private List<TechnologyStruct> SavePickingTechs()
    {
        List<TechnologyStruct> techList = new List<TechnologyStruct>();
        foreach (var tech in TechnologySystem.PickingTechs)
        {
            TechnologyStruct techStruct = new TechnologyStruct();
            techStruct.TechName = (int)tech.TechnologyName;
            techStruct.IsAbnormal = tech.IsAbnormal;
            techStruct.TechSaveValue = tech.SaveValue;
            techStruct.CanAbnormal = tech.CanAbnormal;
            techList.Add(techStruct);
        }
        return techList;
    }


    private List<ShapeInfo> SaveCurrentShapes()
    {
        return GameManager.Instance.GetCurrentPickingShapes();
    }

    private List<string> SaveBattleRecipes()
    {
        List<string> saveList = new List<string>();
        foreach (var item in StaticData.Instance.ContentFactory.BattleRecipes)
        {
            saveList.Add(item.Name);
        }
        return saveList;
    }

    //private ContentStruct SaveDraggingContent()
    //{
    //    ContentStruct contentSctruct = null;
    //    if (DraggingShape.PickingShape != null && DraggingShape.PickingShape.TileShape.shapeType == ShapeType.D)
    //    {
    //        DraggingShape.PickingShape.TileShape.tiles[0].Content.SaveContent(out contentSctruct);
    //    }
    //    return contentSctruct;
    //}

    private void LoadGameSave()
    {
        if (LastGameSave.HasLastGame)
        {
            CurrentLevel = GetLevelAtt(LastGameSave.SaveRes.Mode);
            //DeleteGameSave();
        }
    }


    public void LoadGame()
    {
        LoadByJson();
    }


    private void SaveByJson()
    {
        if (NeedLoadGame)
        {
            if (!LevelEnd && CurrentLevel.CanSaveGame)
            {
                if (DraggingShape.PickingShape != null)//把当前正在摆放的内容进行撤回
                    DraggingShape.PickingShape.UndoShape();

                LastGameSave.SaveGame
                    (
                    SaveAllTechs(),
                    SavePickingTechs(),
                    SaveAllBlueprints(),
                    GameRes.SaveRes,
                    SaveContens(),
                    SaveSequences(),
                    SaveCurrentShapes(),
                    SaveRules(),
                    SaveBattleRecipes()
                    );
                LevelEnd = true;//是否在游戏状态和存档的FLAG

                string filePath2 = Application.persistentDataPath + "/GameSave.json";
                Debug.Log(filePath2);
                string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
                StreamWriter sw2 = new StreamWriter(filePath2);
                sw2.Write(EncryptionTool.EncryptString(saveJsonStr2, key));
                //sw2.Write(saveJsonStr2);
                sw2.Close();
                Debug.Log("战斗成功存档");
            }
            else
            {
                LastGameSave.ClearGame();
            }
            GameSaveContents.Clear();



            //try
            //{
            //    if (CurrentLevel.CanSaveGame && !LevelEnd)
            //        LastGameSave.SaveGame(SaveAllBlueprints(), GameRes.SaveRes, SaveContens(), SaveSequences());
            //    else
            //        LastGameSave.ClearGame();
            //    GameContents.Clear();

            //    string filePath2 = Application.persistentDataPath + "/GameSave.json";
            //    Debug.Log(filePath2);
            //    string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
            //    StreamWriter sw2 = new StreamWriter(filePath2);
            //    sw2.Write(saveJsonStr2);
            //    sw2.Close();
            //    Debug.Log("战斗成功存档");
            //}
            //catch
            //{
            //    Debug.LogWarning("战斗存档失败");
            //}

        }

    }

    private void LoadByJson()
    {
        SetGameLevel(GameLevel);//基于等级解锁内容

        if (NeedLoadGame && File.Exists(SaveGameFilePath))
        {
            try
            {
                StreamReader sr = new StreamReader(SaveGameFilePath);
                string jsonStr = sr.ReadToEnd();
                jsonStr = EncryptionTool.DecryptString(jsonStr, key);
                sr.Close();
                GameSave save = JsonMapper.ToObject<GameSave>(jsonStr);
                LastGameSave = save;
                LoadGameSave();
                Debug.Log("成功读取战斗");
            }
            catch
            {
                LastGameSave = new GameSave();
                Debug.LogWarning("读取战斗数据有错误");
            }

        }
        else
        {
            LastGameSave = new GameSave();
            Debug.Log("没有可读取战斗");
        }


    }


    public void SaveAll()
    {
        if (Game.Instance.CurrentState == "BattleState")
            SaveByJson();
    }


    private void OnApplicationQuit()//游戏结算时退出存空档
    {
        SaveAll();
    }
}

public class EncryptionTool
{
    /// <summary>
    /// 加密算法
    /// </summary>
    /// <param name="str">要加密的字符串</param>
    /// <param name="key">加密钥匙</param>
    /// <returns></returns>
    public static string EncryptString(string str, string key)
    {
        byte[] buffer;
        UTF8Encoding encoding = new UTF8Encoding();
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] buffer2 = provider.ComputeHash(encoding.GetBytes(key));
        TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
        {
            Key = buffer2,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        byte[] bytes = encoding.GetBytes(str);
        try
        {
            buffer = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }
        finally
        {
            provider2.Clear();
            provider.Clear();
        }
        return Convert.ToBase64String(buffer);
    }
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string DecryptString(string str, string key)
    {
        byte[] buffer;
        UTF8Encoding encoding = new UTF8Encoding();
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] buffer2 = provider.ComputeHash(encoding.GetBytes(key));
        TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
        {
            Key = buffer2,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        try
        {
            byte[] inputBuffer = Convert.FromBase64String(str);
            buffer = provider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }
        catch (Exception ex)
        {
            Debug.LogError("DecryptString failed. return empty string." + ex.Message);
            return "";
        }
        finally
        {
            provider2.Clear();
            provider.Clear();
        }
        return encoding.GetString(buffer);
    }
}
