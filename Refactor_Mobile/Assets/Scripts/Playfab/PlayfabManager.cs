using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
//using Steamworks;
using System;

public struct LeaderBoardInfo
{
    public int Version;
    public GetLeaderboardResult LeaderBoardResult;
}

public class PlayfabManager : Singleton<PlayfabManager>
{

    //每周无尽
    public const string EndlessWeeklyName = "Endless_Week";
    public LeaderBoardInfo[] EndlessResult = new LeaderBoardInfo[2];
    public int OnlineEndlessVersion;
    public int LocalEndlessVersion//当前无尽模式的本地版本
    {
        get => PlayerPrefs.GetInt(EndlessWeeklyName + "Version", 0);
        set => PlayerPrefs.SetInt(EndlessWeeklyName + "Version", value);
    }
    public int EndlessWave //当前无尽模式的本地分数
    {
        get => PlayerPrefs.GetInt(EndlessWeeklyName, 0);
        set
        {
            if (value > PlayerPrefs.GetInt(EndlessWeeklyName, 0))
            {
                PlayerPrefs.SetInt(EndlessWeeklyName, value);
            }
        }
    }

    //每日挑战
    public const string ChallengeDailyName = "Challenge_Daily";

    public LeaderBoardInfo[] ChallengeResults = new LeaderBoardInfo[2];
    public int OnlineChallengeVersion;
    public int LocalChallengeVersion//当前挑战模式的本地版本
    {
        get => PlayerPrefs.GetInt(ChallengeDailyName + "Version", 0);
        set => PlayerPrefs.SetInt(ChallengeDailyName + "Version", value);
    }
    public int ChallngeScore //当前挑战模式的本地分数
    {
        get => PlayerPrefs.GetInt(ChallengeDailyName, 0);
        set
        {
            if (value > PlayerPrefs.GetInt(ChallengeDailyName, 0))
            {
                PlayerPrefs.SetInt(ChallengeDailyName, value);
            }
        }
    }

    public bool EndlessLeaderboardGot { get; set; }
    public bool ChallengeLeaderboardGot { get; set; }


    private bool FirstLogin = true;

