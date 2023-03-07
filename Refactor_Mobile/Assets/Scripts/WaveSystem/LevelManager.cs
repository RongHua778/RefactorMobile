using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using LitJson;
using System.Security.Cryptography;
using System.Text;
using System;



[System.Serializable]
public struct GameLevelInfo
{
    public int ExpRequire;
    public ContentAttribute[] UnlockItems;
}

public class LevelManager : MySingleton<LevelManager>
{
    public bool LevelWin { get; set; }
    #region ����ģʽ
    [SerializeField]
    private int localEndlessVersion;//�����޾��汾
    public int LocalEndlessVersion
    {
        get => localEndlessVersion;
        set => localEndlessVersion = value;
    }
    [SerializeField]
    private int localEndlessWave;//�����޾�����
    public int LocalEndlessWave
    {
        get => localEndlessWave;
        set => localEndlessWave = value;
    }

    [SerializeField]
    private int localChallengeVersion;//������ս�汾
    public int LocalChallengeVersion
    {
        get => localChallengeVersion;
        set => localChallengeVersion = value;
    }
    [SerializeField]
    private int localChallengeScore;//������ս����
    public int LocalChallengeScore
    {
        get => localChallengeScore;
        set => localChallengeScore = value;
    }

    #endregion

    #region ��������

    private string key = "123456789";

    [SerializeField] private GameLevelInfo[] GameLevels = default;
    [Header("�������ȼ�")]
    [SerializeField] int permitGameLevel = default;
    public int PermitDifficulty = default;
    public int MaxGameLevel => Mathf.Min(GameLevels.Length - 1, permitGameLevel);

    private int gameLevel;
    public int GameLevel
    {
        get => gameLevel;
        set => gameLevel = Mathf.Min(MaxGameLevel, value);
    }
    private int gameExp;
    public int GameExp
    {
        get => gameExp;
        set => gameExp = value;
    }
    private int passDifficulty;
    public int PassDifficulty
    {
        get => passDifficulty;
        set => passDifficulty = Mathf.Clamp(value, 0, 9);
    }

    [SerializeField]
    private int lifeTotalRefactor;
    public int LifeTotalRefactor { get => lifeTotalRefactor; set => lifeTotalRefactor = value; }
    [SerializeField]
    private int lifeTotalCoin;
    public int LifeTotalCoin { get => lifeTotalCoin; set => lifeTotalCoin = value; }

    public List<ContentAttribute> AllContent;

    public LevelAttribute[] StandardLevels = default;

    public LevelAttribute[] ChallengeLevels = default;

    private Dictionary<int, LevelAttribute> LevelDIC;


    public LevelAttribute CurrentLevel;
    #endregion

    private string SaveGameFilePath;


    //��������ʼ��Ϸ
    [HideInInspector]
    public bool StartingGame = false;


    #region ��ʱ��Ϸ����
    [HideInInspector] public List<GameTileContent> GameSaveContents;
    [Header("�Ƿ��ȡ�浵")]
    [SerializeField] bool NeedLoadGame = default;
    [Header("�Ƿ���δ�����Ϸ")]
    public GameSave LastGameSave;
    public bool LevelEnd = true;//��Ϸ�Ƿ����
    #endregion

    public void Initialize()
    {
        LitJsonRegister.Register();
        SaveGameFilePath = Application.persistentDataPath + "/GameSave.json";

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
        PassDifficulty = 9;
    }



    private void DeleteGameSave()//ɾ���Ծִ浵�ļ�
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
            Debug.LogWarning("�����ģʽ����");
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
        for (int i = 0; i < GameLevel + 1; i++)//�������ڵ�ǰ�ȼ��Ľ���
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
        if (GameLevel >= MaxGameLevel)//�ﵽ���ȼ���ֻ�Ӿ��鲻����
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

    //�浵����

