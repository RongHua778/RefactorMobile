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
                .ConfigBuilder();
        TapBootstrap.Init(config);
    }

    public async void TapLogin()
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
                tapLeaderBoard.GetLeaderBoard();
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
            LoginSuccessful = true;
            tapLeaderBoard.GetLeaderBoard();
            // ������Ϸ
        }


    }

    public void TapLogout()
    {
        TDSUser.Logout();
        AntiAddictionUIKit.Exit();
        TipsManager.Instance.ShowMessage("�ɹ��ǳ�");
        Debug.Log("�ɹ��ǳ�");
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
