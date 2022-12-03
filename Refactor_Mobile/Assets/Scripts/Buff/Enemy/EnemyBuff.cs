using UnityEngine;

public enum EnemyBuffName
{
    SlowDown,
    TileCountStun,
    TileBaseDamageIntensify,
    DirectionSlow,
    BreakIntensify,
    DamageTarget,
    Stun,
    TileStun,
    DamageIntensify,
    RuleLowStrikeBuff,
    TinyMap,
    LongMap,
    SlowIntensify,
    RuleStrikeBuff,
    RuleRestoreBuff,
    RuleAmountBuff,
    RuleAirborneBuff,
    RuleFastBuff,
    Invisible,
    TileDamageResistBuff,
    SpeedUpBuff

}
public abstract class EnemyBuff
{
    public abstract EnemyBuffName BuffName { get; }
    public bool IsFinished { get; set; }
    public abstract bool IsTimeBase { get; }
    public abstract bool IsStackable { get; }
    public virtual int MaxStacks { get => 1; }
    public int CurrentStack;
    public virtual float KeyValue { get; }
    public virtual float KeyValue2 { get; }
    public Enemy Target;
    public float Duration;
    public bool IsAbnormal;
    public virtual float BasicDuration { get; }
    public virtual void ApplyBuff(Enemy target, int stacks, bool isAbnormal = false)
    {
        this.Target = target;
        this.IsAbnormal = isAbnormal;
    }


    public abstract void Affect(int stacks);

    public virtual void Tick(float delta)//先TICK再Affect
    {

    }
    public virtual void OnHit() { }
    public abstract void End();


}

public abstract class TimeBuff : EnemyBuff
{

    private int addStacks;
    public override void ApplyBuff(Enemy target, int stacks, bool isAbnomral = false)
    {
        base.ApplyBuff(target, stacks, isAbnomral);
        Duration = BasicDuration;
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
    public override void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }
}

public abstract class TileBuff : EnemyBuff
{
    public int TileCount;
    public int PrivateStacks;//格子BUFF每个BUFF单独计算层数
    public override void ApplyBuff(Enemy target, int stacks, bool isAbnomral = false)
    {
        base.ApplyBuff(target, stacks, isAbnomral);
        TileCount = (int)BasicDuration;
        PrivateStacks = stacks;
        Affect(stacks);
    }
    public override void Tick(float delta)//先TICK再Affect
    {
        TileCount -= (int)delta;
        if (TileCount <= 0)
        {
            End();
            IsFinished = true;
            return;
        }
    }
}

public class SlowBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.SlowDown;
    public override bool IsStackable => true;
    public override bool IsTimeBase => true;

    public override float KeyValue => 0.1f;
    public override int MaxStacks => 6;
    public override float BasicDuration => 3f;


    public override void Affect(int stacks)
    {
        Target.SpeedAdjust -= KeyValue * stacks;
        Target.HealthBar.ShowIcon(1, true);

    }
    public override void End()
    {
        Target.SpeedAdjust += KeyValue * CurrentStack;
        Target.HealthBar.ShowIcon(1, false);

    }
}

public class DamageIntensifyBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.DamageIntensify;
    public override bool IsStackable => true;
    public override bool IsTimeBase => true;

    public override int MaxStacks => 100;
    public override float KeyValue => 0.01f;
    public override float BasicDuration => 3f;
    public override void Affect(int stacks)
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue * stacks);
    }

    public override void End()
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue * CurrentStack);
    }
}

public class TinyMapBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.TinyMap;

    public override float KeyValue => 0.5f;
    public override float BasicDuration => 25;
    public override bool IsTimeBase => false;

    public override bool IsStackable => false;

    public override void Tick(float delta)
    {
        base.Tick(delta);
    }
    public override void Affect(int stacks)
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue);
    }

    public override void End()
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue);
    }
}

public class LongMapBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.LongMap;

    public override float KeyValue => 0.02f;
    public override float KeyValue2 => 0.2f;
    public override float BasicDuration => 999;

    public override bool IsTimeBase => false;

    public override bool IsStackable => false;
    private float maxValue => KeyValue2 + 1f;
    private float intensifiedValue;

    public override void Tick(float delta)
    {
        if (intensifiedValue < maxValue)
        {
            intensifiedValue += KeyValue;
            Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue);
        }
    }
    public override void Affect(int stacks)
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue2);
    }

    public override void End()
    {

    }


}

