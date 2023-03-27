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

    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> CurrentChallengeRankings { get; private set; }
    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> LastChallengeRankings { get; private set; }
    public async void GetLeaderBoard(LeaderBoard leaderBoardType)
    {
        switch (leaderBoardType)
        {
            case LeaderBoard.Endless:
                var endlessLeaderBoard = await LCLeaderboard.GetLeaderboard(EndlessWeeklyName);
                int endlessVersion = endlessLeaderBoard.Version;

                if (LevelManager.Instance.LocalEndlessVersion == endlessVersion)
                {
                    UpdateScore(leaderBoardType);
                    Debug.Log("�汾һ��,�汾��:" + endlessVersion + " ���·���,��ǰ����:" + LevelManager.Instance.LocalEndlessWave);
                }
                else
                {
                    LevelManager.Instance.LocalEndlessVersion = endlessVersion;
                    LevelManager.Instance.LocalEndlessWave = 0;
                    Debug.Log("�汾��һ��,�°汾��:" + endlessVersion + " ���÷�������ǰ����:" + LevelManager.Instance.LocalEndlessWave);
                }

                CurrentEndlessRankings = await endlessLeaderBoard.GetResults(version: endlessVersion, limit: 50, selectKeys: new List<string> { "nickname" });
                Debug.Log("GetEndlessResult1");
                if (endlessVersion > 0)
                    LastEndlessRankings = await endlessLeaderBoard.GetResults(version: (endlessVersion - 1), limit: 50, selectKeys: new List<string> { "nickname" });
                Debug.Log("GetEndlessResult2");
                GameEvents.Instance.EndlessLeaderboardGet();
                break;
            case LeaderBoard.Challenge:
                var challengeLeaderBoard = await LCLeaderboard.GetLeaderboard(ChallengeDayName);
                int challengeVersion = challengeLeaderBoard.Version;

                if (LevelManager.Instance.LocalChallengeVersion == challengeVersion)
                {
                    UpdateScore(leaderBoardType);
                    Debug.Log("�汾һ��,�汾��:" + challengeVersion + " ���·���,��ǰ����:" + LevelManager.Instance.LocalChallengeScore);
                }
                else
                {
                    LevelManager.Instance.LocalChallengeVersion = challengeVersion;
                    LevelManager.Instance.LocalChallengeScore = 0;
                    Debug.Log("�汾��һ��,�汾��:" + challengeVersion + " ���÷�������ǰ����:" + LevelManager.Instance.LocalChallengeScore);
                }

                CurrentChallengeRankings = await challengeLeaderBoard.GetResults(version: challengeVersion, limit: 50, selectKeys: new List<string> { "nickname" });
                Debug.Log("GetChallengeResult1");
                if (challengeVersion > 0)
                    LastChallengeRankings = await challengeLeaderBoard.GetResults(version: (challengeVersion - 1), limit: 50, selectKeys: new List<string> { "nickname" });
                Debug.Log("GetChallengeResult2");
                GameEvents.Instance.ChallengeLeaderboardGet();
                break;
        }

    }

    public async void GetLastLeaderboard()
    {
        var endlessLeaderBoard =LCLeaderboard.CreateWithoutData(EndlessWeeklyName);
        LastEndlessRankings = await endlessLeaderBoard.GetResults(version: 0, limit: 50, selectKeys: new List<string> { "nickname" });
        Debug.Log("GetEndlessResult2");
        GameEvents.Instance.EndlessLeaderboardGet();


        var challengeLeaderBoard = await LCLeaderboard.GetLeaderboard(ChallengeDayName);
        LastChallengeRankings = await challengeLeaderBoard.GetResults(version: challengeLeaderBoard.Version-1, limit: 50, selectKeys: new List<string> { "nickname" });
        Debug.Log("GetChallengeResult2");
        GameEvents.Instance.ChallengeLeaderboardGet();

    }

    public async void UpdateScore(LeaderBoard leaderBoardType)
    {
        int value = 0;
        string leaderBoard = "";
        switch (leaderBoardType)
        {
            case LeaderBoard.Endless:
                leaderBoard = EndlessWeeklyName;
                value = LevelManager.Instance.LocalEndlessWave;
                break;
            case LeaderBoard.Challenge:
                leaderBoard = ChallengeDayName;
                value = LevelManager.Instance.LocalChallengeScore;
                break;
        }
        if (value <= 0)
            return;
        var statistic = new Dictionary<string, double>();
        statistic[leaderBoard] = value;
        await LCLeaderboard.UpdateStatistics(TaptapManager.Instance.CurentUser, statistic);
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
