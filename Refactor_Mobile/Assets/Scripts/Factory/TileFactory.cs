using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BasicTileType
{
    None, SpawnPoint, Destination, Ground, Turret, Trap
}
[CreateAssetMenu(menuName = "Factory/TileFactory", fileName = "TileFactory")]
public class TileFactory : GameObjectFactory
{
    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile basicTilePrefab = default;
    [SerializeField] GameObject tutorialPrefab = default;

    public void Initialize()
    {

    }

    public GameTile GetBasicTile()
    {
        BasicTile basicTile = CreateInstance(basicTilePrefab) as BasicTile;
        return basicTile;
    }
    public GroundTile GetGroundTile()
    {
        return CreateInstance(groundTile) as GroundTile;

    }
    public GameObject GetTutorialPrefab()
    {
        return tutorialPrefab;
    }


    //public GameTile BuildTrapTile()
    //{
    //    return null;
    //}

    //public GameTile BuildTurretTile()
    //{
    //    return null;

    //}



    //public TrapTile GetRandomTrap()
    //{
    //    int index = Random.Range(0,trapAttributes.Length);
    //    TrapTile trap = CreateInstance(trapAttributes[index].ContentPrefab).GetComponent<TrapTile>();
    //    trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
    //    return trap;
    //}

    //public TrapTile GetTrapByName(string name)
    //{
    //    foreach(var attribute in trapAttributes)
    //    {
    //        if (attribute.Name == name)
    //        {
    //            TrapTile trap = CreateInstance(attribute.ContentPrefab).GetComponent<TrapTile>();
    //            trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
    //            return trap;
    //        }
    //    }
    //    Debug.LogWarning("û��������ֵ�TRAP");
    //    return null;
    //}


    //******************turrettile����**************

    //public TurretTile GetBasicTurret(int quality, int element)
    //{
    //    TurretAttribute attribute = GameManager.Instance.GetElementAttribute((Element)element);
    //    GameObject temp = CreateInstance(attribute.ContentPrefab);
    //    TurretTile tile = temp.GetComponent<TurretTile>();
    //    tile.Initialize(attribute, quality);
    //    return tile;
    //}
    //public GameTile GetRandomElementTile()
    //{
    //    int playerLevel = LevelUIManager.Instance.PlayerLevel;
    //    int element = StaticData.RandomNumber(elementTileChance);
    //    float[] levelC = new float[5];
    //    for (int i = 0; i < 5; i++)
    //    {
    //        levelC[i] = StaticData.Instance.LevelChances[playerLevel - 1, i];
    //    }
    //    int level = StaticData.RandomNumber(levelC) + 1;
    //    GameTile temp = GetBasicTurret(level, element);
    //    return temp;
    //}

    //public TurretTile GetCompositeTurretTile(TurretAttribute attribute)
    //{
    //    GameObject temp = CreateInstance(attribute.ContentPrefab);
    //    TurretTile tile = temp.GetComponent<TurretTile>();
    //    tile.Initialize(attribute, 1);
    //    return tile;
    //}

}
