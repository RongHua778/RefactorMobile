using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TechSelectUI : IUserInterface
{
    [SerializeField] TechSelectPanel[] techSelectPanels = default;
    [SerializeField] GameObject selectParent = default;
    [SerializeField] TextMeshProUGUI foldBtnText = default;

    [SerializeField] TextMeshProUGUI refreashBtnTxt = default;
    [SerializeField] GameObject refreashBtn = default;
    [SerializeField] TextMeshProUGUI skipBtnTxt = default;
    [SerializeField] GameObject skipBtn = default;


    [SerializeField] TextMeshProUGUI selectInfoTxt = default;

    bool isFold = false;
    public override void Initialize()
    {
        base.Initialize();
        foldBtnText.text = GameMultiLang.GetTraduction("FOLD");
    }

    public void GetRandomTechs()
    {
        selectInfoTxt.text = GameMultiLang.GetTraduction("SELECTTECH");
        refreashBtn.SetActive(true);
        skipBtn.SetActive(false);
        if (TechnologySystem.PickingTechs.Count <= 0)//首次
        {
            TechnologySystem.PickingTechs = TechnologyFactory.GetRandomTechs(3);
            for (int i = 0; i < TechnologySystem.PickingTechs.Count; i++)
            {
                TechnologySystem.PickingTechs[i].CanAbnormal = UnityEngine.Random.value < GameRes.AbnormalRate;
                techSelectPanels[i].SetTechInfo(TechnologySystem.PickingTechs[i]);
            }

        }
        else//读取存档
        {
            for (int i = 0; i < TechnologySystem.PickingTechs.Count; i++)
            {
                techSelectPanels[i].SetTechInfo(TechnologySystem.PickingTechs[i]);
            }
        }
        refreashBtnTxt.text = GameMultiLang.GetTraduction("REFREASH") + ":" + GameRes.FreeRefreshTech;

    }

    public void LoadSaveGame()
    {
        if (TechnologySystem.PickingTechs.Count > 0)
        {
            GetRandomTechs();
            Show();
        }

    }


    public override void Show()
    {
        base.Show();
        isFold = false;
        selectParent.SetActive(!isFold);
    }

    public void FoldBtnClick()
    {
        selectParent.SetActive(isFold);
        foldBtnText.text = isFold ? GameMultiLang.GetTraduction("FOLD") : GameMultiLang.GetTraduction("UNFOLD");
        isFold = !isFold;
        if (!isFold)
        {
            for (int i = 0; i < techSelectPanels.Length; i++)
            {
                techSelectPanels[i].UpdateInfo();
            }
        }
    }

    public void RefreashTechBtnClick()
    {
        if (GameRes.FreeRefreshTech > 0)
        {
            GameRes.FreeRefreshTech--;
            TechnologySystem.PickingTechs.Clear();
            GetRandomTechs();
        }

    }

    public void SkipChoices()
    {
        GameRes.SkipTimes++;
        Hide();
        GameManager.Instance.GainMoney(100);
        GameManager.Instance.ConfirmChoice();
    }
    public override void Release()
    {
        base.Release();
        TechSelectPanel.SelectingTech = null;
    }

    //挑战模式
    public void GetCurrentChoices(ChallengeChoice challengeChoice)
    {
        GameRes.ChallengeChoicePicked = false;
        selectInfoTxt.text = GameMultiLang.GetTraduction("SELECTCHOICE");//+": "+ choiceRemain;
        refreashBtn.SetActive(false);
        skipBtn.SetActive(true);
        skipBtnTxt.text = GameMultiLang.GetTraduction("SKIP") + "(<sprite=7>+100)";

        for (int i = 0; i < techSelectPanels.Length; i++)
        {
            switch (challengeChoice.Choices[i].ChoiceType)
            {
                case ChallengeChoiceType.Turret:
                    RefactorStrategy strategy = ConstructHelper.GetSpecificStrategyByString(
                        challengeChoice.Choices[i].Value1, challengeChoice.Choices[i].Elements, new List<int> { 1, 1, 1 });
                    strategy.AddElementSkill(TurretSkillFactory.GetElementSkill(challengeChoice.Choices[i].Elements));
                    techSelectPanels[i].SetTurretInfo(strategy);
                    break;
                case ChallengeChoiceType.Trap:
                    TrapAttribute att = StaticData.Instance.ContentFactory.GetTrapAtt(challengeChoice.Choices[i].Value1);
                    techSelectPanels[i].SetTrapInfo(att);
                    break;
                case ChallengeChoiceType.Technology:
                    Technology technology = TechnologyFactory.GetTech((int)(Enum.Parse(typeof(TechnologyName), challengeChoice.Choices[i].Value1)));
                    technology.IsAbnormal = false;
                    technology.CanAbnormal = challengeChoice.Choices[i].Elements[0] == 1;
                    techSelectPanels[i].SetTechInfo(technology);
                    break;
            }

        }
    }


}
