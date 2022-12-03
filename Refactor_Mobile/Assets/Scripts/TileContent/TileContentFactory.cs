using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class TileContentFactory : GameObjectFactory
{
    [SerializeField] TrapAttribute emptyAtt = default;
    [SerializeField] TrapAttribute spawnPointAtt = default;
    [SerializeField] TrapAttribute destinationAtt = default;

    [SerializeField] TurretAttribute[] allRefactorTurrets = default;
    [SerializeField] TurretAttribute[] allElementTurrets = default;
    [SerializeField] TrapAttribute[] allTraps = default;
    [SerializeField] TechAttribute[] allTechs = default;


    private Dictionary<ElementType, TurretAttribute> ElementDIC;
    public Dictionary<string, TurretAttribute> RefactorDIC;
    //private Dictionary<string, TurretAttribute> BuildingDIC;
    private Dictionary<string, TrapAttribute> TrapDIC;
    private Dictionary<TechnologyName, TechAttribute> TechDIC;

    public List<string> RefactorTurretNames;
    public List<string> TrapNames;


    //各等级配方
    public List<TurretAttribute> Rare1Att { get; private set; }
    public List<TurretAttribute> Rare2Att { get; private set; }
    public List<TurretAttribute> Rare3Att { get; private set; }
    public List<TurretAttribute> Rare4Att { get; private set; }
    public List<TurretAttribute> Rare5Att { get; private set; }
    public List<TurretAttribute> Rare6Att { get; private set; }

    [SerializeField] List<TurretAttribute> DefaultRecipes;
    public List<TrapAttribute> BattleTraps;
    public List<TurretAttribute> BattleRecipes;//保存在此，如果为空则自动生成
    //public List<TurretAttribute> BattleBuildings;

    //ContentAttribute[] returnAtts;
    public void Initialize()
    {
        ElementDIC = new Dictionary<ElementType, TurretAttribute>();
        TrapDIC = new Dictionary<string, TrapAttribute>();
        RefactorDIC = new Dictionary<string, TurretAttribute>();
        //BuildingDIC = new Dictionary<string, TurretAttribute>();
        TechDIC = new Dictionary<TechnologyName, TechAttribute>();

        Rare1Att = new List<TurretAttribute>();
        Rare2Att = new List<TurretAttribute>();
        Rare3Att = new List<TurretAttribute>();
        Rare4Att = new List<TurretAttribute>();
        Rare5Att = new List<TurretAttribute>();
        Rare6Att = new List<TurretAttribute>();

        BattleTraps = new List<TrapAttribute>();


        //BattleBuildings = new List<TurretAttribute>();

        //returnAtts = Resources.LoadAll<TurretAttribute>("SO/RefactorAttribute");
        foreach (TurretAttribute attribute in allRefactorTurrets)
        {
            RefactorDIC.Add(attribute.Name, attribute);
        }

        RefactorTurretNames = RefactorDIC.Keys.ToList();
        RefactorTurretNames.Remove("BOUNTY");//移除2个科技炮
        RefactorTurretNames.Remove("TELEPORTOR");
        RefactorTurretNames.Remove("PRISM");
        RefactorTurretNames.Remove("AMPLIFIER");



        //returnAtts = Resources.LoadAll<TrapAttribute>("SO/TrapAttribute");
        foreach (TrapAttribute attribute in allTraps)
        {
            if (!attribute.isLock)
            {
                BattleTraps.Add(attribute);
            }
            TrapDIC.Add(attribute.Name, attribute);
        }
        TrapNames = TrapDIC.Keys.ToList();
        TrapNames.Remove("BONUSTRAP");//随机挑战模式不包含铸币陷阱


        //returnAtts = Resources.LoadAll<TurretAttribute>("SO/ElementAttribute");
        foreach (TurretAttribute attribute in allElementTurrets)
        {
            ElementDIC.Add(attribute.element, attribute);
        }

        //returnAtts = Resources.LoadAll<TechAttribute>("SO/TechAttributes");
        foreach (TechAttribute attribute in allTechs)
        {
            TechDIC.Add(attribute.TechName, attribute);
        }

    }

    public void SetDefaultRecipes()
    {
        BattleRecipes = DefaultRecipes.ToList();
    }
    public void LoadRareList()
    {
        Rare1Att.Clear();
        Rare2Att.Clear();
        Rare3Att.Clear();
        Rare4Att.Clear();
        Rare5Att.Clear();
        Rare6Att.Clear();
        BattleRecipes.Clear();
        foreach (var item in LevelManager.Instance.LastGameSave.SaveBattleRecipes)
        {
            TurretAttribute att = GetRefactorByString(item);
            BattleRecipes.Add(att);
            switch (att.Rare)
            {
                case 1:
                    Rare1Att.Add(att);
                    break;
                case 2:
                    Rare2Att.Add(att);
                    break;
                case 3:
                    Rare3Att.Add(att);
                    break;
                case 4:
                    Rare4Att.Add(att);
                    break;
                case 5:
                    Rare5Att.Add(att);
                    break;
                case 6:
                    Rare6Att.Add(att);
                    break;
            }

        }
    }
    public void SetRareLists()
    {
        Rare1Att.Clear();
        Rare2Att.Clear();
        Rare3Att.Clear();
        Rare4Att.Clear();
        Rare5Att.Clear();
        Rare6Att.Clear();
        foreach (var attribute in BattleRecipes)
        {
            switch (attribute.Rare)
            {
                case 1:
                    Rare1Att.Add(attribute);
                    break;
                case 2:
                    Rare2Att.Add(attribute);
                    break;
                case 3:
                    Rare3Att.Add(attribute);
                    break;
                case 4:
                    Rare4Att.Add(attribute);
                    break;
                case 5:
                    Rare5Att.Add(attribute);
                    break;
                case 6:
                    Rare6Att.Add(attribute);
                    break;
            }
        }
    }


    //*******终点
    public GameTileContent GetDestinationPoint()
    {
        TrapContent content = Get(destinationAtt.Prefab) as TrapContent;
        content.Initialize(destinationAtt);
        return content;
    }
    //*******起点
    public GameTileContent GetSpawnPoint()
    {
        TrapContent content = Get(spawnPointAtt.Prefab) as TrapContent;
        content.Initialize(spawnPointAtt);
        return content;
    }

    public GameTileContent GetBasicContent(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyAtt.Prefab) as GameTileContent;
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }

    //元素塔*******************

    //public ElementTurret GetRandomElementTurret()
    //{
    //    float[] qualityC = new float[5];
    //    for (int i = 0; i < 5; i++)
    //    {
    //        qualityC[i] = StaticData.QualityChances[GameRes.SystemLevel - 1, i];
    //    }
    //    int quality = StaticData.RandomNumber(qualityC) + 1;
    //    TurretAttribute attribute = elementTurrets[UnityEngine.Random.Range(0, StaticData.elementN)];
    //    ElementTurret content = Get(attribute.Prefab) as ElementTurret;
    //    content.InitializeTurret(new StrategyBase(attribute, quality));
    //    return content;
    //}

    public ElementTurret GetElementTurret(ElementType element, int quality)
    {
        TurretAttribute attribute = ElementDIC[element];
        ElementTurret content = Get(attribute.Prefab) as ElementTurret;
        content.InitializeTurret(new StrategyBase(attribute, quality));

        return content;
    }

    public TurretAttribute GetElementAttribute(ElementType element)
    {
        TurretAttribute attribute = ElementDIC[element];
        return attribute;
    }

    public TurretAttribute GetRandomElementAttribute()
    {
        return ElementDIC[(ElementType)UnityEngine.Random.Range(0, ElementDIC.Count)];
    }
    //合成塔***********
    public RefactorTurret GetRefactorTurret(RefactorStrategy strategy)
    {
        RefactorTurret content = Get(strategy.Attribute.Prefab) as RefactorTurret;
        content.InitializeTurret(strategy);
        strategy.CompositeSkill();
        GameRes.TotalRefactor++;
        return content;
    }


    public TurretAttribute GetRefactorByString(string name)
    {
        if (RefactorDIC.ContainsKey(name))
        {
            return RefactorDIC[name];
        }
        Debug.LogWarning("没有对应名字的合成塔");
        return null;
    }

    public string GetRandomRefactorName()
    {
        return RefactorTurretNames[UnityEngine.Random.Range(0, RefactorTurretNames.Count)];
    }
    public string GetRandomTrapName()
    {
        return TrapNames[UnityEngine.Random.Range(0, TrapNames.Count)];
    }

    public TurretAttribute GetRandomCompositeAtt()
    {
        float[] rareChance = new float[6];
        for (int i = 0; i < 6; i++)
        {
            rareChance[i] = StaticData.RareChances[GameRes.SystemLevel - 1, i];
        }
        int rare = StaticData.RandomNumber(rareChance) + 1;
        TurretAttribute atrributeToReturn = null;
        switch (rare)
        {
            case 1:
                atrributeToReturn = Rare1Att[UnityEngine.Random.Range(0, Rare1Att.Count)];
                break;
            case 2:
                atrributeToReturn = Rare2Att[UnityEngine.Random.Range(0, Rare2Att.Count)];
                break;
            case 3:
                atrributeToReturn = Rare3Att[UnityEngine.Random.Range(0, Rare3Att.Count)];
                break;
            case 4:
                atrributeToReturn = Rare4Att[UnityEngine.Random.Range(0, Rare4Att.Count)];
                break;
            case 5:
                atrributeToReturn = Rare5Att[UnityEngine.Random.Range(0, Rare5Att.Count)];
                break;
            case 6:
                atrributeToReturn = Rare6Att[UnityEngine.Random.Range(0, Rare6Att.Count)];
                break;
        }
        Debug.Assert(atrributeToReturn != null, "没有可以返回的配方");
        return atrributeToReturn;
    }

    //陷阱*************

    public TrapContent GetTrapContentByName(string name, bool isReveal = false)
    {
        if (TrapDIC.ContainsKey(name))
        {
            TrapContent content = Get(TrapDIC[name].Prefab) as TrapContent;
            content.Initialize(TrapDIC[name]);
            if (isReveal) content.RevealTrap();
            return content;
        }
        Debug.LogWarning("没有对应名字的陷阱");
        return null;
    }


    public TrapContent GetRandomTrapContent()
    {
        TrapAttribute att = BattleTraps[UnityEngine.Random.Range(0, BattleTraps.Count)];
        TrapContent content = Get(att.Prefab) as TrapContent;
        content.Initialize(att);
        return content;
    }

    public TrapAttribute GetTrapAtt(string trapName)
    {
        if (TrapDIC.ContainsKey(trapName))
        {
            TrapAttribute att = TrapDIC[trapName];
            return att;
        }
        Debug.LogWarning("不包含该TRAP:" + trapName);
        return null;
    }


    private ReusableObject Get(ReusableObject prefab)
    {
        ReusableObject instance = CreateInstance(prefab);
        return instance;
    }

    //科技******
    public TechAttribute GetTechAtt(TechnologyName techName)
    {
        if (TechDIC.ContainsKey(techName))
        {
            return TechDIC[techName];
        }
        Debug.LogWarning("没有对应的科技ATT");
        return null;
    }


}
