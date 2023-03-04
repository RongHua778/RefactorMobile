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
            Debug.Log("�汾һ�£����·���,��ǰ����:" + LevelManager.Instance.LocalEndlessWave);
        }
        else
        {
            LevelManager.Instance.LocalEndlessVersion = currentVersion;
            LevelManager.Instance.LocalEndlessWave = 0;
            Debug.Log("�汾��һ�£����÷�������ǰ����:" + LevelManager.Instance.LocalEndlessWave);
        }


        CurrentEndlessRankings = await leaderboard.GetResults(limit: 10, selectKeys: new List<string> { "username", "nickname" });
        for (int i = 0; i < CurrentEndlessRankings.Count; i++)
        {
            Debug.Log("���:" + CurrentEndlessRankings[i].User["nickname"] + "����:" + CurrentEndlessRankings[i].Rank + "����:" + CurrentEndlessRankings[i].Value);
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
        Debug.Log("�ϴ�����" + TaptapManager.Instance.CurentUser["nickname"] + LevelManager.Instance.LocalEndlessWave);
    }
    //public void UpdateLoacalScore()
    //{
    //    if (OnlineEndlessVersion == LocalEndlessVersion)//�汾һ�£��ϴ����ط���
    //    {
    //        if (EndlessWave > 0)//����0�����ϴ�
    //            SendLeaderBoard(EndlessWeeklyName, EndlessWave);
    //    }
    //    else//�汾��һ�£����ñ��ط��������°汾
    //    {
    //        LocalEndlessVersion = OnlineEndlessVersion;
    //        PlayerPrefs.SetInt(EndlessWeeklyName, 0);
    //    }

    //    if (OnlineChallengeVersion == LocalChallengeVersion)//�汾һ�£��ϴ����ط���
    //    {
    //        if (ChallngeScore > 0)//����0�����ϴ�
    //            SendLeaderBoard(ChallengeDailyName, ChallngeScore);
    //    }
    //    else//�汾��һ�£����ñ��ط��������°汾
    //    {
    //        LocalChallengeVersion = OnlineChallengeVersion;
    //        PlayerPrefs.SetInt(ChallengeDailyName, 0);
    //    }
    //}
}