    public void Login()
    {
        if (FirstLogin)
        {
            var request = new LoginWithCustomIDRequest
            {
                //CustomId = SteamFriends.GetPersonaName(),
                CustomId = "KingdomJack",

                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        }
        else
        {
            UpdateLoacalScore();
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Successfully Login" + result.PlayFabId);
        SetPlayerDisplayName(result);
    }

    private void SetPlayerDisplayName(LoginResult result)
    {
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            if (name == null)
            {
                var request = new UpdateUserTitleDisplayNameRequest
                {
                    //DisplayName = SteamFriends.GetPersonaName()
                    DisplayName = "KindomJack"

                };
                PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
            }
            else
            {
                GetEndlessVersion();
                //GetChallengeVersion();
            }
        }
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        GetEndlessVersion();
        //GetChallengeVersion();

        Debug.Log("Update display name");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.Error);
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderBoard(string leaderBoard, int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
               new StatisticUpdate
               {
                   StatisticName=leaderBoard,
                   Value=score
               }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Successful Leader board Update");
    }


    //public void GetLeaderBoard(string leaderBoardName)
    //{
    //    LeaderboardGot = false;
    //    GettingLeaderBoard = leaderBoardName;
    //    var request = new GetLeaderboardRequest
    //    {
    //        StatisticName = leaderBoardName,
    //        StartPosition = 0,
    //        MaxResultsCount = 50
    //    };
    //    PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    //}

    public void GetEndlessVersion()
    {
        EndlessLeaderboardGot = false;
        var request = new GetPlayerStatisticVersionsRequest
        {
            StatisticName = EndlessWeeklyName
        };
        PlayFabClientAPI.GetPlayerStatisticVersions(request, OnEndlessVersionGet, OnError);

    }

    public void GetChallengeVersion()
    {
        ChallengeLeaderboardGot = false;
        var request = new GetPlayerStatisticVersionsRequest
        {
            StatisticName = ChallengeDailyName
        };
        PlayFabClientAPI.GetPlayerStatisticVersions(request, OnChallengeVersionGet, OnError);
    }


    private void OnEndlessVersionGet(GetPlayerStatisticVersionsResult result)
    {
        //更新本地版本
        OnlineEndlessVersion = (int)result.StatisticVersions[result.StatisticVersions.Count - 1].Version;

        GetLeaderBoard(EndlessWeeklyName, OnlineEndlessVersion);
        if (OnlineEndlessVersion > 0)
            GetLeaderBoard(EndlessWeeklyName, OnlineEndlessVersion - 1);

        GetChallengeVersion();
    }

    private void OnChallengeVersionGet(GetPlayerStatisticVersionsResult result)
    {
        //更新本地版本
        OnlineChallengeVersion = (int)result.StatisticVersions[result.StatisticVersions.Count - 1].Version;

        GetLeaderBoard(ChallengeDailyName, OnlineChallengeVersion);
        if (OnlineChallengeVersion > 0)
            GetLeaderBoard(ChallengeDailyName, OnlineChallengeVersion - 1);
    }

    private void GetLeaderBoard(string leaderBoardName, int version)
    {
        switch (leaderBoardName)
        {
            case EndlessWeeklyName:
                var request = new GetLeaderboardRequest
                {
                    StatisticName = leaderBoardName,
                    StartPosition = 0,
                    MaxResultsCount = 50,
                    Version = version
                };
                PlayFabClientAPI.GetLeaderboard(request, OnEndlessLeaderBoardGet, OnError);
                break;
            case ChallengeDailyName:
                var request2 = new GetLeaderboardRequest
                {
                    StatisticName = leaderBoardName,
                    StartPosition = 0,
                    MaxResultsCount = 50,
                    Version = version
                };
                PlayFabClientAPI.GetLeaderboard(request2, OnChallengeLeaderBoardGet, OnError);
                break;
        }

    }


    private void OnChallengeLeaderBoardGet(GetLeaderboardResult result)
    {
        ChallengeResults[OnlineChallengeVersion - result.Version].LeaderBoardResult = result;
        ChallengeLeaderboardGot = true;
        GameEvents.Instance.ChallengeLeaderboardGet(true);
        if (FirstLogin)
        {
            UpdateLoacalScore();
            FirstLogin = false;
        }
        GameEvents.Instance.ChallengeLeaderboardGet(true);
        Debug.Log("Successfully get leaderboard：" + ChallengeDailyName + result.Version);

    }

    private void OnEndlessLeaderBoardGet(GetLeaderboardResult result)
    {
        EndlessResult[OnlineEndlessVersion - result.Version].LeaderBoardResult = result;
        EndlessLeaderboardGot = true;
        if (FirstLogin)
        {
            UpdateLoacalScore();
            FirstLogin = false;
        }
        GameEvents.Instance.EndlessLeaderboardGet(true);
        Debug.Log("Successfully get leaderboard：" + EndlessWeeklyName);
    }

    public void UpdateLoacalScore()
    {
        if (OnlineEndlessVersion == LocalEndlessVersion)//版本一致，上传本地分数
        {
            if (EndlessWave > 0)//大于0波才上传
                SendLeaderBoard(EndlessWeeklyName, EndlessWave);
        }
        else//版本不一致，重置本地分数，更新版本
        {
            LocalEndlessVersion = OnlineEndlessVersion;
            PlayerPrefs.SetInt(EndlessWeeklyName, 0);
        }

        if (OnlineChallengeVersion == LocalChallengeVersion)//版本一致，上传本地分数
        {
            if (ChallngeScore > 0)//大于0波才上传
                SendLeaderBoard(ChallengeDailyName, ChallngeScore);
        }
        else//版本不一致，重置本地分数，更新版本
        {
            LocalChallengeVersion = OnlineChallengeVersion;
            PlayerPrefs.SetInt(ChallengeDailyName, 0);
        }
    }


}

//public class GetLeaderBoardTask
//{
//    public GetLeaderBoardTask NextTask;
//    public int LeaderBoardVersion;
//    private string m_leaderboardName;

//    public GetLeaderBoardTask(GetLeaderBoardTask nextTask, int version)
//    {
//        this.NextTask = nextTask;
//        this.LeaderBoardVersion = version;
//    }
//    public GetLeaderboardRequest GetLeaderBoard(string leaderBoardName)
//    {
//        m_leaderboardName = leaderBoardName;
//        var request = new GetLeaderboardRequest
//        {
//            StatisticName = leaderBoardName,
//            StartPosition = 0,
//            MaxResultsCount = 50,
//            Version = LeaderBoardVersion
//        };
//        return request;
//    }

//    public void OnGetLeaderBoardComplete()
//    {
//        if (NextTask != null)
//            NextTask.GetLeaderBoard(m_leaderboardName);
//    }
//}
