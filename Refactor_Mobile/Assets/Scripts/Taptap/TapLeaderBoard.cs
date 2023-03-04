using LeanCloud.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapLeaderBoard : MonoBehaviour
{
    public const string EndlessWeeklyName = "WeekEndless";
    public const string ChallengeDayName = "DayChallenge";

    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> CurrentEndlessRankings { get; private set; }
    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> LastEndlessRankings { get; private set; }


    public async void GetLeaderBoard()
    {
        var leaderboard = LCLeaderboard.CreateWithoutData(EndlessWeeklyName);
        int currentVersion = leaderboard.Version;

        if (LevelManager.Instance.LocalEndlessVersion == currentVersion)
        {
            UpdateScore(LeaderBoard.Endless);
            Debug.Log("版本一致，更新分数,当前波数:" + LevelManager.Instance.LocalEndlessWave);
        }
        else
        {
            LevelManager.Instance.LocalEndlessVersion = currentVersion;
            LevelManager.Instance.LocalEndlessWave = 0;
            Debug.Log("版本不一致，重置分数，当前波数:" + LevelManager.Instance.LocalEndlessWave);
        }


        CurrentEndlessRankings = await leaderboard.GetResults(limit: 10, selectKeys: new List<string> { "username", "nickname" });
        for (int i = 0; i < CurrentEndlessRankings.Count; i++)
        {
            Debug.Log("玩家:" + CurrentEndlessRankings[i].User["nickname"] + "排名:" + CurrentEndlessRankings[i].Rank + "分数:" + CurrentEndlessRankings[i].Value);
        }
        GameEvents.Instance.EndlessLeaderboardGet();
    }


    public async void UpdateScore(LeaderBoard leaderBoardType)
    {
        int wave = 0;
        string leaderBoard="";
        switch (leaderBoardType)
        {
            case LeaderBoard.Endless:
                leaderBoard = EndlessWeeklyName;
                wave = LevelManager.Instance.LocalEndlessWave;
                break;
            case LeaderBoard.Challenge:
                leaderBoard = ChallengeDayName;
                break;
        }
        var statistic = new Dictionary<string, double>();
        statistic[leaderBoard] = wave;
        await LCLeaderboard.UpdateStatistics(TaptapManager.Instance.CurentUser, statistic);
        Debug.Log("上传分数" + TaptapManager.Instance.CurentUser["nickname"] + LevelManager.Instance.LocalEndlessWave);
    }
    //public void UpdateLoacalScore()
    //{
    //    if (OnlineEndlessVersion == LocalEndlessVersion)//版本一致，上传本地分数
    //    {
    //        if (EndlessWave > 0)//大于0波才上传
    //            SendLeaderBoard(EndlessWeeklyName, EndlessWave);
    //    }
    //    else//版本不一致，重置本地分数，更新版本
    //    {
    //        LocalEndlessVersion = OnlineEndlessVersion;
    //        PlayerPrefs.SetInt(EndlessWeeklyName, 0);
    //    }

    //    if (OnlineChallengeVersion == LocalChallengeVersion)//版本一致，上传本地分数
    //    {
    //        if (ChallngeScore > 0)//大于0波才上传
    //            SendLeaderBoard(ChallengeDailyName, ChallngeScore);
    //    }
    //    else//版本不一致，重置本地分数，更新版本
    //    {
    //        LocalChallengeVersion = OnlineChallengeVersion;
    //        PlayerPrefs.SetInt(ChallengeDailyName, 0);
    //    }
    //}
}
