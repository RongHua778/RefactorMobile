using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class TurretSkillFactory
{
    public static Dictionary<int, Type> TurretSkillDIC;
    public static Dictionary<List<int>, Type> ElementSkillDIC;
    public static Dictionary<int, Type> BuildingSkillDIC;
    public static Dictionary<int, Type> GlobalSkillDIC;

    public static List<GlobalSkillInfo> GetGlobalSkills = new List<GlobalSkillInfo>();

    private static bool isInitialize => TurretSkillDIC != null;

    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(InitialSkill)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(InitialSkill)));
        TurretSkillDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as InitialSkill;
            TurretSkillDIC.Add((int)effect.EffectName, type);
        }

        InitialzieElementDIC();
        InitializeBuildingDIC();
        InitializeGlobalSkillDIC();
    }

    private static void InitializeGlobalSkillDIC()
    {
        var types = Assembly.GetAssembly(typeof(GlobalSkill)).GetTypes().
        Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(GlobalSkill)));
        GlobalSkillDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as GlobalSkill;
            GlobalSkillDIC.Add((int)effect.GlobalSkillName, type);
        }
    }



    private static void InitialzieElementDIC()
    {
        var types = Assembly.GetAssembly(typeof(ElementSkill)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ElementSkill)));
        ElementSkillDIC = new Dictionary<List<int>, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as ElementSkill;
            if (!ElementSkillDIC.ContainsKey(effect.InitElements))
            {
                ElementSkillDIC.Add(effect.InitElements, type);
            }
            else
            {
                Debug.LogWarning("重复的元素搭配：" + effect.InitElements[0].ToString() + effect.InitElements[1].ToString() + effect.InitElements[2].ToString());
            }
        }
    }

    private static void InitializeBuildingDIC()
    {
        var types = Assembly.GetAssembly(typeof(BuildingSkill)).GetTypes().
       Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BuildingSkill)));
        BuildingSkillDIC = new Dictionary<int, Type>();
        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as BuildingSkill;
            BuildingSkillDIC.Add((int)effect.BuildingSkillName, type);
        }
    }

    public static InitialSkill GetInitialSkill(int id)
    {
        if (TurretSkillDIC.ContainsKey(id))
        {
            Type type = TurretSkillDIC[id];
            InitialSkill effect = Activator.CreateInstance(type) as InitialSkill;
            return effect;
        }
        return null;
    }

    public static GlobalSkill GetGlobalSkill(GlobalSkillInfo info)
    {
        if (GlobalSkillDIC.ContainsKey((int)info.SkillName))
        {
            Type type = GlobalSkillDIC[(int)info.SkillName];
            GlobalSkill skill = Activator.CreateInstance(type) as GlobalSkill;
            skill.IsAbnormal = info.IsAbnormal;
            return skill;
        }
        Debug.LogWarning("不存在该全局技能");
        return null;
    }

    public static BuildingSkill GetBuidlingSkill(int id, bool isAbnormal)
    {
        if (BuildingSkillDIC.ContainsKey(id))
        {
            Type type = BuildingSkillDIC[id];
            BuildingSkill effect = Activator.CreateInstance(type) as BuildingSkill;
            effect.IsAbnormalBuilding = isAbnormal;
            return effect;
        }
        return null;
    }



    public static ElementSkill GetElementSkill(List<int> elements)
    {
        Type type;
        ElementSkill skillReturn;
        foreach (var skill in ElementSkillDIC.Keys)
        {
            List<int> temp = skill.ToList();
            foreach (var element in elements)
            {
                if (temp.Contains(element))
                {
                    temp.Remove(element);
                    if (temp.Count == 0)
                    {
                        type = ElementSkillDIC[skill];
                        skillReturn = Activator.CreateInstance(type) as ElementSkill;

                        skillReturn.Elements = skillReturn.InitElements;
                        //设置名称及描述
                        int offset = element / 10;
                        string key = "";
                        foreach (var e in skillReturn.Elements)
                        {
                            key += StaticData.ElementDIC[(ElementType)(e % 10)].GetElementName;
                        }
                        key += offset > 0 ? offset.ToString() : "";
                        skillReturn.SkillDescription = GameMultiLang.GetTraduction(key + "INFO");
                        skillReturn.SkillName = GameMultiLang.GetTraduction(key);

                        return skillReturn;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        Debug.LogWarning("没有这个元素技能" + elements[0].ToString() + elements[1].ToString() + elements[2].ToString());
        return null;
    }

    public static void AddGlobalSkill(GlobalSkillInfo skillInfo)
    {
        GetGlobalSkills.Add(skillInfo);
        foreach (var turret in GameManager.Instance.refactorTurrets.behaviors)
        {
            GlobalSkill skill = GetGlobalSkill(skillInfo);
            StrategyBase strategy = ((TurretContent)turret).Strategy;
            strategy.AddGlobalSkill(skill);
        }

        foreach (var grid in BluePrintShopUI.ShopBluePrints)
        {
            GlobalSkill skill = GetGlobalSkill(skillInfo);
            StrategyBase strategy = grid.Strategy;
            strategy.AddGlobalSkill(skill);
        }
    }

    public static void AddGlobalSkillToStrategy(StrategyBase strategy)
    {
        foreach (var info in GetGlobalSkills)
        {
            GlobalSkill skill = GetGlobalSkill(info);
            strategy.AddGlobalSkill(skill);
        }
    }
    public static void Release()
    {
        GetGlobalSkills.Clear();
    }

}
