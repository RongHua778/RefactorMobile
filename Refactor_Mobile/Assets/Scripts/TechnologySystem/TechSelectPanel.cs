using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TechSelectPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI selectNameTxt = default;
    [SerializeField] Image techIcon = default;
    [SerializeField] TextMeshProUGUI desTxt = default;
    [SerializeField] private Animator m_Anim = default;
    [SerializeField] TextMeshProUGUI warningTxt = default;
    [SerializeField] GameObject WarningBtn = default;
    [SerializeField] UI_ChoiceElements choiceElement = default;

    private Technology m_Tech;
    private TechAttribute m_TechAtt;

    private ChallengeChoiceType choiceType;
    private RefactorStrategy refactorStrategy;
    private TrapAttribute trapAtt;

    public static bool PlacingChoice;
    public static Technology SelectingTech;
    public void SetTechInfo(Technology tech)
    {
        choiceType = ChallengeChoiceType.Technology;
        choiceElement.gameObject.SetActive(false);
        m_Tech = tech;
        m_TechAtt = StaticData.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
        WarningBtn.SetActive(m_Tech.CanAbnormal);
        m_Tech.IsAbnormal = false;
        UpdateInfo();
    }

    public void SetTurretInfo(RefactorStrategy strategy)
    {
        choiceElement.gameObject.SetActive(true);
        choiceType = ChallengeChoiceType.Turret;
        this.refactorStrategy = strategy;
        selectNameTxt.text = GameMultiLang.GetTraduction(strategy.Attribute.Name);
        ElementSkill m_Skill = (ElementSkill)strategy.TurretSkills[1];
        desTxt.text = string.Format(m_Skill.SkillDescription,
            "<b>" + m_Skill.DisplayValue + "</b>", "<b>" + m_Skill.DisplayValue2 + "</b>", "<b>" + m_Skill.DisplayValue3 + "</b>"
            , "<b>" + m_Skill.DisplayValue4 + "</b>", "<b>" + m_Skill.DisplayValue5 + "</b>");
        techIcon.sprite = strategy.Attribute.Icon;
        choiceElement.SetElements(m_Skill);

        WarningBtn.SetActive(false);
        m_Anim.Play("TechSelect_Default", 0, 0);
        m_Anim.SetBool("Abnormal", false);
    }

    public void SetTrapInfo(TrapAttribute att)
    {
        choiceElement.gameObject.SetActive(false);

        choiceType = ChallengeChoiceType.Trap;
        this.trapAtt = att;
        selectNameTxt.text = GameMultiLang.GetTraduction(att.Name);
        desTxt.text = GameMultiLang.GetTraduction(att.Name + "INFO");
        techIcon.sprite = att.Icon;

        WarningBtn.SetActive(false);
        m_Anim.Play("TechSelect_Default", 0, 0);
        m_Anim.SetBool("Abnormal", false);
    }


    public void OnSelected()
    {
        switch (choiceType)
        {
            case ChallengeChoiceType.Technology:
                SelectingTech = m_Tech;
                GameManager.Instance.GetTech(m_Tech);
                if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge)
                {
                    bool needPlacing= m_Tech.OnGet2();
                    GameManager.Instance.ShowChoices(false, needPlacing);
                    PlacingChoice = needPlacing;
                }
                else
                    GameManager.Instance.ShowTechSelect(false, m_Tech.OnGet2());
                break;
            case ChallengeChoiceType.Turret:
                PlacingChoice = true;
                ConstructHelper.GetRefactorTurretByStrategy(refactorStrategy);
                GameManager.Instance.ShowChoices(false, true);
                break;
            case ChallengeChoiceType.Trap:
                PlacingChoice = true;
                ConstructHelper.GetTrapShapeByName(trapAtt.Name);
                GameManager.Instance.ShowChoices(false, true);
                break;
        }

    }

    public void OnBuildingWithdraw()
    {

    }

    public void ChangeAbnormalMode()
    {
        m_Tech.IsAbnormal = !m_Tech.IsAbnormal;
        if (m_Tech.IsAbnormal)
            Sound.Instance.PlayUISound("Sound_Error");
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (choiceType == ChallengeChoiceType.Technology)
        {
            selectNameTxt.text = GameMultiLang.GetTraduction(m_Tech.TechName);
            desTxt.text = string.Format(m_Tech.TechnologyDes,
                "<b>" + m_Tech.DisplayValue1 + "</b>", "<b>" + m_Tech.DisplayValue2 + "</b>", "<b>" + m_Tech.DisplayValue3 + "</b>",
                "<b>" + m_Tech.DisplayValue4 + "</b>", "<b>" + m_Tech.DisplayValue5 + "</b>");
            techIcon.sprite = m_TechAtt.Icon;
            warningTxt.gameObject.SetActive(m_Tech.IsAbnormal);
            m_Anim.Play("TechSelect_Default", 0, 0);
            m_Anim.SetBool("Abnormal", m_Tech.IsAbnormal);
        }
    }
}
