﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Pathfinding;


public class StaticData : Singleton<StaticData>
{

    [Header("静态数据")]
    public static Vector2Int BoardOffset;
    public static LayerMask PathLayer = 1 << 6 | 1 << 10;
    public static string TrapMask = "Trap";
    public static string ConcreteTileMask = "ConcreteTile";
    public static string GroundTileMask = "GroundTile";
    public static string TempTileMask = "TempTile";
    public static string TempTurretMask = "TempTurret";
    public static string TurretMask = "Turret";
    public static string TempGroundMask = "TempGround";

    public static string Untagged = "Untagged";
    public static string OnlyRefactorTag = "OnlyCompositeTurret";
    public static string UndropablePoint = "UnDropablePoint";
    public static LayerMask EnemyLayerMask = 1 << 11;
    public static LayerMask TurretLayerMask = 1 << 13;

    public static LayerMask GetGroundLayer = 1 << 8 | 1 << 12;
    public static LayerMask GetSelectLayer = 1 << 6 | 1 << 8 | 1 << 10;

    public static Vector2 LeftTipsPos;
    public static Vector2 LeftMidTipsPos;
    public static Vector2 RightTipsPos;
    public static Vector2 RightMidTipsPos;
    public static Vector2 MidTipsPos;
    public static Vector2 MidUpPos;

    public static bool LockKeyboard = false;

    //抽取品质概率
    public static float[,] QualityChances = new float[6, 5]
    {
        { 1f, 0f, 0f, 0f, 0f },
        { 0.6f, 0.3f, 0.1f, 0f, 0f },//可以选择3级塔跳科技，3级科技开始需要3级塔
        { 0.4f, 0.3f, 0.25f, 0.05f, 0f },//可以选择4级塔跳科技，4级科技开始需要4级塔
        { 0.3f, 0.3f, 0.25f, 0.15f, 0f },//无法选择5级塔，5级科技开始是分水岭，4级科技是巩固战力期
        { 0.2f, 0.25f, 0.25f, 0.25f, 0.05f },//5级科技开始需要5级塔，但概率较低，最少升6
        { 0.15f, 0.25f, 0.25f, 0.25f, 0.1f },//6级科技不需要1级塔
    };
    public static float[,] RareChances = new float[6, 6]
    {
        { 0.1f, 0f, 0f, 0f, 0f,0f },
        { 0.1f, 0.1f, 0f, 0f, 0f ,0f},
        { 0.1f, 0.1f, 0.1f, 0f, 0f,0f },
        { 0.1f, 0.1f, 0.1f, 0.1f, 0f,0f },
        { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f,0f },
        { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f,0.1f },
    };
    //元素加成配置
    public static float[] ElementBenefit = new float[4]
    {
        1f, 2f, 2.5f, 3f
    };