public class LowStrikeBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleLowStrikeBuff;
    public override float BasicDuration => 9999f;
    public override float KeyValue => 0.05f;
    public override float KeyValue2 => 0.2f;

    public override bool IsStackable => false;
    public override bool IsTimeBase => true;

    private float intensifyValue;
    private float intensify;

    public override void Affect(int stacks)
    {

    }
    public override void Tick(float delta)
    {
        if (intensifyValue > 0)
        {
            intensify = KeyValue2 * delta;
            Target.DamageStrategy.ApplyBuffDmgIntensify(intensify);
            intensifyValue -= intensify;
        }
    }
    public override void OnHit()
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue);
        intensifyValue += KeyValue;
    }

    public override void End()
    {

    }

}


public class TileCountStunBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.TileCountStun;
    public override bool IsStackable => true;
    public override bool IsTimeBase => false;
    private int tileCount;
    public override float BasicDuration => 999;
    public override float KeyValue => 0.1f;
    public override void Tick(float delta)
    {
        //base.Tick(delta);
        if (tileCount > 0 && Target.CurrentTile.Content.ContentType == GameTileContentType.Trap)
        {
            End();
            IsFinished = true;
        }
        else
        {
            tileCount++;
        }
    }

    public override void Affect(int stacks)
    {
        tileCount = 0;
        //Target.HealthBar.ShowIcon(2, true);
    }

    public override void End()
    {
        Target.DamageStrategy.StunTime += PrivateStacks * KeyValue * tileCount;
        //Target.HealthBar.ShowIcon(2, false);
    }
}

public class TileBaseDamageIntensify : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.TileBaseDamageIntensify;

    public override bool IsStackable => true;

    public override bool IsTimeBase => false;

    public override float KeyValue => 0.35f;

    public override float BasicDuration => 3;

    public override void Affect(int stacks)
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue * stacks);
        Target.HealthBar.ShowIcon(0, true);

    }

    public override void End()
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue * PrivateStacks);
        Target.HealthBar.ShowIcon(0, false);
    }
}


public class SlowIntensify : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.SlowIntensify;
    public override bool IsStackable => true;
    public override bool IsTimeBase => true;
    public override int MaxStacks => 100;
    public override float KeyValue => 0.01f;
    public override float BasicDuration => 3f;


    public override void Affect(int stacks)
    {
        Target.DamageStrategy.FrostIntensify += KeyValue * stacks;
    }

    public override void End()
    {
        Target.DamageStrategy.FrostIntensify -= KeyValue * CurrentStack;
    }
}

public class BreakIntensify : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.BreakIntensify;
    public override bool IsStackable => true;
    public override bool IsTimeBase => true;
    public override int MaxStacks => 2;
    public override float KeyValue => 0.5f;
    public override float BasicDuration => 5f;


    public override void Affect(int stacks)
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue * stacks);
        Target.HealthBar.ShowIcon(2, true);
    }

    public override void End()
    {
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue * CurrentStack);
        Target.HealthBar.ShowIcon(2, false);

    }
}

public class StrikeBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleStrikeBuff;

    public override float BasicDuration => 20f;
    public override bool IsTimeBase => true;
    public override bool IsStackable => false;

    private int initPointIndex = 0;

    public override void End()
    {
        int distance = Mathf.Max(0, (Target.PointIndex - initPointIndex) / 2);
        Target.Flash(-distance);
    }

    public override void Affect(int stacks)
    {
        initPointIndex = Target.PointIndex;
    }
}


public class RestoreBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleRestoreBuff;

    public override float KeyValue => 0.3f;
    public override bool IsTimeBase => false;
    public override bool IsStackable => false;
    public override float BasicDuration => 999;


    public override void Tick(float delta)
    {
        base.Tick(delta);
        if (Target.DamageStrategy.PathProgress > 0.5f)
        {
            End();
            IsFinished = true;
        }
    }
    public override void Affect(int stacks)
    {

    }

    public override void End()
    {
        Target.DamageStrategy.CurrentHealth += (Target.DamageStrategy.MaxHealth - Target.DamageStrategy.CurrentHealth) * KeyValue;

    }
}

