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

public class TaptapManager : MySingleton<TaptapManager>
{
    public bool LoginSuccessful { get; set; }
    public TDSUser CurentUser { get; set; }
    [SerializeField] private TapLeaderBoard tapLeaderBoard = default;

    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> CurrentEndlessRankings => tapLeaderBoard.CurrentEndlessRankings;
    public System.Collections.ObjectModel.ReadOnlyCollection<LCRanking> LastEndlessRankings => tapLeaderBoard.LastEndlessRankings;
    // Start is called before the first frame update
    void Start()
    {
        InitTaptap();
        InitAntiAddiction();
        //TapLogin();
    }

    private static void StartAntiAddiction()
    {
        string userIdentifier = "RefactorMobile";
        AntiAddictionUIKit.Startup(userIdentifier);

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
                .ConfigBuilder();
        TapBootstrap.Init(config);
    }

    public async void TapLogin()
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
                tapLeaderBoard.GetLeaderBoard();
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
            LoginSuccessful = true;
            tapLeaderBoard.GetLeaderBoard();
            // 进入游戏
        }


    }

    public void TapLogout()
    {
        TDSUser.Logout();
        AntiAddictionUIKit.Exit();
        TipsManager.Instance.ShowMessage("成功登出");
        Debug.Log("成功登出");
    }



    public void UpdateScore(LeaderBoard leaderBoardType)
    {
        tapLeaderBoard.UpdateScore(leaderBoardType);
    }

    public void GetLeaderBoard()
    {
        tapLeaderBoard.GetLeaderBoard();
    }
}
