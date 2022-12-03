using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BattleRule : MonoBehaviour
{
    [SerializeField] UIRuleSet m_UIRuleSet = default;
    public List<Rule> SelectedRules;
    [SerializeField] private TextMeshProUGUI ruleTxt = default;
    public void OpenRuleSet()
    {
        m_UIRuleSet.Show();
        m_UIRuleSet.m_BattleRule = this;
    }

    public void SetRules(List<Rule> rules)
    {
        SelectedRules = rules;
        ruleTxt.text = "";
        foreach (var item in rules)
        {
            ruleTxt.text += item.Description + "\n";
        }

    }

    public void UpdateRules()
    {
        if (SelectedRules != null)
            RuleFactory.BattleRules = SelectedRules;
    }
}
