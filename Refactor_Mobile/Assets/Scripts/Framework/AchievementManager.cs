using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void GetAchievement(string key)
    {
        foreach (var ach in AchList)
        {
            if (ach.AchKey.CompareTo(key) == 0)
            {
                ach.IsGet = true;
                break;
            }
        }
    }

    public void LoadAch()
    {
        foreach (var achStruct in LevelManager.Instance.LastGameSave.SaveAchievements)
        {
            foreach (var ach in AchList)
            {
                if (ach.AchKey.CompareTo(achStruct.Key) == 0)
                {
                    ach.IsGet = achStruct.IsGet;
                    break;
                }
            }
        }
    }

}
