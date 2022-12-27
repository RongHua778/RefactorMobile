using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixArmour : Boss
{
    public override EnemyType EnemyType => EnemyType.SixArmor;
    [SerializeField] float armorIntensify = default;
    [SerializeField] ArmorHolder armorHolderPrefab = default;
    private ArmorHolder m_ArmorHolder;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        m_ArmorHolder = Instantiate(armorHolderPrefab, this.gfxSprite.transform);
        m_ArmorHolder.Initialize(this, DamageStrategy.MaxHealth * armorIntensify);
        ShowBossText(0.5f);
    }

    //public override void OnDie()
    //{
    //    base.OnDie();
    //    LevelManager.Instance.SetAchievement("ACH_TORTOISE");
    //}

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Destroy(m_ArmorHolder.gameObject);
        m_ArmorHolder = null;
    }
}
