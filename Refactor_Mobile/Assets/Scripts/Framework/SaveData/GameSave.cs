using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class GameSave
{
    //玩家成就保存
    public List<AchievementStruct> SaveAchievements = new List<AchievementStruct>();
    //玩家等级保存
    public int GameLevel;
    public int GameExp;
    public int PassDifficulty;


    //临时游戏保存
    public bool HasLastGame;
    public List<BlueprintStruct> SaveBluePrints = new List<BlueprintStruct>();
    public List<TechnologyStruct> SaveTechnologies = new List<TechnologyStruct>();
    public List<TechnologyStruct> SavePickingTechs = new List<TechnologyStruct>();

    public bool ChallengeChoicePicked;


    public List<int> SaveRules = new List<int>();
    public GameResStruct SaveRes;

    public List<ContentStruct> SaveContents = new List<ContentStruct>();

    public List<string> SaveBattleRecipes = new List<string>();

    public List<EnemySequenceStruct> SaveSequences = new List<EnemySequenceStruct>();
    public List<ShapeInfo> SaveShapes = new List<ShapeInfo>();

    public void ClearGame()
    {
        HasLastGame = false;
        SaveBattleRecipes.Clear();
        SaveBluePrints.Clear();
        SaveTechnologies.Clear();
        SavePickingTechs.Clear();
        ChallengeChoicePicked = false;
        SaveRules.Clear();
        SaveRes = null;
        SaveContents.Clear();
        SaveSequences.Clear();
    }

    public void ClearData()
    {
        GameLevel = 0;
        GameExp = 0;
        PassDifficulty = 0;
    }

    public void SaveData(int gameLevel, int gameExp, int passDifficulty, List<AchievementStruct> saveAchievements)
    {
        GameLevel = gameLevel;
        GameExp = gameExp;
        PassDifficulty = passDifficulty;
        SaveAchievements = saveAchievements;
    }

    public void SaveGame(List<TechnologyStruct> saveTechs, List<TechnologyStruct> savePickingTechs, List<BlueprintStruct> saveBlueprints,
        GameResStruct saveRes, List<ContentStruct> saveContents,
        List<EnemySequenceStruct> saveSequences, List<ShapeInfo> currentShapes, List<int> saveRules,
        List<string> saveRecipes)
    {
        ChallengeChoicePicked = GameRes.ChallengeChoicePicked;
        SavePickingTechs = savePickingTechs;
        HasLastGame = true;
        SaveTechnologies = saveTechs;
        SaveBluePrints = saveBlueprints;
        SaveRes = saveRes;
        SaveContents = saveContents;
        SaveSequences = saveSequences;
        SaveShapes = currentShapes;
        SaveRules = saveRules;
        SaveBattleRecipes = saveRecipes;
    }
}

[Serializable]
public class BlueprintStruct
{
    public string Name;
    public List<int> ElementRequirements;
    public List<int> QualityRequirements;
}

[Serializable]
public class AchievementStruct
{
    public string Key;
    public bool IsGet;
}

[Serializable]
public class EnemySequenceStruct
{
    public List<EnemySequence> SequencesList;
}
[Serializable]
public class TechnologyStruct
{
    public int TechName;
    public bool IsAbnormal;
    public float TechSaveValue;
    public bool CanAbnormal;
}

[Serializable]
public class GameResStruct
{
    public int Mode;//0-6=standard,11=Endless
    public int Coin;
    public int Wave;
    public int BuildCost;
    public int SwitchTrapCost;
    public int CurrentLife;
    public int MaxLife;
    public int SystemLevel;
    public int SystemUpgradeCost;
    public int TotalRefactor;
    public long TotalDamage;
    public int ShopCapacity;
    public int NextRefreshTurn;
    public int PefectElementCount;
    public bool DrawThisTurn;
    public int DieProtect;
    public int BuyGroundCost;
    public int PerfectProgress;
    public int RefreashTechCost;
    public int SkillChip;
    public int FreeRefreshTech;

    //游戏时长
    public int GainGold;
    public DateTime StartTime;

    //挑战模式跳过次数
    public int SkipTimes;

}

[Serializable]
public class ContentStruct
{
    public int ContentType;

    public Vector2Int Pos;
    public int Direction;
    public string ContentName;
    //Turret
    public int Element;
    public int Quality;
    public string TotalDamage;
    public int ExtraSlot;

    public bool TrapRevealed;
    public bool IsAbnormalBuilding;

    public Dictionary<string, List<int>> SkillList;
    public Dictionary<string, List<int>> ElementsList;
    public Dictionary<string, bool> IsException;
}


//[Serializable]
//public class ItemLockInfo
//{
//    public string itemName;
//    public bool isLock;
//    public ItemLockInfo(string itemname, bool isLock)
//    {
//        this.itemName = itemname;
//        this.isLock = isLock;
//    }
//}
