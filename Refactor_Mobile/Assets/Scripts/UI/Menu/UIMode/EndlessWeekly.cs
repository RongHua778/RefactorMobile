using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessWeekly : MonoBehaviour
{
    [SerializeField] BattleRecipe m_BattleRecipe = default;
    [SerializeField] BattleRule m_BattleRule = default;

    [SerializeField] GameObject reconnectArea = default;
    [SerializeField] GameObject mainArea = default;


    [SerializeField] Text connectTips = default;
    [SerializeField] GameObject reConnectBtn = default;

    [SerializeField] WeeklyParam_Endless EndlessParamData = default;

    [SerializeField] Text playerScoreTxt = default;


    private bool isReconnecting;


    public void Initialize()
    {
        m_BattleRecipe.Initialize();
        SetArea(false);
        GameEvents.Instance.onEndlessLeaderBoardGet += GetLeaderboardCallback;

    }

    //private void SetArea(bool value)
    //{
    //    reconnectArea.SetActive(!value);
    //    mainArea.SetActive(value);
    //    if (value)
    //    {
    //        List<Rule> rules = new List<Rule>();
    //        foreach (var ruleName in EndlessParamData.EndlessParams[PlayfabManager.Instance.OnlineEndlessVersion % 10].RuleNames)
    //        {
    //            Rule rule = RuleFactory.GetRule((int)ruleName);
    //            rules.Add(rule);
    //        }
    //        m_BattleRule.SetRules(rules);
    //        m_BattleRecipe.SetRecipes(EndlessParamData.EndlessParams[PlayfabManager.Instance.OnlineEndlessVersion % 10].Recipes);
    //    }
    //    playerScoreTxt.text= GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + PlayfabManager.Instance.EndlessWave + GameMultiLang.GetTraduction("WAVE");
    //}

    private void SetArea(bool value)
    {
        reconnectArea.SetActive(!value);
        mainArea.SetActive(value);
        if (value)
        {
            List<Rule> rules = new List<Rule>();
            foreach (var ruleName in EndlessParamData.EndlessParams[LevelManager.Instance.LocalEndlessVersion % 10].RuleNames)
            {
                Rule rule = RuleFactory.GetRule((int)ruleName);
                rules.Add(rule);
            }
            m_BattleRule.SetRules(rules);
            m_BattleRecipe.SetRecipes(EndlessParamData.EndlessParams[LevelManager.Instance.LocalEndlessVersion % 10].Recipes);
        }
        playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + LevelManager.Instance.LocalEndlessWave + GameMultiLang.GetTraduction("WAVE");
    }

    private void GetLeaderboardCallback()
    {
        SetArea(true);
    }

    public void ReconnectBtnClick()
    {
        if (!isReconnecting)
            ReconnectCor();
    }

    private async void ReconnectCor()
    {
        isReconnecting = true;
        connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS");
        reConnectBtn.SetActive(false);
        await TaptapManager.Instance.TapLogin();
        //yield return new WaitForSeconds(5f);
        SetArea(TaptapManager.Instance.LoginSuccessful);
        connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS2");
        reConnectBtn.SetActive(true);
        isReconnecting = false;
    }

    public void EndlessModeStart()
    {
        m_BattleRecipe.UpdateRecipes();
        m_BattleRule.UpdateRules();
        LevelManager.Instance.StartNewGame(12);//11简单无尽，12困难无尽
    }


    private void OnDestroy()
    {
        GameEvents.Instance.onEndlessLeaderBoardGet -= GetLeaderboardCallback;

    }
}
