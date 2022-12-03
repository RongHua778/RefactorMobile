using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcutionTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        base.OnContentPass(enemy, content, index);
        float damage = ((enemy.DamageStrategy.MaxHealth - enemy.DamageStrategy.CurrentHealth) * 0.12f * (1 + TrapIntensify + enemy.DamageStrategy.TrapIntensify)) / (1f - enemy.DamageStrategy.HiddenResist);
        float damageReturn;
        enemy.DamageStrategy.ApplyDamage(damage, out damageReturn, null, false);
        StaticData.Instance.ShowJumpDamage(enemy.model.position, (long)damageReturn, true);
        DamageAnalysis += (long)damageReturn;
        enemy.DamageStrategy.TrapIntensify = 0;
    }
}
