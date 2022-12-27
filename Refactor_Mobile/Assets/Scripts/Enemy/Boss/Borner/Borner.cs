using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Boss
{
    public override EnemyType EnemyType => EnemyType.Borner;

    [SerializeField] float bornCD = default;
    [SerializeField] protected int bornCount = default;
    [SerializeField] protected float bornTime = default;
    float bornCounter;
    [SerializeField] float dmgIntensifyWhenBorning;
    [SerializeField] protected int minIndex = default;
    [SerializeField] protected int maxIndex = default;
    //[SerializeField] private float dmgIntentWhenSpawn = default;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        bornCounter = bornCD / 2;
        //DamageStrategy.ApplyBuffDmgIntensify(dmgIntentWhenSpawn);
        //dmgIntensifyWhenBorning = 1f - dmgIntentWhenSpawn;
    }

    protected override void OnEnemyUpdate()
    {

        base.OnEnemyUpdate();
        bornCounter -= Time.deltaTime;
        if (bornCounter <= 0)
        {
            bornCounter = bornCD;
            if (GameRes.EnemyRemain < StaticData.MaxEnemyAmount)
            {
                StartCoroutine(CastleBorn());
                ShowBossText(0.3f);
            }
        }
    }

    private IEnumerator CastleBorn()
    {
        DamageStrategy.StunTime += bornTime;
        DamageStrategy.ApplyBuffDmgIntensify(dmgIntensifyWhenBorning);
        anim.SetBool("Transform", true);
        Sound.Instance.PlayEffect("Sound_BornerTransform");
        int count = Mathf.RoundToInt(bornCount * GameRes.EnemyAmoundAdjust);
        for (int i = 0; i < count; i++)
        {
            Born();
            yield return new WaitForSeconds(bornTime / count);
        }
        anim.SetBool("Transform", false);
        DamageStrategy.ApplyBuffDmgIntensify(-dmgIntensifyWhenBorning);
        Sound.Instance.PlayEffect("Sound_BornerTransform");
    }

    private void Born()
    {
        int typeInt = Random.Range(minIndex, maxIndex);
        GameManager.Instance.SpawnEnemy((EnemyType)typeInt, PointIndex, (Intensify / 4f) * GameRes.EnemyIntensifyAdjust, DmgResist, BoardSystem.shortestPoints);
    }

    //public override void OnDie()
    //{
    //    base.OnDie();
    //    LevelManager.Instance.SetAchievement("ACH_FORTRESS");
    //}

}