    [Header("工厂类")]
    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    [SerializeField] TurretStrategyFactory _strategyFactory = default;
    [SerializeField] NonEnemyFactory _nonEnemyFactory = default;

    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }
    public TurretStrategyFactory StrategyFactory => _strategyFactory;
    public NonEnemyFactory NonEnemyFactory => _nonEnemyFactory;

    [Header("战斗基础数值")]
    public int SystemMaxLevel;
    public int[] LevelUpMoney;
    public int StartCoin;
    public int BaseWaveIncome;
    public int WaveMultiplyIncome;
    public int ShopRefreshCost;
    public int BaseShapeCost;
    public int MultipleShapeCost;
    public int BuyGroundCost;
    public int BuyGroundCostMultyply;
    public int SwitchTrapCost;
    public int SwitchTurretCost;
    public int SwitchTrapCostMultiply;


    [Header("场景数值设置")]
    public static int GroundSize = 25;
    public static float EnvrionmentBaseVolume = .5f;
    public float TileSize = default;
    public static int maxLevel = 5;    //塔的最大等级
    public static int elementN = 5;//一共有几种战斗元素
    public static int maxQuality = 5;//最大quality
    public static float DefaultCritDmg = 2.5f;//暴击伤害率
    public static int BonusTrapMaxCoinPerTurn = 100;//铸币陷阱回合上限
    public static float EarlySkillPercent = 0.25f;
    public static int MaxEnemyAmount = 250;

    private static bool showDamage;
    public static bool ShowDamage
    {
        get => showDamage;
        set
        {
            showDamage = value;
            PlayerPrefs.SetInt("UI_ShowDamage", showDamage ? 0 : 1);
        }
    }
    private static bool showIntensify;
    public static bool ShowIntensify
    {
        get => showIntensify;
        set
        {
            showIntensify = value;
            PlayerPrefs.SetInt("UI_ShowIntensify", showIntensify ? 0 : 1);
            GameEvents.Instance.ShowDamageIntensify(showIntensify);
        }

    }

    [Header("元素加成")]
    public float GoldAttackIntensify = 0.3f;
    public float WoodFirerateIntensify = 0.3f;
    public float WaterSlowIntensify = 0.5f;
    public float FireCritIntensify = 0.2f;
    public float DustSplashIntensify = 0.25f;
    public static Dictionary<ElementType, Element> ElementDIC;

    [Header("Prefabs")]
    public RangeHolder RangeHolder;
    public RangeIndicator RangeIndicatorPrefab;
    public JumpDamage JumpDamagePrefab;
    public Sprite UnrevealTrap;
    public ParticalControl LandedEffect;
    public ReusableObject GainMoneyPrefab;
    public ReusableObject FrostExplosion;
    public ReusableObject FrostEffectPrefab;
    public ReusableObject GainPerfectPrefab;
    public ReusableObject BlinkHolePrefab;
    public Sprite[] ElementSprites;

    public Color NormalBlue;
    public Color HighlightBlue;

    [Header("CompositionAttributes")]
    public int[,] LevelUpCostPerRare = new int[6, 2]
    {
        { 75, 150},
        { 125, 250},
        { 200, 400},
        { 300, 600},
        { 400, 800},
        { 500, 1000}
    };


    public void Initialize()
    {
        InitializeData();
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        EnemyFactory.InitializeFactory();
        SetTipsPos();
    }

    public static void SetTipsPos()
    {
        MidUpPos = new Vector2(Screen.width / 2, Screen.height - 100f);
        MidTipsPos = new Vector2(Screen.width / 2, Screen.height / 2);
        LeftTipsPos = new Vector2(Screen.width / 5, Screen.height / 2);
        LeftMidTipsPos = new Vector2(Screen.width / 2.5f, Screen.height / 2);
        RightTipsPos = new Vector2(Screen.width - Screen.width / 5, Screen.height / 2);
        RightMidTipsPos = new Vector2(Screen.width - Screen.width / 2.5f, Screen.height / 2);
    }

    private void InitializeData()
    {
        //元素字典
        ElementDIC = new Dictionary<ElementType, Element>();
        ElementDIC.Add(ElementType.None, new None());
        ElementDIC.Add(ElementType.GOLD, new Gold());
        ElementDIC.Add(ElementType.WOOD, new Wood());
        ElementDIC.Add(ElementType.WATER, new Water());
        ElementDIC.Add(ElementType.FIRE, new Fire());
        ElementDIC.Add(ElementType.DUST, new Dust());

        showDamage = PlayerPrefs.GetInt("UI_ShowDamage", 0) == 0 ? true : false;
        showIntensify = PlayerPrefs.GetInt("UI_ShowIntensify", 1) == 0 ? true : false;
    }

    //随机打乱一个int list的方法
    public static List<T> RandomSort<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }

    public static int RandomNumber(float[] pros)
    {
        float total = 0f;
        foreach (float elem in pros)
        {
            total += elem;
        }
        float randomPoint = UnityEngine.Random.value * total;
        for (int i = 0; i < pros.Length; i++)
        {
            if (randomPoint < pros[i])
            {
                return i;
            }
            else
            {
                randomPoint -= pros[i];
            }
        }
        return pros.Length - 1;

    }

    public static int[] GetRandomSequence(int total, int count)
    {
        int[] sequence = new int[total];
        int[] output = new int[count];

        for (int i = 0; i < total; i++)
        {
            sequence[i] = i;
        }
        int end = total - 1;
        for (int i = 0; i < count; i++)
        {
            int num = UnityEngine.Random.Range(0, end + 1);
            output[i] = sequence[num];
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }

    public static List<Vector2Int> GetCirclePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -(range - Mathf.Abs(x)); y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2Int pos = new Vector2Int(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = -(forbidRange - Mathf.Abs(x)); y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2Int pos = new Vector2Int(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2Int> GetHalfCirclePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = 0; y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2Int pos = new Vector2Int(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = 0; y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2Int pos = new Vector2Int(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2Int> GetLinePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int i = 1; i <= range; i++)
        {
            Vector2Int pos = new Vector2Int(0, i);
            pointsToRetrun.Add(pos);
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int i = 1; i <= forbidRange; i++)
            {
                Vector2Int pos = new Vector2Int(0, i);
                pointsToExcept.Add(pos);
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }
    //给定一个总等级，返回若干个随机数的方法
    public static int[] GetSomeRandoms(int levelMin, int levelMax, int totalLevel, int number)
    {
        if ((totalLevel / number) > levelMax)
        {
            Debug.LogWarning("配方等级输错了，菜鸡");
            int[] errorRandom = new int[number];
            for (int i = 0; i < number; i++)
            {
                errorRandom[i] = levelMax;
            }
            return errorRandom;
        }
        //if (number < 1)
        //{
        //    number = 1;
        //}
        //if (totalLevel < 1)
        //{
        //    totalLevel = 1;
        //}
        //if (number > totalLevel)
        //{
        //    totalLevel = number;
        //}
        int[] result = new int[number];
        while (number > 1)
        {
            if (number == 2)
            {
                int min = levelMin;
                while (totalLevel - min > levelMax)
                {
                    min++;
                }
                result[0] = UnityEngine.Random.Range(min, totalLevel - min + 1);
                result[1] = totalLevel - result[0];
                number--;
            }
            else if (number >= 2)
            {
                int max = Mathf.Min(totalLevel - (number - 1), levelMax);
                int min = levelMin;
                while (totalLevel - min > levelMax * (number - 1))
                {
                    min++;
                }
                int a = UnityEngine.Random.Range(min, max + 1);
                totalLevel -= a;
                result[number - 1] = a;
                number--;
            }
            else
            {
                Debug.LogWarning("刷随机等级的算法不支持！");
                return null;
            }
        }
        return result;
    }


    //total是总量，number是想要几个随机数
    public static List<int> SelectNoRepeat(int total, int number)
    {
        List<int> data = new List<int>();
        for (int i = 0; i < total; i++)
        {
            data.Add(i);
        }
        if (data.Count < number)
        {
            return data;
        }
        else
        {
            List<int> result = new List<int>();
            for (int i = 0; i < number; i++)
            {
                int index = UnityEngine.Random.Range(0, data.Count);
                result.Add(data[index]);
                data.RemoveAt(index);
            }
            return result;
        }
    }

    public static Collider2D RaycastCollider(Vector2 pos, LayerMask layer)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, layer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    //public static string GetElementIntensifyText(ElementType element,int amount)//根据元素及品质设置显示加成效果
    //{
    //    return ElementDIC[element].GetIntensifyText(amount);
    //}

    public static string SetElementSkillInfo(ElementSkill skill)
    {
        string intensifyTxt = GameMultiLang.GetTraduction("ELEMENTSKILL") + ":";//根据元素及品质设置显示加成效果
        foreach (var element in skill.InitElements)
        {
            intensifyTxt += ElementDIC[(ElementType)element].GetSkillText;
        }
        return intensifyTxt;
    }

    public static string GetTurretDes(TurretSkill skill)
    {
        string finalDes = "";
        finalDes = string.Format(GameMultiLang.GetTraduction(skill.strategy.Attribute.Name + "SKILL"),
            "<b>" + skill.DisplayValue + "</b>", "<b>" + skill.DisplayValue2 + "</b>", "<b>" + skill.DisplayValue3 + "</b>"
            , "<b>" + skill.DisplayValue4 + "</b>", "<b>" + skill.DisplayValue5 + "</b>")
            + ElementDIC[skill.IntensifyElement].GetExtraInfo;
        return finalDes;
    }

    public static string GetBuildingDes(BuildingSkill skill)
    {
        string finalDes = "";
        finalDes = skill.IsAbnormalBuilding ?
        ElementDIC[ElementType.GOLD].Colorized("<sprite=8>" + GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO")) + "\n"
        + ElementDIC[ElementType.FIRE].Colorized("<sprite=9>" + GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO2")) :
        GameMultiLang.GetTraduction(skill.BuildingSkillName.ToString() + "INFO");
        finalDes = string.Format(finalDes,
            "<b>" + skill.DisplayValue + "</b>", "<b>" + skill.DisplayValue2 + "</b>", "<b>" + skill.DisplayValue3 + "</b>",
             "<b>" + skill.DisplayValue4 + "</b>", "<b>" + skill.DisplayValue5 + "</b>");
        return finalDes;
    }

    public static string GetLevelInfo(int level)
    {
        float[] levelChances = new float[5];
        for (int i = 0; i < 5; i++)
        {
            levelChances[i] = QualityChances[level - 1, i];
        }
        string text = "";
        text += GameMultiLang.GetTraduction("MODULELEVELINFO1") + ":\n";
        for (int x = 0; x < 5; x++)
        {
            text += GameMultiLang.GetTraduction("MODULELEVELINFO2") + (x + 1).ToString() + ": " + levelChances[x] * 100 + "%\n";
        }
        text += GameMultiLang.GetTraduction("MODULELEVELINFO3");
        return text;
    }

    public static string GetEnergyInfo()
    {
        string text = GameMultiLang.GetTraduction("ENERGYINFO");
        return text;
    }

    public static void CorrectTileCoord(TileBase tile)
    {
        Vector2Int coord = new Vector2Int(Convert.ToInt32(tile.transform.position.x), Convert.ToInt32(tile.transform.position.y));
        int newX = coord.x + BoardOffset.x;
        int newY = coord.y + BoardOffset.y;
        tile.OffsetCoord = new Vector2Int(newX, newY);
    }

    public static void SetNodeWalkable(TileBase tile, bool walkable, bool changeAble = true)
    {
        var grid = AstarPath.active.data.gridGraph;
        int p = tile.OffsetCoord.x;
        int q = tile.OffsetCoord.y;

        GridNodeBase node = grid.nodes[q * grid.width + p];

        node.Walkable = walkable;
        node.ChangeAbleNode = changeAble;
        grid.CalculateConnectionsForCellAndNeighbours(p, q);
    }

    public static bool GetNodeWalkable(TileBase tile)
    {
        var grid = AstarPath.active.data.gridGraph;
        int p = tile.OffsetCoord.x;
        int q = tile.OffsetCoord.y;

        GridNodeBase node = grid.nodes[q * grid.width + p];

        return node.Walkable;
    }

    public void ShowJumpDamage(Vector2 pos, long amount, bool isCritical)
    {
        if (!ShowDamage)
            return;
        JumpDamage obj = ObjectPool.Instance.Spawn(JumpDamagePrefab) as JumpDamage;
        obj.Jump(amount, pos, isCritical);
    }



    public static void C(List<int> lsArray, int selectCount)
    {
        int totolcount = lsArray.Count;
        int[] currectselect = new int[selectCount];
        int last = selectCount - 1;

        for (int i = 0; i < selectCount; i++)
        {
            currectselect[i] = i;
        }

        while (true)
        {
            for (int i = 0; i < selectCount; i++)
            {
                Debug.Log(lsArray[currectselect[i]]);
            }
            Debug.Log("");
            if (currectselect[last] < totolcount - 1)
            {
                currectselect[last]++;
            }
            else
            {
                int pos = last;
                while (pos > 0 && currectselect[pos - 1] == currectselect[pos] - 1)
                {
                    pos--;
                }
                if (pos == 0) return;
                currectselect[pos - 1]++;
                for (int i = pos; i < selectCount; i++)
                {
                    currectselect[i] = currectselect[i - 1] + 1;
                }
            }
        }
    }

    public static List<List<int>> GetAllC(List<int> dataList, int n, List<int> value = null)
    {
        List<List<int>> result = new List<List<int>>();
        for (int i = 0, count = dataList.Count; i < count; i++)
        {
            List<int> itemList = new List<int>();
            if (value != null && value.Count > 0)
            {
                itemList.AddRange(value);
            }
            itemList.Add(dataList[i]);

            if (itemList.Count == n)
            {
                result.Add(itemList);
            }
            else
            {
                result.AddRange(GetAllC(dataList, n, itemList));
            }
        }
        return result;
    }

    public static List<List<int>> GetAllCC(List<int> dataList)
    {
        List<List<int>> result = new List<List<int>>();
        for (int a = 0; a < dataList.Count; a++)
        {
            for (int b = a; b < dataList.Count; b++)
            {
                for (int c = b; c < dataList.Count; c++)
                {
                    List<int> itemList = new List<int>();
                    itemList.Add(dataList[a]);
                    itemList.Add(dataList[b]);
                    itemList.Add(dataList[c]);
                    result.Add(itemList);
                }
            }
        }
        return result;
    }

    public static List<List<int>> GetAllCC2(List<int> dataList)
    {
        List<List<int>> result = new List<List<int>>();
        for (int a = 0; a < dataList.Count; a++)//同名先加
        {
            List<int> itemList = new List<int>();
            itemList.Add(dataList[a]);
            itemList.Add(dataList[a]);
            itemList.Add(dataList[a]);
            result.Add(itemList);
        }
        for (int b = 0; b < dataList.Count; b++)//双同名
        {
            for (int c = 0; c < dataList.Count; c++)
            {
                if (c == b)
                    continue;
                List<int> itemList = new List<int>();
                itemList.Add(dataList[b]);
                itemList.Add(dataList[b]);
                itemList.Add(dataList[c]);
                result.Add(itemList);
            }
        }
        for (int a = 0; a < dataList.Count; a++)//混合
        {
            for (int b = a + 1; b < dataList.Count; b++)
            {
                for (int c = b + 1; c < dataList.Count; c++)
                {
                    List<int> itemList = new List<int>();
                    itemList.Add(dataList[a]);
                    itemList.Add(dataList[b]);
                    itemList.Add(dataList[c]);
                    result.Add(itemList);
                }
            }
        }

        return result;
    }

    public GameObject GainMoneyEffect(Vector2 pos, int amount)
    {
        GameManager.Instance.GainMoney(amount);
        GameObject obj = ObjectPool.Instance.Spawn(GainMoneyPrefab).gameObject;
        obj.transform.position = pos + Vector2.up * 0.2f;
        obj.transform.localScale = Vector3.one;
        Sound.Instance.PlayEffect("Sound_GainCoin");
        GameRes.GainGoldBattleTurn += amount;
        return obj;
    }

    public void GainPerfectEffect(Vector2 pos, int amount)
    {
        GameManager.Instance.GainPerfectElement(amount);
        GameObject obj = ObjectPool.Instance.Spawn(GainPerfectPrefab).gameObject;
        obj.transform.position = pos + Vector2.up * 0.2f;
        //Sound.Instance.PlayEffect("Sound_GainCoin");
    }

    public void FrostTurretEffect(Vector2 pos, float distance, float frostTime)
    {
        ReusableObject partical = ObjectPool.Instance.Spawn(FrostExplosion);
        partical.transform.position = pos;
        partical.transform.localScale = Vector3.one * distance;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, distance, LayerMask.GetMask(TurretMask));
        foreach (var col in cols)
        {
            ConcreteContent turret = col.GetComponent<ConcreteContent>();
            FrostEffect frosteffect = FrostEffect(col.transform.position);
            turret.Frost(frostTime, frosteffect);
        }
        Sound.Instance.PlayEffect("Sound_EnemyExplosionFrost");

    }

    public FrostEffect FrostEffect(Vector2 pos)
    {
        FrostEffect effect = ObjectPool.Instance.Spawn(FrostEffectPrefab) as FrostEffect;
        effect.transform.position = pos;
        Sound.Instance.PlayEffect("Sound_EnemyExplosionFrost");
        return effect;
    }

    public static string FormElementName(ElementType element, int quality)
    {
        string texttoreturn = "";
        texttoreturn += ElementDIC[element].GetElementName;
        texttoreturn += quality;
        return texttoreturn;
    }

    public static string GetElementName(ElementType element)
    {
        return ElementDIC[element].GetElementName;
    }
}
