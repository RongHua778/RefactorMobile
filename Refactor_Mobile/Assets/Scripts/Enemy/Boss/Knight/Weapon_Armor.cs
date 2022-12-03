using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Armor : Weapon
{
    [SerializeField] private ArmorHolder armorHolderPrefab = default;
    [SerializeField] private float armorIntensify = default;
    protected override void TriggerWeapon()
    {
        base.TriggerWeapon();

        if (Knight.HoldingArmor != null)
        {
            Destroy(Knight.HoldingArmor.gameObject);
            Knight.HoldingArmor = null;
        }

        ArmorHolder armorHolder = Instantiate(armorHolderPrefab, Knight.gfxSprite.transform);
        armorHolder.Initialize(Knight, Knight.DamageStrategy.MaxHealth * armorIntensify);
        Knight.HoldingArmor = armorHolder;
    }

}
