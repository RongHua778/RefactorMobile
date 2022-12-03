using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RuleGrid : MonoBehaviour
{
    [SerializeField] Toggle m_Toggle = default;
    [SerializeField] Text m_RuleTxt = default;
    public bool IsSelect => m_Toggle.isOn;
    public Rule mRule { get; set; }
    public void SetRuleContent(Rule rule)
    {
        mRule = rule;
        m_RuleTxt.text = rule.Description;
        m_Toggle.isOn = false;
    }
}
