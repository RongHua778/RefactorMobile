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
        //        Debug.Log("Taptap ����ɹ�");
        //    }
        //    else
        //    {
        //        Debug.Log("Taptap ����ʧ��");
        //    }
        //});
        bool isSuccess = await TapCommon.UpdateGameAndFailToWebInTapTap("228703");
    }

    private static void InitAntiAddiction()
    {
        AntiAddictionConfig config = new AntiAddictionConfig()
        {
            gameId = "gjbbmb3iasltjf9taz",      // TapTap ���������Ķ�Ӧ Client ID
            useTapLogin = true,             // �Ƿ����� TapTap ������֤
            showSwitchAccount = false,      // �Ƿ���ʾ�л��˺Ű�ť
        };

        Action<int, string> callback = (code, errorMsg) =>
        {
            //code == 500;   //��¼�ɹ�
            //code = 1000;   //�û��ǳ�
            //code = 1001;   //�л��˺�
            //code = 1030;   //�û���ǰ�޷�������Ϸ
            //code = 1050;   //ʱ������
            //code = 9002;   //ʵ�������е���˹ر�ʵ����
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
            Debug.Log("��ǰδ��¼");
            // ��ʼ��¼
            try
            {
                // �� iOS��Android ϵͳ�»ỽ�� TapTap �ͻ��˻��� WebView ��ʽ���е�¼
                // �� Windows��macOS ϵͳ����ʾ��ά�루Ĭ�ϣ�����ת���ӣ������ã�
                CurentUser = await TDSUser.LoginWithTapTap();
                // ��ȡ TDSUser ����
                var objectId = CurentUser.ObjectId;     // �û�Ψһ��ʶ
                var nickname = CurentUser["nickname"];  // �ǳ�
                var avatar = CurentUser["avatar"];      // ͷ��
                TipsManager.Instance.ShowMessage("��¼�ɹ�:" + nickname);
                LoginSuccessful = true;
                Debug.Log("��¼�ɹ�:" + nickname);
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
                    if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // ȡ����¼
                    {
                        Debug.Log("��¼ȡ��");
                    }
                }
            }
        }
        else
        {
            Debug.Log("�ѵ�¼");
            TapDB.SetUser(CurentUser.ObjectId);
            LoginSuccessful = true;
            tapLeaderBoard.GetLeaderBoard(LeaderBoard.Endless);
            tapLeaderBoard.GetLeaderBoard(LeaderBoard.Challenge);
            // ������Ϸ
        }


    }
    //TapGameSave gameSave;
    public async Task UpLoadTapGameSave()
    {
        if (File.Exists(LevelManager.Instance.SaveGameFilePath))//���ڱ��ش浵
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
        //ȷ�����ص;���浵���Ḳ�Ǹ߾���浵
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
            //�뱾���ļ����бȽ�
            if (onlineLevel == LevelManager.Instance.GameLevel)//�ȼ���ͬʱ�ȽϾ���ֵ
            {
                if (onlineExp > LevelManager.Instance.GameExp)//���ϰ汾�ľ���ֵ���ߣ�ֻ�д���ʱ������
                {
                    await DownloadFileAsync(gameFileUrl);
                }
                else
                {
                    await SaveTapGame();//�������ϰ汾
                }
            }
            else if (onlineLevel > LevelManager.Instance.GameLevel)//���ϰ汾�ĵȼ�����
            {
                await DownloadFileAsync(gameFileUrl);//����
            }
            else//���°汾�ĵȼ��Ƚϸ�
            {
                await SaveTapGame();//�������ϰ汾
            }

        }
        else
        {
            await UpLoadTapGameSave();//û�д浵ʱ�ϴ��浵
            Debug.Log("�����ߴ浵");
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
    //        Debug.Log("���سɹ�");
    //    }
    //    else
    //    {
    //        Debug.Log("����ʧ��");
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
        TipsManager.Instance.ShowMessage("�ɹ��ǳ�");
        Debug.Log("�ɹ��ǳ�");
    }

    public void TrackEvents()
    {
        TapDB.TrackEvent("#TestEvent", "{\"#player\":\"kingdomjack\"}");
        Debug.Log("#TestEvent�¼�����" + "{\"#player\":\"Kingdomjack\"}");
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
