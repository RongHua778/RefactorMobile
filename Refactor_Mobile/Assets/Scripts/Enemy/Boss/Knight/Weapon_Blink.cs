using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Blink : Weapon
{

    protected override void TriggerWeapon()
    {
        base.TriggerWeapon();
        int blinkDistance = Mathf.Min(6, 1 + ((GameRes.CurrentWave + 1) / 20));
        Knight.Blink(blinkDistance);
    }


}
