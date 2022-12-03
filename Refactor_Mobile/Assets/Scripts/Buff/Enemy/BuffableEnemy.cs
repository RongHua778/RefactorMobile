using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BuffableEnemy : MonoBehaviour
{
    public List<TileBuff> TileBuffs = new List<TileBuff>();
    public List<TimeBuff> TimeBuffs = new List<TimeBuff>();
    public Enemy Enemy { get; set; }

    private void Awake()
    {
        Enemy = this.GetComponent<Enemy>();
    }

    //private void Update()
    //{
    //    TimeTick();
    //}

    public void TimeTick()
    {
        var effects = TimeBuffs.ToList().GetEnumerator();
        while (effects.MoveNext())
        {
            effects.Current.Tick(Time.deltaTime);
            if (effects.Current.IsFinished)
            {
                TimeBuffs.Remove(effects.Current);
            }
        }

    }

    public void TileTick()
    {
        foreach (var buff in TileBuffs.ToList())
        {
            buff.Tick(1);
            if (buff.IsFinished)
            {
                TileBuffs.Remove(buff);
            }
        }
    }

    public void OnHit()
    {
        foreach (var buff in TimeBuffs)
        {
            buff.OnHit();
        }
    }

    public void AddBuff(BuffInfo buffInfo)
    {
        EnemyBuff newBuff = EnemyBuffFactory.GetBuff((int)buffInfo.EnemyBuffName);
        if (newBuff.IsTimeBase)
        {
            foreach (var buff in TimeBuffs)
            {
                if (buff.BuffName == newBuff.BuffName)//已经存在该BUFF，再次施加BUFF,刷新持续时间
                {
                    buff.ApplyBuff(Enemy, buffInfo.Stacks, buffInfo.IsAbnormal);
                    return;
                }
            }
            TimeBuffs.Add(newBuff as TimeBuff);
        }
        else
        {
            if (!newBuff.IsStackable)
            {
                foreach (var buff in TileBuffs)
                {
                    if (buff.BuffName == newBuff.BuffName)
                    {
                        return;
                    }
                }
            }
            TileBuffs.Add(newBuff as TileBuff);
        }
        newBuff.ApplyBuff(Enemy, buffInfo.Stacks, buffInfo.IsAbnormal);
    }

    public void RemoveAllBuffs()
    {
        foreach(var buff in TimeBuffs)
        {
            buff.End();
        }
        foreach (var buff in TileBuffs)
        {
            buff.End();
        }
        TimeBuffs.Clear();
        TileBuffs.Clear();
    }

    public void RemoveBuff(EnemyBuffName buffName)
    {
        foreach (var buff in TimeBuffs)
        {
            if (buff.BuffName == buffName)
            {
                TimeBuffs.Remove(buff);
                return;
            }
        }
        foreach (var buff in TileBuffs)
        {
            if (buff.BuffName == buffName)
            {
                TileBuffs.Remove(buff);
                return;
            }
        }
    }
}
