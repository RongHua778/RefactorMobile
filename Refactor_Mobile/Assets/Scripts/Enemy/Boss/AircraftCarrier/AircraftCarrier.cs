using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Boss
{
    public override EnemyType EnemyType => EnemyType.AircraftCarrier;
    [SerializeField] float aircarftIntensify;

    List<Aircraft> aircrafts = new List<Aircraft>();

    float bornCounter;
    float bornCD;

    protected float dmgIntenWhenAircraftDie;

    [SerializeField] private Aircraft battleAirCraft=default;
    [SerializeField] private int airCrifatAmount;
    //[SerializeField] private float dmgIntentWhenSpawn = default;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        //DamageStrategy.ApplyBuffDmgIntensify(dmgIntentWhenSpawn);
        bornCD = 1;
        //airCrifatAmount = Mathf.Min(30, 8 + 2 * ((GameRes.CurrentWave + 1) / 20));
        airCrifatAmount = Mathf.Min(15, 4 + ((GameRes.CurrentWave + 1) / 20));

        dmgIntenWhenAircraftDie = 1f / airCrifatAmount;
    }


    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        if (bornCounter < bornCD)
        {
            bornCounter += Time.deltaTime;
            if (bornCounter > bornCD)
            {
                StartCoroutine(BornCor());  //开局3秒后第一次诞生小飞机
                ShowBossText(1f);
            }
        }
    }

    IEnumerator BornCor()
    {
        anim.SetBool("Transform", true);
        Sound.Instance.PlayEffect("Sound_BornerTransform");

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Mathf.RoundToInt(airCrifatAmount * GameRes.EnemyAmoundAdjust); i++)
        {
            AirAttacker aircraft = ObjectPool.Instance.Spawn(battleAirCraft) as AirAttacker;
            aircraft.transform.position = this.model.position;
            aircraft.Initiate(this, DamageStrategy.MaxHealth * aircarftIntensify * GameRes.EnemyIntensifyAdjust, dmgIntenWhenAircraftDie, DmgResist);
            Sound.Instance.PlayEffect("Sound_Aircraft");
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Transform", false);
    }



    public void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
    }

    //public override void OnDie()
    //{
    //    base.OnDie();
    //    LevelManager.Instance.SetAchievement("ACH_BATTLESHIP");
    //}

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        bornCounter = 0;
        for (int i = 0; i < aircrafts.Count; i++)
        {
            aircrafts[i].DamageStrategy.CurrentHealth = 0;
        }
        aircrafts.Clear();
    }
}
