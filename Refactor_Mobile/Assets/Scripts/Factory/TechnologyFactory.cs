using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class TechnologyFactory
{
    public static Dictionary<int, Technology> TechDIC;
    public static List<Technology> BattleTechs;
    private static bool isInitialize => TechDIC != null;
    public static void Initialize()
    {
        if (isInitialize)
            return;
        var types = Assembly.GetAssembly(typeof(Technology)).GetTypes().
            Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Technology)));
        TechDIC = new Dictionary<int, Technology>();

        foreach (var type in types)
        {
            var effect = Activator.CreateInstance(type) as Technology;
            effect.InitializeTech();
            TechDIC.Add((int)effect.TechnologyName, effect);
        }

    }

    public static void SetBattleTechs()
    {
        BattleTechs = new List<Technology>();
        foreach (var tech in TechDIC.Values)
        {
            //加入所有非配方科技
            if (tech.RefactorBinding == RefactorTurretName.None)
                BattleTechs.Add(tech);
        }

        ResetAllTech();
    }

    public static void SetRecipeTechs()
    {
        //根据战斗配方加入特定科技
        foreach (var recipe in StaticData.Instance.ContentFactory.BattleRecipes)
        {
            foreach (var tech in TechDIC.Values)
            {
                if (recipe.RefactorName == tech.RefactorBinding)
                {
                    BattleTechs.Add(tech);
                    break;
                }
            }
        }
    }






    public static void ResetAllTech()
    {
        foreach (var item in TechDIC.Values)
        {
            //重置所有科技状态
            item.IsAbnormal = false;
            item.SaveValue = 0;
        }
    }

    public static Technology GetTech(int techName)
    {
        if (TechDIC.ContainsKey(techName))
        {
            return TechDIC[techName];
        }
        Debug.LogWarning("没有对应的科技"+techName);
        return null;

    }


    public static Technology GetBattleTech(int techName)
    {
        foreach (var tech in BattleTechs)
        {
            if ((int)tech.TechnologyName == techName)
            {
                return tech;
            }
        }
        Debug.LogWarning("没有对应的科技" + (TechnologyName)techName);
        return null;
    }

    public static List<Technology> GetRandomTechs(int count)
    {
        List<int> indexs = StaticData.SelectNoRepeat(BattleTechs.Count, count);
        List<Technology> returnTechs = new List<Technology>();
        foreach (var index in indexs)
        {
            returnTechs.Add(BattleTechs[index]);
        }
        return returnTechs;
    }



}
