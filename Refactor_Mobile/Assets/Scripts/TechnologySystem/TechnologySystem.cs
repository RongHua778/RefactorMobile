using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologySystem : IGameSystem
{
    public static List<Technology> GetTechnologies;
    public static List<Technology> GetGlobalSkillTechs;
    public static List<Technology> PickingTechs;
    [SerializeField] TechListPanel m_TechPanel=default;

    public override void Initialize()
    {
        base.Initialize();
        GetTechnologies = new List<Technology>();
        GetGlobalSkillTechs = new List<Technology>();
        PickingTechs = new List<Technology>();
    }

    public void AddTech(Technology tech)
    {
        GetTechnologies.Add(tech);
        m_TechPanel.AddTech(tech);
        tech.OnGet();
        TechnologyFactory.BattleTechs.Remove(tech);
    }



    public void TechBtnClick()
    {
        if (GetTechnologies.Count <= 0)
        {
           TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTECH"));
            return;
        }
        if(!m_TechPanel.IsVisible())
            m_TechPanel.Show();
        else
            m_TechPanel.Hide();

    }

    public void OnTurnEnd()
    {
        foreach (var tech in GetTechnologies)
        {
            tech.OnTurnEnd();
        }
    }

    public void OnTurnStart()
    {
        foreach (var tech in GetTechnologies)
        {
            tech.OnTurnStart();
        }
    }

    public static void OnRefactor(StrategyBase strategy)
    {
        foreach (var tech in GetTechnologies)
        {
            tech.OnRefactor(strategy);
        }
    }

    public static void OnEquip(StrategyBase strategy)
    {
        foreach (var tech in GetTechnologies)
        {
            tech.OnEquip(strategy);
        }
    }

    public void ConfirmTechSelect()
    {
        PickingTechs.Clear();

        TechSelectPanel.SelectingTech = null;
        GameRes.RefreashTechCost = 100;
        GameRes.FreeRefreshTech = 1;
    }

    public void LoadSaveGame()
    {
        foreach (var tech in LevelManager.Instance.LastGameSave.SaveTechnologies)
        {
            Technology technology = TechnologyFactory.GetTech(tech.TechName);
            technology.IsAbnormal = tech.IsAbnormal;
            technology.CanAbnormal = tech.CanAbnormal;
            technology.SaveValue = tech.TechSaveValue;
            AddTech(technology);

        }
        if (LevelManager.Instance.LastGameSave.SavePickingTechs != null)//读取正在选择的科技
        {
            foreach (var tech in LevelManager.Instance.LastGameSave.SavePickingTechs)
            {
                Technology technology = TechnologyFactory.GetTech(tech.TechName);
                technology.IsAbnormal = tech.IsAbnormal;
                technology.SaveValue = tech.TechSaveValue;
                technology.CanAbnormal = tech.CanAbnormal;
                PickingTechs.Add(technology);
            }
        }
    }

    public void RemoveTech(Technology tech)
    {
        GetTechnologies.Remove(tech);
        m_TechPanel.RemoveTech(tech);
        TechnologyFactory.BattleTechs.Add(tech);
    }




}