    public void StartNewGame(int modeID)
    {
        if (StartingGame)
            return;
        StartingGame = true;//��ֹ�������

        CurrentLevel = GetLevelAtt(modeID);
        if (CurrentLevel == null)
        {
            Debug.LogWarning("û���ҵ���Ӧ�Ĺؿ��ļ�");
            return;
        }

        LastGameSave.ClearGame();//�������ս��
        GameSaveContents.Clear();//�����������
        DeleteGameSave();//ɾ���浵�ļ�
        Game.Instance.LoadScene(1);
    }


    public void ContinueLastGame()
    {
        if (StartingGame)
            return;
        StartingGame = true;

        //ȱ��һ��LoadRateList

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
    private List<AchievementStruct> SaveAchievements()
    {
        List<AchievementStruct> saveList = new List<AchievementStruct>();
        foreach (var ach in AchievementManager.Instance.AchList)
        {
            AchievementStruct achStruct = new AchievementStruct();
            achStruct.Key = ach.AchKey;
            achStruct.IsGet = ach.IsGet;
            saveList.Add(achStruct);
        }
        return saveList;
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

    private void LoadGameSave()
    {
        if (LastGameSave.HasLastGame)
        {
            CurrentLevel = GetLevelAtt(LastGameSave.SaveRes.Mode);
        }
    }

    private void LoadDataSave()
    {
        GameLevel = LastGameSave.GameLevel;
        GameExp = LastGameSave.GameExp;
        PassDifficulty = LastGameSave.PassDifficulty;
        LifeTotalRefactor = LastGameSave.LifeTotalRefactor;
        LifeTotalCoin = LastGameSave.LifeTotalCoin;

        LocalEndlessVersion = LastGameSave.LocalEndlessVersion;
        LocalEndlessWave = LastGameSave.LocalEndlessWave;

        LocalChallengeVersion = LastGameSave.LocalChallengeVersion;
        LocalChallengeScore = LastGameSave.LocalChalleneScore;
    }


    public void LoadGame()
    {
        LoadByJson();
        AchievementManager.Instance.LoadAch();
    }


    private void SaveByJson()
    {
        if (NeedLoadGame)
        {

            if (!LevelEnd && CurrentLevel.CanSaveGame)
            {
                if (DraggingShape.PickingShape != null)//�ѵ�ǰ���ڰڷŵ����ݽ��г���
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
                LevelEnd = true;//�Ƿ�����Ϸ״̬�ʹ浵��FLAG

            }
            else if (!LevelEnd && !CurrentLevel.CanSaveGame)
            {
                LastGameSave.ClearGame();
            }

            GameSaveContents.Clear();
            LastGameSave.SaveData(SaveAchievements());


            string filePath2 = Application.persistentDataPath + "/GameSave.json";
            Debug.Log(filePath2);
            string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
            StreamWriter sw2 = new StreamWriter(filePath2);
            sw2.Write(EncryptionTool.EncryptString(saveJsonStr2, key));
            //sw2.Write(saveJsonStr2);
            sw2.Close();
            Debug.Log("ս���ɹ��浵");


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
            //    Debug.Log("ս���ɹ��浵");
            //}
            //catch
            //{
            //    Debug.LogWarning("ս���浵ʧ��");
            //}

        }

    }

    private void LoadByJson()
    {

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
                LoadDataSave();
                Debug.Log("�ɹ���ȡս��");
            }
            catch
            {
                LastGameSave = new GameSave();
                Debug.LogWarning("��ȡս�������д���");
            }

        }
        else
        {
            LastGameSave = new GameSave();
            Debug.Log("û�пɶ�ȡս��");
        }
        SetGameLevel(GameLevel);//���ڵȼ���������

    }


    public void SaveAll()
    {
        SaveByJson();
    }


    private void OnApplicationQuit()//��Ϸ����ʱ�˳���յ�
    {
        SaveAll();
    }
}

public class EncryptionTool
{
    /// <summary>
    /// �����㷨
    /// </summary>
    /// <param name="str">Ҫ���ܵ��ַ���</param>
    /// <param name="key">����Կ��</param>
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
    /// ����
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
