using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knight : Boss
{
    public override EnemyType EnemyType => EnemyType.Knight;

    [Header("武器")]
    [SerializeField] protected Weapon[] level1Weapon = default;
    [SerializeField] protected Weapon[] level2Weapon = default;
    [SerializeField] protected Weapon[] level3Weapon = default;


    [SerializeField] private ParticalControl summonEffect = default;

    private float level1Counter;
    private float level2Counter;
    private float level3Counter;


    public int weaponInScene = 0;
    [SerializeField]
    private int maxWeaponCount = 6;

    //private bool strongMode = false;

    [SerializeField] protected float[] levelInterval = default;


    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        ShowBossText(1);
        weaponInScene = 0;
        //strongMode = false;
    }
    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        //if (!strongMode && DamageStrategy.HealthPercent <= 0.35f)
        //{
        //    strongMode = true;
        //}
        // float percent = strongMode ? 0.35f : 1f;
        if (weaponInScene >= maxWeaponCount)//超出最大同屏装备数量
            return;
        level1Counter += Time.deltaTime;
        if (level1Counter > levelInterval[0])
        {
            StartCoroutine(SummonWeapon(1));
            level1Counter = 0f;
        }

        level2Counter += Time.deltaTime;
        if (level2Counter > levelInterval[1])
        {
            StartCoroutine(SummonWeapon(2));
            level2Counter = 0f;
        }

        level3Counter += Time.deltaTime;
        if (level3Counter > levelInterval[2])
        {
            StartCoroutine(SummonWeapon(3));
            level3Counter = 0f;
        }

    }
    private IEnumerator SummonWeapon(int level)
    {
        weaponInScene++;
        Weapon weapon = null;
        float offsetMul = 0;
        switch (level)
        {
            case 1:
                weapon = level1Weapon[Random.Range(0, level1Weapon.Length)];
                offsetMul = 8f;
                break;
            case 2:
                weapon = level2Weapon[Random.Range(0, level2Weapon.Length)];
                offsetMul = 15f;
                break;
            case 3:
                weapon = level3Weapon[Random.Range(0, level3Weapon.Length)];
                offsetMul = 20f;
                break;
        }

        Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle.normalized * offsetMul;

        ParticalControl effect = ObjectPool.Instance.Spawn(summonEffect) as ParticalControl;
        effect.transform.position = pos;
        effect.PlayEffect();
        yield return new WaitForSeconds(1f);
        if (!DamageStrategy.IsDie)
        {
            weapon = ObjectPool.Instance.Spawn(weapon) as Weapon;
            weapon.transform.localScale = Vector2.zero;
            weapon.transform.DOScale(Vector2.one, 1f);
            weapon.transform.position = pos;
            weapon.Initiate(this);
        }
    }



}
