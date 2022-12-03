using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructHelper
{
    static TileFactory m_TileFactory;
    static TileShapeFactory m_ShapeFactory;
    static TileContentFactory m_ContentFactory;
    static TurretStrategyFactory m_StrategyFactory;


    public static void Initialize()
    {
        m_TileFactory = StaticData.Instance.TileFactory;
        m_ShapeFactory = StaticData.Instance.ShapeFactory;
        m_ContentFactory = StaticData.Instance.ContentFactory;
        m_StrategyFactory = StaticData.Instance.StrategyFactory;
    }


    //public static TileShape GetRandomShapeByLevel()
    //{
    //    TileShape shape = m_ShapeFactory.GetRandomShape();
    //    GameTile specialTile = m_TileFactory.GetBasicTile();
    //    GameTileContent content = m_ContentFactory.GetRandomElementTurret();
    //    specialTile.SetContent(content);
    //    shape.SetTile(specialTile);
    //    return shape;
    //}


    public static TileShape GetRandomShape()
    {
        ShapeType[] shapTypes = Enum.GetValues(typeof(ShapeType)) as ShapeType[];
        float[] qualityC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            qualityC[i] = StaticData.QualityChances[GameRes.SystemLevel - 1, i];
        }
        ShapeInfo shapeInfo = new ShapeInfo();
        shapeInfo.ShapeType = (int)shapTypes[UnityEngine.Random.Range(0, shapTypes.Length - 1)];
        shapeInfo.Element = UnityEngine.Random.Range(0, 5);
        shapeInfo.Quality = StaticData.RandomNumber(qualityC) + 1;
        shapeInfo.TurretPos = UnityEngine.Random.Range(0, 3);
        shapeInfo.TurretDir = UnityEngine.Random.Range(0, 3);

        //ShapeInfo shapeInfo = new ShapeInfo((int)shapTypes[UnityEngine.Random.Range(0, shapTypes.Length - 1)], UnityEngine.Random.Range(0, 5),//shapetype-1,最后一个是Dshape
        //    StaticData.RandomNumber(qualityC) + 1, UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3));

        TileShape shape = m_ShapeFactory.GetShape(shapeInfo);
        GameTile tile = GetElementTurret((ElementType)shapeInfo.Element, shapeInfo.Quality);
        shape.SetTile(tile, shapeInfo.TurretPos, shapeInfo.TurretDir);
        return shape;
    }

    public static GameTile GetNormalTile(GameTileContentType type)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetBasicContent(type);
        tile.SetContent(content);
        return tile;
    }

    public static GroundTile GetGroundTile()
    {
        GroundTile tile = m_TileFactory.GetGroundTile();
        return tile;
    }

    //元素塔
    public static TurretAttribute GetElementAttribute(ElementType element)
    {
        return m_ContentFactory.GetElementAttribute(element);
    }


    //陷阱
    public static GameTile GetRandomTrap()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomTrapContent();
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetSpawnPoint()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetSpawnPoint();
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetDestinationPoint()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetDestinationPoint();
        tile.SetContent(content);
        return tile;
    }

    //合成塔
    //new
    public static RefactorStrategy GetRandomRefactorStrategy()
    {
        TurretAttribute attribute = m_ContentFactory.GetRandomCompositeAtt();
        RefactorStrategy strategy = m_StrategyFactory.GetRandomRefactorStrategy(attribute);

        strategy.AddElementSkill(GetElementSkillBaseOnComposition(strategy.Compositions));
        TurretSkillFactory.AddGlobalSkillToStrategy(strategy);

        return strategy;
    }

    //基于配方组合获取元素技能
    public static ElementSkill GetElementSkillBaseOnComposition(List<Composition> compositions)
    {
        List<int> elements = new List<int>();
        foreach (var item in compositions)
        {
            elements.Add(item.elementRequirement);
        }
        ElementSkill skill = TurretSkillFactory.GetElementSkill(elements);
        return skill;
    }


    public static TileShape GetRefactorTurretByStrategy(RefactorStrategy strategy)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        RefactorTurret content = m_ContentFactory.GetRefactorTurret(strategy);

        //if (GameRes.NextCompositeCallback != null)//下一次合成获得额外加成
        //{
        //    GameRes.NextCompositeCallback(bluePrint.ComStrategy);
        //    GameRes.NextCompositeCallback = null;
        //}
        //strategy.CompositeSkill();

        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }

    //建筑
    //public static BuildingStrategy GetRandomBuildingStrategy()
    //{
    //    TurretAttribute attribute = m_ContentFactory.GetRandomBuildingAtt();
    //    BuildingStrategy strategy = m_StrategyFactory.GetBuidlingStrategy(attribute, false);
    //    return strategy;
    //}

    //public static TileShape GetBuildingByStrategy(BuildingStrategy strategy)
    //{
    //    TileShape shape = m_ShapeFactory.GetDShape();
    //    GameTile tile = m_TileFactory.GetBasicTile();
    //    BuildingContent content = m_ContentFactory.GetBuildingContent(strategy);
    //    tile.SetContent(content);
    //    shape.SetTile(tile);
    //    return shape;
    //}


    //测试用

    public static GameTile GetTileWithContent(GameTileContent content)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        tile.SetContent(content);
        return tile;
    }

    //public static TileShape GetBuildingByName(string name)
    //{
    //    TurretAttribute attribute = m_ContentFactory.GetRefactorByString
    //    RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, new List<int> { 1, 1, 1 }, new List<int> { 1, 1, 1 });
    //    TileShape shape = GetRefactorTurretByStrategy(strategy);
    //    return shape;
    //}
    public static TileShape GetTrapShapeByName(string name)//测试用，生成一个随意放置的陷阱
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        shape.SetTile(GetTrap(name, true));
        return shape;
    }

    //测试用
    public static TileShape GetRefactorTurretByNameAndElement(string name, int e1, int e2, int e3)
    {
        TurretAttribute attribute = m_ContentFactory.GetRefactorByString(name);
        List<int> elements = new List<int> { e1, e2, e3 };
        List<int> qualities = new List<int> { 1, 1, 1 };

        RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, elements, qualities, 1);
        ElementSkill addElementSkill = TurretSkillFactory.GetElementSkill(elements);
        strategy.AddElementSkill(addElementSkill);
        TurretSkillFactory.AddGlobalSkillToStrategy(strategy);

        TileShape shape = GetRefactorTurretByStrategy(strategy);
        return shape;
    }

    public static TileShape GetElementTurretByQualityAndElement(ElementType element, int quality)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = GetElementTurret(element, quality);
        shape.SetTile(tile);
        return shape;
    }

    public static TileShape GetShapeByContent(GameTileContent content)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        shape.SetTile(content.m_GameTile);
        return shape;
    }


    //教学用
    public static TileShape GetTutorialShape(ShapeInfo shapeInfo, bool levelDown = false)
    {
        TileShape shape = m_ShapeFactory.GetShape(shapeInfo);
        GameTile tile = GetElementTurret((ElementType)shapeInfo.Element, levelDown ? shapeInfo.Quality - 1 : shapeInfo.Quality);
        shape.SetTile(tile, shapeInfo.TurretPos, shapeInfo.TurretDir);
        return shape;
    }


    //该方法不再自动添加元素技能，因为涉及异常构造的保存读取问题
    public static RefactorStrategy GetSpecificStrategyByString(string name, List<int> elements, List<int> qualities, int quality = 1)
    {
        TurretAttribute attribute = m_ContentFactory.GetRefactorByString(name);
        RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, elements, qualities, quality);
        //strategy.AddElementSkill(TurretSkillFactory.GetElementSkill(elements));
        TurretSkillFactory.AddGlobalSkillToStrategy(strategy);
        return strategy;
    }

    //读取保存数据
    public static GameTile GetElementTurret(ElementType element, int quality)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        ElementTurret content = m_ContentFactory.GetElementTurret(element, quality);
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetElementTurret(ContentStruct contentStruct)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        ElementTurret content = m_ContentFactory.GetElementTurret((ElementType)contentStruct.Element, contentStruct.Quality);
        tile.SetContent(content);
        content.Strategy.TotalDamage = long.Parse(contentStruct.TotalDamage);
        return tile;
    }

    public static GameTile GetTrap(string name, bool isReveal)
    {
        GameTileContent content = m_ContentFactory.GetTrapContentByName(name, isReveal);
        if (content == null)
        {
            return null;
        }
        GameTile tile = m_TileFactory.GetBasicTile();
        tile.SetContent(content);
        return tile;
    }


    public static GameTile GetRefactorTurret(ContentStruct contentStruct)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        List<int> elements;
        RefactorStrategy strategy = GetSpecificStrategyByString(contentStruct.ContentName, new List<int> { 1, 1, 1 }, new List<int> { 1, 1, 1 }, contentStruct.Quality);
        RefactorTurret content = m_ContentFactory.GetRefactorTurret(strategy);
        strategy.PrivateExtraSlot = contentStruct.ExtraSlot;
        tile.SetContent(content);
        for (int i = 0; i < contentStruct.SkillList.Count; i++)
        {
            elements = contentStruct.SkillList[(i + 1).ToString()];
            ElementSkill skill = TurretSkillFactory.GetElementSkill(elements);
            skill.Elements = contentStruct.ElementsList[(i + 1).ToString()];
            skill.IsException = contentStruct.IsException[(i + 1).ToString()];
            content.Strategy.AddElementSkill(skill);
            ((BasicTile)tile).EquipTurret(contentStruct.SkillList.Count);
        }
        strategy.TotalDamage = long.Parse(contentStruct.TotalDamage);
        return tile;
    }

    public static TileShape GetSaveDragingContent(ContentStruct content)
    {
        GameTile tile = null;
        switch (content.ContentType)
        {
            case 3:
                tile = GetElementTurret(content);
                break;
            case 4:
                tile = GetRefactorTurret(content);
                break;
            case 5:
                tile = GetTrap(content.ContentName, content.TrapRevealed);
                break;
            //case 7:
            //    tile = GetBuilding(content);
            //    break;
            default:
                Debug.Log("错误的保存类型");
                tile = m_TileFactory.GetBasicTile();
                break;

        }
        TileShape shape = m_ShapeFactory.GetDShape();
        shape.SetTile(tile);
        return shape;
    }



    //public static GameTile GetBuilding(ContentStruct structContent)
    //{
    //    TurretAttribute attribute = m_ContentFactory.GetBuildingAtt(structContent.ContentName);
    //    RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, new List<int> { 1, 1, 1 }, new List<int> { 1, 1, 1 });
    //    GameTile tile = m_TileFactory.GetBasicTile();
    //    RefactorTurret content = m_ContentFactory.GetRefactorTurret(strategy);
    //    tile.SetContent(content);
    //    return tile;
    //}


}
