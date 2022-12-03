using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

public class MainUI : IUserInterface
{

    static Animator CoinAnim, WaveAnim, LifeAnim;

    //UI
    [SerializeField] Image GameSpeedImg = default;
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] WaveInfoSetter m_WaveInfoSetter = default;
    [SerializeField] Text waveTxt = default;
    [SerializeField] TechListPanel m_TechPanel = default;

    [SerializeField] InfoBtn m_RuleBtn = default;

    public int CurrentWave
    {
        set
        {
            string lang = PlayerPrefs.GetString("_language");
            switch (lang)
            {
                case "ch":
                    waveTxt.text = GameMultiLang.GetTraduction("NUM") + value + (LevelManager.Instance.CurrentLevel.ModeType != ModeType.Endless ? "/" + LevelManager.Instance.CurrentLevel.Wave : "") + GameMultiLang.GetTraduction("WAVE");
                    break;
                case "en":
                    waveTxt.text = GameMultiLang.GetTraduction("WAVE") + value + (LevelManager.Instance.CurrentLevel.ModeType != ModeType.Endless ? "/" + LevelManager.Instance.CurrentLevel.Wave : "");
                    break;
            }
        }
    }

    public int Coin
    {
        set => coinTxt.text = value.ToString();
    }

    public int Life
    {
        set => PlayerLifeTxt.text = value.ToString() + "/" + LevelManager.Instance.CurrentLevel.PlayerHealth.ToString();
    }



    [SerializeField] Sprite[] GameSpeedSprites = default;
    //”Œœ∑ÀŸ∂»
    public int GameSpeed
    {
        set
        {
            GameSpeedImg.sprite = GameSpeedSprites[value - 1];
        }
    }


    public override void Initialize()
    {
        base.Initialize();
        GameSpeed = 1;
        CoinAnim = m_RootUI.transform.Find("Coin").GetComponent<Animator>();
        LifeAnim = m_RootUI.transform.Find("Life").GetComponent<Animator>();
        WaveAnim = m_RootUI.transform.Find("Wave").GetComponent<Animator>();
        m_TechPanel.Initialize();
        SetRules();
    }



    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
    }


    public static void PlayMainUIAnim(int part, string key, bool value)
    {
        switch (part)
        {
            case 0://Coin
                CoinAnim.SetBool(key, value);
                break;
            case 1://Life
                LifeAnim.SetBool(key, value);
                break;
            case 2://Wave
                WaveAnim.SetBool(key, value);
                break;
        }
    }

    public override void Show()
    {
        PlayMainUIAnim(0, "Show", true);
        PlayMainUIAnim(1, "Show", true);
        PlayMainUIAnim(2, "Show", true);
    }

    public override void Hide()
    {
        PlayMainUIAnim(0, "Show", false);
        PlayMainUIAnim(1, "Show", false);
        PlayMainUIAnim(2, "Show", false);
    }


    public void PrepareNextWave(List<EnemySequence> sequences, EnemyType nextBoss, int nextBossWave)
    {
        m_WaveInfoSetter.SetWaveInfo(sequences, nextBoss, nextBossWave);
    }

    public bool ConsumeMoney(int cost)
    {
        if (GameRes.Coin >= cost)
        {
            GameRes.Coin -= cost;
            return true;
        }
        else
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMONEY"));
            return false;
        }
    }

    public void GuideBookBtnClick()
    {
        GuideGirlSystem.Instance.ShowGuideBook(0);
    }

    public void GameSpeedBtnClick()
    {
        GameRes.GameSpeed++;
    }

    public void SetRules()
    {
        if (RuleFactory.BattleRules.Count > 0)
        {
            string ruleStr = "";
            foreach (var rule in RuleFactory.BattleRules)
            {
                ruleStr += rule.Description + "\n";
            }
            m_RuleBtn.SetContent(ruleStr);
            m_RuleBtn.gameObject.SetActive(true);
        }
        else
        {
            m_RuleBtn.gameObject.SetActive(false);
        }
    }
}
