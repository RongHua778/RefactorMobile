using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using Steamworks;
using UnityEngine.UI;

[System.Serializable]
public enum LeaderBoard
{
    Endless,
    Challenge
}

public class UIBillBoard : IUserInterface
{
    Animator anim;
    [SerializeField] BillboardItem billboardItemPrefab = default;
    [SerializeField] Transform todayParent = default;
    [SerializeField] Transform yesterdayParent = default;

    List<BillboardItem> m_Items = new List<BillboardItem>();

    [SerializeField] GameObject refreashBtn = default;
    [SerializeField] bool refreashingLeaderboard = false;
    //[SerializeField] TextMeshProUGUI playerScoreTxt = default;
    [SerializeField] Text lastTxt = default;
    [SerializeField] Text newTxt = default;
    [SerializeField] TextMeshProUGUI refreshTipsTxt = default;
    [SerializeField] TextMeshProUGUI titleTxt = default;


    private LeaderBoard LeaderBoardType;
    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }

    public void RefreashBtnClick()
    {
        if (!refreashingLeaderboard)
            StartCoroutine(RefreashCor());
    }

    //private IEnumerator RefreashCor()
    //{
    //    refreashBtn.SetActive(false);
    //    refreashingLeaderboard = true;
    //    PlayfabManager.Instance.UpdateLoacalScore();
    //    yield return new WaitForSeconds(2f);
    //    switch (LeaderBoardType)
    //    {
    //        case LeaderBoard.Endless:
    //            PlayfabManager.Instance.GetEndlessVersion();
    //            break;
    //        case LeaderBoard.Challenge:
    //            PlayfabManager.Instance.GetChallengeVersion();
    //            break;
    //    }
    //    yield return new WaitForSeconds(2f);
    //    refreashingLeaderboard = false;
    //    SetLeaderBoard();
    //    refreashBtn.SetActive(true);
    //}
    private IEnumerator RefreashCor()
    {
        refreashBtn.SetActive(false);
        refreashingLeaderboard = true;
        TaptapManager.Instance.UpdateScore(LeaderBoardType);
        yield return new WaitForSeconds(2f);
        TaptapManager.Instance.GetLeaderBoard(LeaderBoardType);
        yield return new WaitForSeconds(2f);
        refreashingLeaderboard = false;
        SetLeaderBoard();
        refreashBtn.SetActive(true);
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
        refreashBtn.SetActive(true);
    }

    private void SetUIInfo()
    {
        switch (LeaderBoardType)
        {
            case LeaderBoard.Endless:
                lastTxt.text = GameMultiLang.GetTraduction("LASTWEEK");
                newTxt.text = GameMultiLang.GetTraduction("THISWEEK");
                refreshTipsTxt.text = GameMultiLang.GetTraduction("RESETTIME1");
                titleTxt.text = GameMultiLang.GetTraduction("ENDLESSBILLBOARD");
                break;
            case LeaderBoard.Challenge:
                lastTxt.text = GameMultiLang.GetTraduction("YESTERDAY");
                newTxt.text = GameMultiLang.GetTraduction("TODAY");
                refreshTipsTxt.text = GameMultiLang.GetTraduction("RESETTIME2");
                titleTxt.text = GameMultiLang.GetTraduction("CHALLENGEBILLBOARD");
                break;
        }
    }

    public void ShowLeaderBoard(LeaderBoard leaderBoardType)
    {
        this.LeaderBoardType = leaderBoardType;
        SetLeaderBoard();
        SetUIInfo();
        Show();
    }

    public override void ClosePanel()
    {
        anim.SetBool("isOpen", false);
    }

    //public void SetLeaderBoard()
    //{
    //    ClearBillBoard();
    //    switch (LeaderBoardType)
    //    {
    //        case LeaderBoard.Endless:
    //            if (PlayfabManager.Instance.EndlessResult[0].LeaderBoardResult != null)
    //                foreach (var item in PlayfabManager.Instance.EndlessResult[0].LeaderBoardResult.Leaderboard)
    //                {
    //                    BillboardItem billBoardItem = Instantiate(billboardItemPrefab, todayParent);
    //                    billBoardItem.SetContent(item.Position + 1, item.DisplayName, item.StatValue,true);
    //                    m_Items.Add(billBoardItem);
    //                }
    //            if (PlayfabManager.Instance.EndlessResult[1].LeaderBoardResult != null)
    //                foreach (var item in PlayfabManager.Instance.EndlessResult[1].LeaderBoardResult.Leaderboard)
    //                {
    //                    BillboardItem billBoardItem = Instantiate(billboardItemPrefab, yesterdayParent);
    //                    billBoardItem.SetContent(item.Position + 1, item.DisplayName, item.StatValue, true);
    //                    m_Items.Add(billBoardItem);
    //                }
    //            break;

    //        case LeaderBoard.Challenge:
    //            if (PlayfabManager.Instance.ChallengeResults[0].LeaderBoardResult != null)
    //                foreach (var item in PlayfabManager.Instance.ChallengeResults[0].LeaderBoardResult.Leaderboard)
    //                {
    //                    BillboardItem billBoardItem = Instantiate(billboardItemPrefab, todayParent);
    //                    billBoardItem.SetContent(item.Position + 1, item.DisplayName, item.StatValue, false);
    //                    m_Items.Add(billBoardItem);
    //                }
    //            if (PlayfabManager.Instance.ChallengeResults[1].LeaderBoardResult != null)
    //                foreach (var item in PlayfabManager.Instance.ChallengeResults[1].LeaderBoardResult.Leaderboard)
    //                {
    //                    BillboardItem billBoardItem = Instantiate(billboardItemPrefab, yesterdayParent);
    //                    billBoardItem.SetContent(item.Position + 1, item.DisplayName, item.StatValue, false);
    //                    m_Items.Add(billBoardItem);
    //                }
    //            break;
    //    }
    //    //playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + score + GameMultiLang.GetTraduction("WAVE");

    //}

    public void SetLeaderBoard()
    {
        ClearBillBoard();
        switch (LeaderBoardType)
        {
            case LeaderBoard.Endless:
                if (TaptapManager.Instance.CurrentEndlessRankings != null)
                    foreach (var item in TaptapManager.Instance.CurrentEndlessRankings)
                    {
                        BillboardItem billBoardItem = Instantiate(billboardItemPrefab, todayParent);
                        billBoardItem.SetContent(item.Rank + 1, (string)item.User["nickname"], (int)item.Value, true);
                        m_Items.Add(billBoardItem);
                    }
                if (TaptapManager.Instance.LastEndlessRankings != null)
                    foreach (var item in TaptapManager.Instance.LastEndlessRankings)
                    {
                        BillboardItem billBoardItem = Instantiate(billboardItemPrefab, yesterdayParent);
                        billBoardItem.SetContent(item.Rank + 1, (string)item.User["nickname"], (int)item.Value, true);
                        m_Items.Add(billBoardItem);
                    }
                break;

            case LeaderBoard.Challenge:
                if (TaptapManager.Instance.CurrentChallengeRankings != null)
                    foreach (var item in TaptapManager.Instance.CurrentChallengeRankings)
                    {
                        BillboardItem billBoardItem = Instantiate(billboardItemPrefab, todayParent);
                        billBoardItem.SetContent(item.Rank + 1, (string)item.User["nickname"], (int)item.Value, true);
                        m_Items.Add(billBoardItem);
                    }
                if (TaptapManager.Instance.LastChallengeRankings != null)
                    foreach (var item in TaptapManager.Instance.LastChallengeRankings)
                    {
                        BillboardItem billBoardItem = Instantiate(billboardItemPrefab, yesterdayParent);
                        billBoardItem.SetContent(item.Rank + 1, (string)item.User["nickname"], (int)item.Value, true);
                        m_Items.Add(billBoardItem);
                    }
                break;
        }
        //playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + score + GameMultiLang.GetTraduction("WAVE");

    }

    private void ClearBillBoard()
    {
        foreach (var item in m_Items)
        {
            Destroy(item.gameObject);
        }
        m_Items.Clear();
    }
}
