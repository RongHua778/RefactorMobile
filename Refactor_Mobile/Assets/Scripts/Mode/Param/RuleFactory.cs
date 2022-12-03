using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class RuleFactory
{
    public static Dictionary<int, Rule> RuleDIC;
    public static List<Rule> BattleRules;

    private static bool isInitialize => RuleDIC != null;
    public static void Initialize()
    {
        if (isInitialize)
            return;
        BattleRules = new List<Rule>();
        var types = Assembly.GetAssembly(typeof(Rule)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Rule)));
        RuleDIC = new Dictionary<int, Rule>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as Rule;
            RuleDIC.Add((int)effect.RuleName, effect);
        }

    }
    public static Rule GetRule(int ruleName)
    {
        if (RuleDIC.ContainsKey(ruleName))
        {
            return RuleDIC[ruleName];
        }
        Debug.LogWarning("没有对应的规则");
        return null;
    }

    public static void TriggerRules()
    {
        foreach (var rule in BattleRules)
        {
            rule.OnGameInit();
        }
    }

    public static void LoadSaveRules()
    {
        BattleRules.Clear();
        foreach (var rule in LevelManager.Instance.LastGameSave.SaveRules)
        {
            BattleRules.Add(GetRule(rule));
        }
        TriggerRules();
    }

    public static void Release()
    {
        BattleRules.Clear();
    }


}
