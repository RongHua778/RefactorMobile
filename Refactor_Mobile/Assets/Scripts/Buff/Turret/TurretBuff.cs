using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretBuffName
{
    FirerateBuff,
    DamageBonusBuff,
    BasicCritBuff
}
public abstract class TurretBuff
{
    public abstract TurretBuffName TBuffName { get; }
    public bool IsFinished { get; set; }
    public virtual float KeyValue { get; }
    public virtual float KeyValue2 { get; }

    public int CurrentStack;
    public virtual int MaxStacks { get => 1; }
    public float Duration;
    public StrategyBase TStrategy;
    private int addStacks;
    public void ApplyBuff(StrategyBase strategy,int stacks,float duration)
    {
        TStrategy = strategy;
        Duration = duration;
        addStacks = 0;
        if (CurrentStack + stacks > MaxStacks)
        {
            addStacks = MaxStacks - CurrentStack;
        }
        else
        {
            addStacks = stacks;
        }
        CurrentStack += addStacks;
        Affect(addStacks);
    }

    public abstract void Affect(int stacks);


    public virtual void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }
    public abstract void End();
}

public class FirerateBuff : TurretBuff
{
    public override TurretBuffName TBuffName => TurretBuffName.FirerateBuff;
    public override int MaxStacks => 10;
    public override float KeyValue => 0.05f;

    public override void Affect(int stacks)
    {
        TStrategy.TurnFireRateIntensify += KeyValue * stacks;
    }

    public override void End()
    {
        TStrategy.TurnFireRateIntensify -= KeyValue * CurrentStack;
    }
}

public class DamageBonusBuff : TurretBuff
{
    public override TurretBuffName TBuffName => TurretBuffName.DamageBonusBuff;
    public override int MaxStacks => 4;
    public override float KeyValue => 0.25f;

    public override void Affect(int stacks)
    {
        TStrategy.TurnFixDamageBonus += KeyValue * stacks;
    }

    public override void End()
    {
        TStrategy.TurnFixDamageBonus -= KeyValue * CurrentStack;
    }
}

public class BasicCritBuff : TurretBuff
{
    public override TurretBuffName TBuffName => TurretBuffName.BasicCritBuff;
    public override int MaxStacks => 10;
    public override float KeyValue => 0.05f;

    public override void Affect(int stacks)
    {
        TStrategy.TurnFixCriticalRate += KeyValue * stacks;
    }

    public override void End()
    {
        TStrategy.TurnFixCriticalRate -= KeyValue * CurrentStack;
    }
}