using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIRuleSet : IUserInterface
{
    [SerializeField] RuleGrid ruleGridPrefab = default;
    [SerializeField] Transform contentParent = default;
    public BattleRule m_BattleRule { get; set; }
    private Animator m_Anim;


    private List<RuleGrid> m_RuleGrids = new List<RuleGrid>();
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        SetRules();
    }

    private void SetRules()
    {
        foreach (var rule in RuleFactory.RuleDIC.Values)
        {
            RuleGrid ruleGrid = Instantiate(ruleGridPrefab, contentParent);
            ruleGrid.SetRuleContent(rule);
            m_RuleGrids.Add(ruleGrid);
        }
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("isOpen", true);
    }

    public override void ClosePanel()
    {
        m_Anim.SetBool("isOpen", false);
        SaveSetting();
    }

    private void SaveSetting()
    {
        List<Rule> rules = new List<Rule>();
        foreach (var item in m_RuleGrids)
        {
            if (item.IsSelect)
                rules.Add(item.mRule);
        }
        m_BattleRule.SetRules(rules);
    }
}
