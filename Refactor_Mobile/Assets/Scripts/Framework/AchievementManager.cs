using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Achievement
{
    public string AchKey;
    public bool IsGet;
    public Sprite AchIcon;
}

public class AchievementManager : Singleton<AchievementManager>
{
    public List<Achievement> AchList;

    [SerializeField] private AchievementUnlockTips unlockTips = default;
    private Dictionary<string, Achievement> achDIC;
    


    private void Start()
    {
        GameEvents.Instance.onGameEnd += GameEndAchievement;
    }

    public void GetAchievement(string key, bool value = true)
    {
        if (achDIC.ContainsKey(key))
        {
            achDIC[key].IsGet = value;
            if (value)
            {
                unlockTips.UnlockAchievement(achDIC[key]);
            }
        }
        else
        {
            Debug.LogWarning("不存在该成就:" + key);
        }
    }

    public void LoadAch()
    {
        achDIC = new Dictionary<string, Achievement>();
        foreach (var achStruct in LevelManager.Instance.LastGameSave.SaveAchievements)
        {
            foreach (var ach in AchList)
            {
                if (ach.AchKey.CompareTo(achStruct.Key) == 0)
                {
                    ach.IsGet = achStruct.IsGet;
                    achDIC.Add(ach.AchKey, ach);
                    break;
                }
            }
        }
    }



    private void GameEndAchievement(GameEndStruct endData)
    {
        LevelManager.Instance.LifeTotalRefactor += GameRes.TotalRefactor;
        LevelManager.Instance.LifeTotalCoin += GameRes.GainGold;

        if (LevelManager.Instance.LifeTotalRefactor >= 300)//重构次数，传奇指挥
        {
            GetAchievement("ACH_REFACTOR4");
        }
        else if (LevelManager.Instance.LifeTotalRefactor >= 100)
        {
            GetAchievement("ACH_REFACTOR3");
        }
        else if (LevelManager.Instance.LifeTotalRefactor >= 30)
        {
            GetAchievement("ACH_REFACTOR2");
        }
        else if (LevelManager.Instance.LifeTotalRefactor >= 5)
        {
            GetAchievement("ACH_REFACTOR1");
        }

        if (LevelManager.Instance.LifeTotalCoin >= 3000000)//打钱狂人
        {
            GetAchievement("ACH_MONEY4");
        }
        else if (LevelManager.Instance.LifeTotalCoin >= 1000000)
        {
            GetAchievement("ACH_MONEY3");
        }
        else if (LevelManager.Instance.LifeTotalCoin >= 300000)
        {
            GetAchievement("ACH_MONEY2");
        }
        else if (LevelManager.Instance.LifeTotalCoin >= 50000)
        {
            GetAchievement("ACH_MONEY1");
        }

        if (GameRes.MaxSingleDamage >= 100000000)//单发伤害1亿
        {
            GetAchievement("ACH_ONLYBULLET");
        }

        switch (LevelManager.Instance.CurrentLevel.ModeType)
        {
            case ModeType.Standard:
                if (LevelManager.Instance.CurrentLevel.Level >= 4)
                {
                    if (GameRes.Life == LevelManager.Instance.CurrentLevel.PlayerHealth)//全能掌控，不损失生命值通关
                        GetAchievement("ACH_EASY");
                }
                if (LevelManager.Instance.CurrentLevel.Level >= 9)//小菜一碟，通关难度9
                {
                    GetAchievement("ACH_CAKE");//小菜一碟，通关难度9
                    TimeSpan ts = DateTime.Now - GameRes.LevelStart;
                    if (ts.Minutes <= 9)
                    {
                        LevelManager.Instance.SetAchievement("ACH_FASTLEVEL6");//速通玩家,10分钟通关难度9
                    }
                }
                break;
            case ModeType.Endless:
                break;

        }


        //if (GameRes.MaxPath >= 200)
        //{
        //    GetAchievement("ACH_LONGPATH")
        //}
    }

}
