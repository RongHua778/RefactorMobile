using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Soilder, Runner, Restorer, Tanker, Transfer, Froster, Fission, Leader, Fisson_Small,
    Hamster, SixArmor, Divider, Blinker, Binary,
    IceBall, Borner, Armorer,
    Borner2, AircraftCarrier, AircraftCarrier2,
    GoldKeeper, Divider_Middle, Divider_Small,
    Knight,Knight2,
    Cheetah,Rhinoceros,Bipolar
        , None
}
[CreateAssetMenu(menuName = "Factory/EnemyFactory", fileName = "EnemyFactory")]
public class EnemyFactory : ScriptableObject
{
    [SerializeField] List<EnemyAttribute> enemies = new List<EnemyAttribute>();
    private Dictionary<EnemyType, EnemyAttribute> EnemyDIC;

    public void InitializeFactory()
    {
        EnemyDIC = new Dictionary<EnemyType, EnemyAttribute>();
        foreach (var enemy in enemies)
        {
            EnemyDIC.Add(enemy.EnemyType, enemy);
        }
    }
    public EnemyAttribute Get(EnemyType type)
    {
        if (EnemyDIC.ContainsKey(type))
        {
            return EnemyDIC[type];
        }
        Debug.Log("使用了未定义的敌人类型");
        return null;
    }



}
