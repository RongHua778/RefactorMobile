using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binary : Boss
{

    public static Binary FirstBinary;
    private Binary m_brother;
    public override EnemyType EnemyType => EnemyType.Binary;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
        if (FirstBinary == null)
        {
            SpeedIntensify = 0;
            FirstBinary = this;
            m_brother = GameManager.Instance.SpawnEnemy(this.EnemyType, this.PointIndex, intensify, dmgResist, BoardSystem.shortestPoints) as Binary;
            m_brother.m_brother = this;
            //FirstBinary = null;
            m_brother.SpeedIntensify = 0;
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (m_brother != null)
        {
            m_brother.SpeedIntensify += 1f;
            m_brother.ProgressFactor = m_brother.Speed * m_brother.Adjust;
            m_brother.ShowBossText(1);
            m_brother.m_brother = null;
            m_brother = null;
        }
        else
        {
            FirstBinary = null;
        }
    }

    public override void OnDie()
    {
        base.OnDie();
        if (m_brother == null)
        {
            FirstBinary = null;
            LevelManager.Instance.SetAchievement("ACH_BINARY");
        }

    }




}
