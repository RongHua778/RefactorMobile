using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class GameEndUI : IUserInterface
{
    [SerializeField] TurretBillboard m_BillBoard = default;
    [SerializeField] Image titleBG = default;
    [SerializeField] TextMeshProUGUI title = default;
    [SerializeField] Text passTimeTxt = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;
    [SerializeField] Text maxPathTxt = default;
    [SerializeField] Text maxMarkTxt = default;
    [SerializeField] Text gainGoldTxt = default;
    [SerializeField] Text expValueTxt = default;
    //[SerializeField] InfoBtn expInfoBtn = default;
    [SerializeField] GameLevelHolder gameLevelPrefab = default;

    [SerializeField] GameObject NextLevelBtn = default;
    [SerializeField] GameObject RestartBtn = default;

    [SerializeField] InfoBtn ChallengeInfo = default;

    [Header("标题底图")]
    [SerializeField] Sprite[] TitleBGs = default;

    int changeSpeed = 10;
    float waittime = 0.05f;
    float result = 0;
    Animator anim;
    int gainExp = 0;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }
    public override void Initialize()
    {
        base.Initialize();
        gainExp = 0;
        gameLevelPrefab.SetData();
    }

    public void SetGameResult(bool win)
    {
        LevelManager.Instance.LevelEnd = true;
        gainExp = LevelManager.Instance.GainExp(GameRes.CurrentWave);

        switch (LevelManager.Instance.CurrentLevel.ModeType)
        {
            case ModeType.Challenge:
                //int starCount = 0;

                //for (int i = 0; i < LevelManager.Instance.CurrentLevel.WaveRequired.Length; i++)
                //{
                //    if(GameRes.CurrentWave>= LevelManager.Instance.CurrentLevel.WaveRequired[i])
                //    {
                //        win = true;
                //        starCount = i;
                //    }
                //    else
                //    {
                //        win = false;
                //        break;
                //    }
                //}
                int score = 0;
                score += GameRes.CurrentWave * 100;
                score += GameRes.Life * 20;
                score += GameRes.SkipTimes * 20;
                titleBG.sprite = TitleBGs[win ? 5 : 0];
                title.text = GameMultiLang.GetTraduction("SCORE") + ":" + score;
                ChallengeInfo.gameObject.SetActive(true);
                ChallengeInfo.SetContent(GameMultiLang.GetTraduction("CHALLENGESCOREINFO"));
                //if (LevelManager.Instance.CurrentLevel.Level < LevelManager.Instance.ChallengeLevels.Length)
                //{
                //    NextLevelBtn.SetActive(win);
                //    RestartBtn.SetActive(true);
                //}
                //else
                //{
                //    NextLevelBtn.SetActive(false);
                //    RestartBtn.SetActive(true);
                //}
                NextLevelBtn.SetActive(false);
                RestartBtn.SetActive(true);
                //LevelManager.Instance.SetChallengeScore(LevelManager.Instance.CurrentLevel.Level,GameRes.CurrentWave);
                PlayfabManager.Instance.ChallngeScore = score;
                break;

            case ModeType.Endless:
                ChallengeInfo.gameObject.SetActive(false);

                title.gameObject.SetActive(true);

                title.text = GameMultiLang.GetTraduction("PASSLEVEL") + (GameRes.CurrentWave) + GameMultiLang.GetTraduction("WAVE");

                if (LevelManager.Instance.CurrentLevel.Level == 1)//每周无尽,更新分数
                    PlayfabManager.Instance.EndlessWave = GameRes.CurrentWave;

                int tempWordID = Mathf.Clamp(GameRes.CurrentWave / 20, 0, 5);
                titleBG.sprite = TitleBGs[tempWordID + 2];
                GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.EndlessEnd, tempWordID));

                NextLevelBtn.SetActive(false);
                RestartBtn.SetActive(true);

                LevelManager.Instance.LevelWin = GameRes.CurrentWave > 29;
                break;

            case ModeType.Standard:
                ChallengeInfo.gameObject.SetActive(false);

                title.gameObject.SetActive(true);

                LevelManager.Instance.LevelWin = win;
                if (win)
                {
                    title.text = GameMultiLang.GetTraduction("WIN") + GameMultiLang.GetTraduction("DIFFICULTY") + LevelManager.Instance.CurrentLevel.Level.ToString();
                    GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.StandardWin, LevelManager.Instance.CurrentLevel.Level));

                    if (LevelManager.Instance.PassDifficulty < LevelManager.Instance.CurrentLevel.Level + 1)
                        LevelManager.Instance.PassDifficulty = LevelManager.Instance.CurrentLevel.Level + 1;

                    titleBG.sprite = TitleBGs[LevelManager.Instance.CurrentLevel.Level + 1];
                    if (LevelManager.Instance.CurrentLevel.Level < LevelManager.Instance.StandardLevels.Length)//当前难度低于最大难度时，显示下一难度
                    {
                        NextLevelBtn.SetActive(true);
                        RestartBtn.SetActive(false);
                    }
                    else
                    {
                        NextLevelBtn.SetActive(false);
                        RestartBtn.SetActive(false);
                    }

                }
                else
                {
                    title.text = GameMultiLang.GetTraduction("LOSE");
                    GameEvents.Instance.TempWordTrigger(new TempWord(TempWordType.StandardLose, LevelManager.Instance.CurrentLevel.Level));

                    titleBG.sprite = TitleBGs[0];
                    NextLevelBtn.SetActive(false);
                    RestartBtn.SetActive(true);
                }
                break;

        }

        m_BillBoard.SetBillBoard();
        StartCoroutine(SetValueCor());
        SetAchievement();

    }

    private void SetAchievement()
    {
        if (LevelManager.Instance.LevelWin)
        {
            if (GameRes.MaxPath >= 150)
            {
                LevelManager.Instance.SetAchievement("ACH_LONGPATH");//遥不可及
            }
            if (GameRes.TotalRefactor >= 50)
            {
                LevelManager.Instance.SetAchievement("ACH_BILLIONS");//疯狂重构
            }
            if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Standard && LevelManager.Instance.CurrentLevel.Level == 6)
            {
                TimeSpan ts = DateTime.Now - GameRes.LevelStart;
                if (ts.Minutes <= 9)
                {
                    LevelManager.Instance.SetAchievement("ACH_FASTLEVEL6");//速通玩家
                }
            }

            if (((float)GameRes.GainGold / (float)GameRes.CurrentWave) > 400)
            {
                LevelManager.Instance.SetAchievement("ACH_MONEY");//极限操作
            }

            if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Standard)//标准模式成就
            {
                if (LevelManager.Instance.CurrentLevel.Level >= 2 && GameRes.Life == LevelManager.Instance.CurrentLevel.PlayerHealth)
                {
                    LevelManager.Instance.SetAchievement("ACH_EASY");//轻车熟路
                }
                if (LevelManager.Instance.CurrentLevel.Level >= 1 && GameRes.TotalRefactor == 0)
                {
                    LevelManager.Instance.SetAchievement("ACH_DECEIVE");//蒙混过关
                }
                if (GameRes.MaxPath <= 3)
                {
                    LevelManager.Instance.SetAchievement("ACH_EXTREME");//极限操作
                }

            }
        }
    }



    private void SetExp()
    {
        expValueTxt.text = GameMultiLang.GetTraduction("EXPVALUE") + ":" + gainExp;
        gameLevelPrefab.AddExp(gainExp);
    }

    IEnumerator SetValueCor()
    {
        passTimeTxt.text = "";
        totalCompositeTxt.text = "";
        totalDamageTxt.text = "";
        maxPathTxt.text = "";
        maxMarkTxt.text = "";
        gainGoldTxt.text = "";
        expValueTxt.text = "";

        float delta;   //delta为速度，每次加的数大小
        yield return new WaitForSeconds(waittime);
        passTimeTxt.text = (GameRes.LevelStart - DateTime.Now).ToString(@"hh\:mm\:ss");

        delta = (float)GameRes.TotalRefactor / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            totalCompositeTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        totalCompositeTxt.text = GameRes.TotalRefactor.ToString();


        delta = (float)GameRes.TotalDamage / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            totalDamageTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        totalDamageTxt.text = GameRes.TotalDamage.ToString();

        delta = (float)GameRes.MaxPath / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            maxPathTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        maxPathTxt.text = GameRes.MaxPath.ToString();

        delta = (float)GameRes.MaxMark / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            maxMarkTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        maxMarkTxt.text = GameRes.MaxMark.ToString();

        delta = (float)GameRes.GainGold / changeSpeed;
        result = 0;
        for (int i = 0; i < changeSpeed; i++)
        {
            result += delta;
            gainGoldTxt.text = Mathf.RoundToInt(result).ToString();
            yield return new WaitForSeconds(waittime);
        }
        gainGoldTxt.text = GameRes.GainGold.ToString();

        SetExp();
        StopCoroutine(SetValueCor());
    }

    public void NextLevelBtnClick()
    {
        //if (LevelManager.Instance.CurrentLevel.Level + 1 > LevelManager.Instance.PermitDifficulty)
        //{
        //    TempWord tempWord = new TempWord(TempWordType.Demo, 0);
        //    GameEvents.Instance.TempWordTrigger(tempWord);
        //    return;
        //}
        LevelManager.Instance.StartNewGame(LevelManager.Instance.CurrentLevel.ModeID + 1);
    }


    public override void Show()
    {
        base.Show();
        anim.SetBool("Show", true);
    }




}
