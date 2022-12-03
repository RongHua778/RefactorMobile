using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffableTurret : MonoBehaviour
{
    public List<TurretBuff> TurretBuffs = new List<TurretBuff>();

    private ConcreteContent TurretContent;

    private void Awake()
    {
        TurretContent = this.GetComponent<ConcreteContent>();
    }

    public void TimeTick()
    {
        foreach (var buff in TurretBuffs.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                TurretBuffs.Remove(buff);
            }
        }
    }

    public void AddBuff(TurretBuffInfo buffInfo)
    {
        TurretBuff newBuff = TurretBuffFactory.GetBuff((int)buffInfo.BuffName);
        foreach (var buff in TurretBuffs)
        {
            if (buff.TBuffName == newBuff.TBuffName)
            {
                buff.ApplyBuff(TurretContent.Strategy, buffInfo.Stacks, buffInfo.Duration);
                return;
            }
        }
        newBuff.ApplyBuff(TurretContent.Strategy, buffInfo.Stacks, buffInfo.Duration);
        TurretBuffs.Add(newBuff);
       
    }

    public void ClearBuffs()
    {
        TurretBuffs.Clear();
    }
}
