using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Restore : Weapon
{
    //[SerializeField] private float restoreHealthPercent = default;
    protected override void TriggerWeapon()
    {
        base.TriggerWeapon();
        BuffInfo buffInfo = new BuffInfo(EnemyBuffName.SpeedUpBuff, 1);
        Knight.DamageStrategy.ApplyBuff(buffInfo);
        //float restoreHealth = Knight.DamageStrategy.MaxHealth * restoreHealthPercent;
        //Knight.DamageStrategy.CurrentHealth += restoreHealth;
    }

}
