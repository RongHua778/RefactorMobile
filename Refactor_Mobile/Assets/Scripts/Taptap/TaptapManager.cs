using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Login;
using System;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using LeanCloud.Storage;
using TapTap.TapDB;
using System.Threading.Tasks;
using LeanCloud;
using UnityEngine.Networking;
using System.IO;

public class TaptapManager : MySingleton<TaptapManager>
{
    public bool LoginSuccessful { get; set; }
    public TDSUser CurentUser { get; set; }
    [SerializeField] private TapLeaderBoard tapLeaderBoard = default;

    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> CurrentEndlessRankings => tapLeaderBoard.CurrentEndlessRankings;
    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> LastEndlessRankings => tapLeaderBoard.LastEndlessRankings;

    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> CurrentChallengeRankings => tapLeaderBoard.CurrentChallengeRankings;
    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> LastChallengeRankings => tapLeaderBoard.LastChallengeRankings;
    // Start is called before the first frame update

    public void Initialized()
    {
        LCLogger.LogDelegate = (LCLogLevel level, string info) =>
        {
            switch (level)
            {
                case LCLogLevel.Debug:
                    Debug.Log($"[DEBUG] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Warn:
                    Debug.Log($"[WARNING] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Error:
                    Debug.Log($"[ERROR] {DateTime.Now} {info}\n");
                    break;
                default:
                    Debug.Log(info);
                    break;
            }
        };
        InitTaptap();
        InitAntiAddiction();
        //UpdateGame();
    }

    private static void StartAntiAddiction()
    {
        string userIdentifier = "RefactorMobile";
        AntiAddictionUIKit.Startup(userIdentifier);

    }

    private async void UpdateGame()
    {
        //TapCommon.UpdateGameInTapTap("228703",callSuccess=>
        //{
        //    if (callSuccess)
        //    {
        //        Debug.Log("Taptap 唤起成功");
        //    }
        //    else
        //    {
        //        Debug.Log("Taptap 唤起失败");
        //    }
        //});
        bool isSuccess = await TapCommon.UpdateGameAndFailToWebInTapTap("228703");
    }

    private static void InitAntiAddiction()
    {
        AntiAddictionConfig config = new AntiAddictionConfig()
        {
            gameId = "gjbbmb3iasltjf9taz",      // TapTap 开发者中心对应 Client ID
            useTapLogin = true,             // 是否启动 TapTap 快速认证
            showSwitchAccount = false,      // 是否显示切换账号按钮
        };

        Action<int, string> callback = (code, errorMsg) =>
        {
            //code == 500;   //登录成功
            //code = 1000;   //用户登出
            //code = 1001;   //切换账号
            //code = 1030;   //用户当前无法进行游戏
            //code = 1050;   //时长限制
            //code = 9002;   //实名过程中点击了关闭实名窗
            UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");
        };

        AntiAddictionUIKit.Init(config, callback);
    }

    private void InitTaptap()
    {
        var config = new TapConfig.Builder()
                .ClientID("gjbbmb3iasltjf9taz")
                .ClientToken("cBCS3rrso0aQqk3mVhxlSVwGLlviCtCRXukt7LV6")
                .ServerURL("https://gjbbmb3i.cloud.tds1.tapapis.cn")
                .RegionType(RegionType.CN)
                .TapDBConfig(true, "", "", true)
                .ConfigBuilder();
        TapBootstrap.Init(config);
    }

    public async Task TapLogin()
    {
        LoginSuccessful = false;
        CurentUser = await TDSUser.GetCurrent();
        if (null == CurentUser)
        {
            Debug.Log("当前未登录");
            // 开始登录
            try
            {
                // 在 iOS、Android 系统下会唤起 TapTap 客户端或以 WebView 方式进行登录
                // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
                CurentUser = await TDSUser.LoginWithTapTap();
                // 获取 TDSUser 属性
                var objectId = CurentUser.ObjectId;     // 用户唯一标识
                var nickname = CurentUser["nickname"];  // 昵称
                var avatar = CurentUser["avatar"];      // 头像
                TipsManager.Instance.ShowMessage("登录成功:" + nickname);
                LoginSuccessful = true;
                Debug.Log("登录成功:" + nickname);
                StartAntiAddiction();
                TapDB.SetUser(CurentUser.ObjectId);
                tapLeaderBoard.GetLeaderBoard(LeaderBoard.Endless);
                tapLeaderBoard.GetLeaderBoard(LeaderBoard.Challenge);

            }
            catch (Exception e)
            {
                if (e is TapException tapError)  // using TapTap.Common
                {
                    Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                    if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                    {
                        Debug.Log("登录取消");
                    }
                }
            }
        }
        else
        {
            Debug.Log("已登录");
            TapDB.SetUser(CurentUser.ObjectId);
            LoginSuccessful = true;
            tapLeaderBoard.GetLeaderBoard(LeaderBoard.Endless);
            tapLeaderBoard.GetLeaderBoard(LeaderBoard.Challenge);
            // 进入游戏
        }


    }
    //TapGameSave gameSave;
    public async Task UpLoadTapGameSave()
    {
        if (File.Exists(LevelManager.Instance.SaveGameFilePath))//存在本地存档
        {
            var gameSave = new TapGameSave
            {
                Name = "RefactorGameSave",
                Summary = "data",
                ModifiedAt = DateTime.Now.ToLocalTime(),
                PlayedTime = LevelManager.Instance.GameExp, // ms
                ProgressValue = LevelManager.Instance.GameLevel,
                //CoverFilePath = image_local_path, // jpg/png
                GameFilePath = LevelManager.Instance.SaveGameFilePath
            };
            await gameSave.Save();
        }
    }

    public async Task SaveTapGame()
    {
        await myGameSave.Save();
        Debug.Log("Save Tap Game");
    }

    //public void DebugCurrentData()
    //{
    //    Debug.Log()
    //}


    private TapGameSave myGameSave;
    public async Task GetTapGameSave()
    {
        //确保本地低经验存档不会覆盖高经验存档
        var collection = await TapGameSave.GetCurrentUserGameSaves();

        if (collection.Count > 0)
        {
            Debug.Log("Game Save Collection has:" + collection.Count);
            string gameFileUrl = "";
            int onlineExp = 0;
            int onlineLevel = 0;
            foreach (var gameSave in collection)
            {
                var summary = gameSave.Summary;
                var modifiedAt = gameSave.ModifiedAt;
                onlineLevel = (int)gameSave.PlayedTime;
                onlineExp = gameSave.ProgressValue;
                var coverFile = gameSave.Cover;
                var gameFile = gameSave.GameFile;
                gameFileUrl = gameFile.Url;
                myGameSave = gameSave;
                break;
            }
            //与本地文件进行比较
            if (onlineLevel == LevelManager.Instance.GameLevel)//等级相同时比较经验值
            {
                if (onlineExp > LevelManager.Instance.GameExp)//线上版本的经验值更高，只有大于时才下载
                {
                    await DownloadFileAsync(gameFileUrl);
                }
                else
                {
                    await SaveTapGame();//更新线上版本
                }
            }
            else if (onlineLevel > LevelManager.Instance.GameLevel)//线上版本的等级更高
            {
                await DownloadFileAsync(gameFileUrl);//下载
            }
            else//线下版本的等级比较高
            {
                await SaveTapGame();//更新线上版本
            }

        }
        else
        {
            await UpLoadTapGameSave();//没有存档时上传存档
            Debug.Log("无在线存档");
        }
    }

    //private async Task DownLoadFile(string url)
    //{
    //    string localurl = Application.persistentDataPath + "/GameSave.json";
    //    UnityWebRequest webRequest = new UnityWebRequest(url);
    //    DownloadHandlerFile downLoad = new DownloadHandlerFile(localurl);
    //    webRequest.downloadHandler = downLoad;
    //    yield return webRequest.SendWebRequest();
    //    while (!webRequest.isDone)
    //    {
    //        yield return null;
    //    }
    //    if (string.IsNullOrEmpty(webRequest.error))
    //    {
    //        Debug.Log("下载成功");
    //    }
    //    else
    //    {
    //        Debug.Log("下载失败");
    //    }
    //}

    private async Task DownloadFileAsync(string url)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }
            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new UnityWebRequestException(request.error);
            }

            var filePath = Application.persistentDataPath + "/GameSave.json";
            await File.WriteAllBytesAsync(filePath, request.downloadHandler.data);
            Debug.Log($"File GameSave.json downloaded from {url} and saved to {filePath}");
        }
    }

    public class UnityWebRequestException : System.Exception
    {
        public UnityWebRequestException(string message) : base(message) { }
    }

    public void TapLogout()
    {
        TDSUser.Logout();
        AntiAddictionUIKit.Exit();
        TipsManager.Instance.ShowMessage("成功登出");
        Debug.Log("成功登出");
    }

    public void TrackEvents()
    {
        TapDB.TrackEvent("#TestEvent", "{\"#player\":\"kingdomjack\"}");
        Debug.Log("#TestEvent事件测试" + "{\"#player\":\"Kingdomjack\"}");
    }

    public void UpdateScore(LeaderBoard leaderBoardType)
    {
        tapLeaderBoard.UpdateScore(leaderBoardType);
    }

    public void GetLeaderBoard(LeaderBoard leaderBoardType)
    {
        tapLeaderBoard.GetLeaderBoard(leaderBoardType);
    }
}
