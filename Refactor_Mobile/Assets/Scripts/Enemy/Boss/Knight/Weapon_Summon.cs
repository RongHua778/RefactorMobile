using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Summon : Weapon
{
    [SerializeField] private EnemyType[] summonTypes = default;
    protected override void TriggerWeapon()
    {
        base.TriggerWeapon();
        EnemyType type = summonTypes[Random.Range(0, summonTypes.Length)];
        GameManager.Instance.SpawnEnemy(type, Knight.PointIndex, Knight.Intensify / 2f, Knight.DmgResist, BoardSystem.shortestPoints);



    }


}