public class AmountBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleAmountBuff;

    public override float KeyValue => 25f;
    public override float BasicDuration => 9999f;
    public override bool IsTimeBase => true;
    public override bool IsStackable => false;

    private float timeCounter;

    public override void Tick(float delta)
    {
        base.Tick(delta);
        timeCounter += delta;
        if (timeCounter > KeyValue)
        {
            timeCounter = 0;
            Duplicate();
        }
    }

    private void Duplicate()
    {
        if (GameRes.EnemyRemain >= Target.MaxAmount)
            return;
        GameManager.Instance.SpawnEnemy(Target.EnemyType, Target.PointIndex,
            Target.Intensify * 0.5f * Target.DamageStrategy.HealthPercent, Target.DmgResist,BoardSystem.shortestPoints);
    }

    public override void Affect(int stacks)
    {

    }

    public override void End()
    {

    }
}

public class AirborneBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleAirborneBuff;
    public override float BasicDuration => 0.1f;
    public override bool IsTimeBase => true;
    public override bool IsStackable => false;

    public override void Affect(int stacks)
    {

    }

    public override void End()
    {
        if (GameRes.EnemyRemain >= Target.MaxAmount)
            return;
        Enemy enemy = GameManager.Instance.SpawnEnemy(Target.EnemyType, ((Target.PathPoints.Count - Target.PointIndex) / 2) + Target.PointIndex,
             Target.Intensify * 0.25f, Target.DmgResist,BoardSystem.shortestPoints);
        enemy.Buffable.RemoveBuff(EnemyBuffName.RuleAirborneBuff);
    }
}

public class RuleFastBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.RuleFastBuff;
    public override float BasicDuration => 999f;
    public override float KeyValue => 0.025f;
    public override bool IsTimeBase => false;
    public override bool IsStackable => false;

    public override void Tick(float delta)
    {
        base.Tick(delta);
        Target.SpeedIntensify += KeyValue;
    }
    public override void Affect(int stacks)
    {

    }

    public override void End()
    {
    }
}

public class InvisibleBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.Invisible;
    public override float BasicDuration => 999f;
    public override float KeyValue => 0.25f;
    public override bool IsTimeBase => false;
    public override bool IsStackable => false;
    private float timeCounter;

    private Collider2D[] hitResult = new Collider2D[10];
    public override void Tick(float delta)
    {
        base.Tick(delta);
        timeCounter += delta;
        if (timeCounter > KeyValue)
        {
            int hits = Physics2D.OverlapCircleNonAlloc(Target.transform.position, 3f, hitResult, StaticData.TurretLayerMask);
            bool detect = false;
            for (int i = 0; i < hits; i++)
            {
                if (hitResult[i].GetComponent<RefactorTurret>())
                {
                    detect = true;
                    break;
                }
            }
            Target.DamageStrategy.InVisible = !detect;
            timeCounter = 0;

        }
    }
    public override void Affect(int stacks)
    {
        Target.DamageStrategy.InVisible = true;
    }

    public override void End()
    {
    }
}

public class TileDamageResistBuff : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.TileDamageResistBuff;
    public override float BasicDuration => 50f;
    public override float KeyValue => 1.5f;
    public override float KeyValue2 => 0.05f;

    public override bool IsStackable => false;
    public override bool IsTimeBase => false;

    private float intensifyValue;


    public override void Affect(int stacks)
    {
        intensifyValue = KeyValue;
        Target.DamageStrategy.ApplyBuffDmgIntensify(-KeyValue);
    }

    public override void Tick(float delta)
    {
        if (intensifyValue > 0)
        {
            intensifyValue -= KeyValue2;
            Target.DamageStrategy.ApplyBuffDmgIntensify(KeyValue2);
        }
    }

    public override void End()
    {
        
    }


}

public class SpeedUpBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.SpeedUpBuff;
    public override float BasicDuration => 6f;
    public override float KeyValue => 1.5f;

    public override bool IsStackable => false;
    public override bool IsTimeBase => true;


    public override void Affect(int stacks)
    {
        Target.SpeedIntensify += KeyValue;
    }


    public override void End()
    {
        Target.SpeedIntensify -= KeyValue;
    }

}



