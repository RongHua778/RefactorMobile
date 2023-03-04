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

public class AchievementManager : MySingleton<AchievementManager>
{
    public List<Achievement> AchList;

    [SerializeField] private AchievementUnlockTips unlockTips = default;
    private Dictionary<string, Achievement> achDIC;



    private void Start()
    {
        GameEvents.Instance.onGameEnd += GameEndAchievement;
        GameEvents.Instance.onBillboardSort += BillBoardAchievement;
        GameEvents.Instance.onPrepareNextWave += PrepareNextWaveAchievement;
    }

    private void PrepareNextWaveAchievement()
    {
        if (GameRes.GainGoldBattleTurn >= 2000)
        {
            GetAchievement("ACH_INCOME");
        }
    }

    private void BillBoardAchievement(BillBoardInfo billboardInfo)
    {
        if (LevelManager.Instance.LevelWin)
        {
            if ((float)billboardInfo.Turret.Strategy.TotalDamage / GameRes.TotalDamage >= 0.8f)
            {
                GetAchievement("ACH_SUPERCORE");
            }
        }
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Endless
            && GameRes.CurrentWave >= 80
            && billboardInfo.Rank == 0
            && RuleFactory.BattleRules.Count <= 0)
        {
            switch (billboardInfo.Turret.Strategy.Attribute.RefactorName)
            {
                case RefactorTurretName.Sniper:
                    GetAchievement("ACH_SNIPER");
                    break;
                case RefactorTurretName.Rapider:
                    GetAchievement("ACH_RAPIDER");
                    break;
                case RefactorTurretName.Constructor:
                    GetAchievement("ACH_CONSTRUCTOR");
                    break;
                case RefactorTurretName.Scatter:
                    GetAchievement("ACH_SCATTER");
                    break;
                case RefactorTurretName.Mortar:
                    GetAchievement("ACH_MORTAR");
                    break;
                case RefactorTurretName.Rotary:
                    GetAchievement("ACH_ROTARY");
                    break;
                case RefactorTurretName.Ultra:
                    GetAchievement("ACH_ULTRA");
                    break;
                case RefactorTurretName.Snow:
                    GetAchievement("ACH_SNOW");
                    break;
                case RefactorTurretName.Coordinator:
                    GetAchievement("ACH_COORDINATOR");
                    break;
                case RefactorTurretName.Boomerrang:
                    GetAchievement("ACH_BOOMERRANG");
                    break;
                case RefactorTurretName.Super:
                    GetAchievement("ACH_SUPER");
                    break;
                case RefactorTurretName.Core:
                    GetAchievement("ACH_CORE");
                    break;
                case RefactorTurretName.Chiller:
                    GetAchievement("ACH_CHILLER");
                    break;
                case RefactorTurretName.Firer:
                    GetAchievement("FIRER");
                    break;
                case RefactorTurretName.Laser:
                    GetAchievement("ACH_LASER");
                    break;
                case RefactorTurretName.Bombard:
                    GetAchievement("ACH_BOMBARD");
                    break;
                case RefactorTurretName.Miner:
                    GetAchievement("ACH_MINER");
                    break;
                case RefactorTurretName.Nuclear:
                    GetAchievement("ACH_NUCLEAR");
                    break;
            }
        }
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
            Debug.LogWarning("�����ڸóɾ�:" + key);
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

        if (LevelManager.Instance.LifeTotalRefactor >= 300)//�ع�����������ָ��
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

        if (LevelManager.Instance.LifeTotalCoin >= 3000000)//��Ǯ����
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

        if (GameRes.MaxSingleDamage >= 100000000)//�����˺�1��
        {
            GetAchievement("ACH_ONLYBULLET");
        }

        switch (LevelManager.Instance.CurrentLevel.ModeType)
        {
            case ModeType.Standard:
                if (LevelManager.Instance.CurrentLevel.Level >= 4)
                {
                    if (GameRes.Life == LevelManager.Instance.CurrentLevel.PlayerHealth)//ȫ���ƿأ�����ʧ����ֵͨ��
                        GetAchievement("ACH_EASY");
                    if (GameRes.MaxPath <= 3 && GameRes.DieProtect >= 1)
                        GetAchievement("ACH_EXTREME");//���޲���
                    if (GameRes.TotalRefactor <= 0)
                        GetAchievement("ACH_DECEVIVE");
                }
                if (LevelManager.Instance.CurrentLevel.Level >= 9)//С��һ����ͨ���Ѷ�9
                {
                    if (endData.Win)
                        GetAchievement("ACH_CAKE");//С��һ����ͨ���Ѷ�9
                    TimeSpan ts = DateTime.Now - GameRes.LevelStart;
                    if (ts.Minutes <= 9)
                    {
                        GetAchievement("ACH_FAST");//��ͨ���,10����ͨ���Ѷ�9
                    }
                }
                break;
            case ModeType.Endless:
                if (RuleFactory.BattleRules.Count <= 0)
                {
                    if (GameRes.CurrentWave >= 120)
                    {
                        GetAchievement("ACH_ENDLESS4");//�޾�֮·4
                        GetAchievement("ACH_ENDLESS3");//�޾�֮·3
                        GetAchievement("ACH_ENDLESS2");//�޾�֮·2
                        GetAchievement("ACH_ENDLESS1");//�޾�֮·1
                    }
                    else if (GameRes.CurrentWave >= 100)
                    {
                        GetAchievement("ACH_ENDLESS3");//�޾�֮·3
                        GetAchievement("ACH_ENDLESS2");//�޾�֮·2
                        GetAchievement("ACH_ENDLESS1");//�޾�֮·1
                    }
                    else if (GameRes.CurrentWave >= 80)
                    {
                        GetAchievement("ACH_ENDLESS2");//�޾�֮·2
                        GetAchievement("ACH_ENDLESS1");//�޾�֮·1
                    }
                    else if (GameRes.CurrentWave >= 60)
                    {
                        GetAchievement("ACH_ENDLESS1");//�޾�֮·1
                    }
                }
                break;

        }

        if (GameRes.MaxPath >= 200)
        {
            GetAchievement("ACH_LONGPATH");
        }

    }

}
