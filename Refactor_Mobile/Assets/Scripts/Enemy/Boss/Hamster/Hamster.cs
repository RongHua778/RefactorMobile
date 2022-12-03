using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : Boss
{

    public static bool isFirstHamster = false;

    public static List<Hamster> AllHamsters;
    public override EnemyType EnemyType => EnemyType.Hamster;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        if (!isFirstHamster)
        {
            isFirstHamster = true;
            AllHamsters = new List<Hamster>();
        }
        DamageStrategy.ApplyBuffDmgIntensify(-0.75f);
        AllHamsters.Add(this);
        ShowBossText(0.3f);
    }
    protected override void SetStrategy()
    {
        DamageStrategy = new HamsterStrategy(this);
    }

    public override void OnDie()
    {
        base.OnDie();
        HamsterReduce();
        
    }

    private void HamsterReduce()
    {
        AllHamsters.Remove(this);
        foreach (var hamster in AllHamsters)
        {
            hamster.DamageStrategy.ApplyBuffDmgIntensify(0.25f); 
        }
        if (AllHamsters.Count <= 0)
        {
            LevelManager.Instance.SetAchievement("ACH_HAMSTER");
        }

    }

    public override void EnemyExit()
    {
        HamsterReduce();
        base.EnemyExit();
    }

}
