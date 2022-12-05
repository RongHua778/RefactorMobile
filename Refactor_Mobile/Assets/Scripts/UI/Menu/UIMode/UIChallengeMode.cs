using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChallengeMode : MonoBehaviour
{
    [SerializeField] DailyChallengeParam daylyChallengeParam = default;
    [SerializeField] GameObject mainArea = default;
    [SerializeField] GameObject reconnectArea = default;
    [SerializeField] Text playerScoreTxt = default;
    private bool isReconnecting;
    [SerializeField] Text connectTips = default;
    [SerializeField] GameObject reConnectBtn = default;
    [SerializeField] ToggleGroup bossToggleGroup = default;

    [SerializeField] ItemSlot[] bossItemSlots = default;

    [SerializeField] TextMeshProUGUI highScore_Txt = default;
    [SerializeField] GameObject challengeUnlockText = default;

    //[SerializeField] Image[] stars = default;
    //[SerializeField] Image waveProgress = default;

    private LevelAttribute levelAtt;
    

    public void Initialize()
    {
        if(LevelManager.Instance.PassDifficulty < 9)
        {
            mainArea.SetActive(false);
            reconnectArea.SetActive(false);
            challengeUnlockText.gameObject.SetActive(true);
        }
        else
        {
            challengeUnlockText.gameObject.SetActive(false);
            GameEvents.Instance.onChallengeLeaderBoardGet += GetLeaderboardCallback;
            SetArea(PlayfabManager.Instance.ChallengeLeaderboardGot);
        }
    }

    private void GetLeaderboardCallback(bool value)
    {
        SetArea(true);
    }

    private void SetArea(bool value)
    {
        reconnectArea.SetActive(!value);
        mainArea.SetActive(value);
        if (value)
        {
            levelAtt = daylyChallengeParam.ChallengeLevels[PlayfabManager.Instance.OnlineChallengeVersion % daylyChallengeParam.ChallengeLevels.Count];
            SetBossInfo();
            SetStarInfo();
        }
        playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + PlayfabManager.Instance.ChallngeScore;
    }


    private void SetBossInfo()
    {
        bossItemSlots[0].SetContent(levelAtt.Boss1[0], bossToggleGroup);
        bossItemSlots[1].SetContent(levelAtt.Boss2[0], bossToggleGroup);
        bossItemSlots[2].SetContent(levelAtt.Boss3[0], bossToggleGroup);
    }

    private void SetStarInfo()
    {
        int score = PlayfabManager.Instance.ChallngeScore;
        highScore_Txt.text = score.ToString();
        //foreach (var star in stars)
        //{
        //    star.gameObject.SetActive(false);
        //}
        //for(int i = 0; i < 3; i++)//4,8,12
        //{
        //    if (wave >= 4 * i)
        //    {
        //        stars[i].gameObject.SetActive(true);
        //    }
        //}
        //waveProgress.fillAmount = (float)wave / 12;
    }

    public void ReconnectBtnClick()
    {
        if (!isReconnecting)
            StartCoroutine(ReconnectCor());
    }

    IEnumerator ReconnectCor()
    {
        isReconnecting = true;
        connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS");
        Game.Instance.InitializeNetworks();
        reConnectBtn.SetActive(false);
        yield return new WaitForSeconds(5f);
        SetArea(PlayfabManager.Instance.ChallengeLeaderboardGot);
        connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS2");
        reConnectBtn.SetActive(true);
        isReconnecting = false;
    }

    public void ChallengeModeStart()
    {
        RuleFactory.Release();//标准模式无特殊规则
        LevelManager.Instance.StartNewGame(levelAtt.ModeID);
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onChallengeLeaderBoardGet -= GetLeaderboardCallback;
    }
}
