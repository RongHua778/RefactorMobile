using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UITujian : IUserInterface
{
    [SerializeField] GameLevelHolder gameLevelPrefab = default;
    [SerializeField] UITujian_ListHolder[] listHolders = default;

    [SerializeField] TipsElementConstruct[] elementConstructs = default;
    List<int> skillPreviewElements;
    ToggleGroup m_ToggleGroup;

    public override void Initialize()
    {
        base.Initialize();
        m_ToggleGroup = this.GetComponent<ToggleGroup>();

        foreach (var item in listHolders)
        {
            item.SetContent(m_ToggleGroup);
        }

        skillPreviewElements = new List<int> { 0, 1, 2, 3, 4 };
        SetElementSkills();
    }

    public void SetElementSkills()
    {
        List<List<int>> skills = StaticData.GetAllCC2(skillPreviewElements);
        for (int i = 0; i < skills.Count; i++)
        {
            ElementSkill skill = TurretSkillFactory.GetElementSkill(skills[i]);
            TurretAttribute attribute = StaticData.Instance.ContentFactory.GetElementAttribute(ElementType.GOLD);
            skill.strategy = new StrategyBase(attribute, 1);

            elementConstructs[i].SetElements(skill);
        }
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("OpenLevel", true);
        gameLevelPrefab.SetData();
    }

    public override void Hide()
    {
        base.Hide();
        TipsManager.Instance.HideTips();
    }

    public override void ClosePanel()
    {
        anim.SetBool("OpenLevel", false);
        MenuManager.Instance.ShowMenu();
    }
}
