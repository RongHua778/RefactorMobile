using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IDamage
{
    Collider2D TargetCollider { get; set; }
    SpriteRenderer gfxSprite { get; set; }
    DamageStrategy DamageStrategy { get; set; }
    HealthBar_Sprie HealthBar { get; set; }
}


public abstract class DamageStrategy
{
    public IDamage damageTarget;
    public Transform ModelTrans;

    public float HiddenResist;//test

    public virtual float CurrentFrost { get; set; }
    public virtual float MaxFrost { get; set; }

    public virtual float PathProgress { get; }
    public virtual int PathIndex { get; }
    protected float currentHealth;
    protected float maxHealth;
    private bool isDie;
    private int trapIntensify = 0;
    public float StunTime;
    public virtual bool IsStun => StunTime > 0;
    public virtual float FrostTime { get; set; }
    public virtual bool IsFrost { get; }
    public bool IsControlled => IsFrost || IsStun;

    private float frostIntensify;
    public virtual float FrostIntensify { get => frostIntensify; set => frostIntensify = value; }
    public virtual int TrapIntensify
    {
        get => trapIntensify;
        set => trapIntensify = value;
    }
    public virtual bool IsDie
    {
        get => isDie;
        set => isDie = value;
    }
    public abstract bool IsEnemy { get; }
    private bool inVisible;
    public bool InVisible
    {
        get => inVisible;
        set
        {
            inVisible = value;
            //damageTarget.TargetCollider.enabled = !value;
            GFXFade(true);
        }
    }

    public float MaxTransparentValue => InVisible ? 0.3f : 1f;

    public void GFXFade(bool show)
    {
        damageTarget.gfxSprite.DOColor(new Color(1, 1, 1, show ? MaxTransparentValue : 0f), 0.5f);
    }
    public virtual float DamageIntensify { get => Mathf.Max(0.1f, 1 + BuffDamageIntensify); }
    public virtual float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && !IsDie)
            {
                IsDie = true;
            }
            damageTarget.HealthBar.FillAmount = currentHealth / MaxHealth;
        }
    }
    public virtual float MaxHealth { get => maxHealth; set { maxHealth = value; CurrentHealth = maxHealth; } }
    public float HealthPercent => CurrentHealth / MaxHealth;


    //buff属性
    private float buffDamageIntensify;
    public virtual float BuffDamageIntensify { get => buffDamageIntensify; set => buffDamageIntensify = value; }


    public virtual void ApplyBuffDmgIntensify(float value)
    {
        BuffDamageIntensify += value;
    }
    public DamageStrategy(IDamage damageTarget)
    {
        this.damageTarget = damageTarget;
    }


    public virtual void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
    {
        realDamage = amount * (acceptIntensify ? DamageIntensify : 1) * (InVisible ? 0.25f : 1f);
        CurrentHealth -= realDamage * (1 - HiddenResist);//200波后为测试
    }


    public virtual void ApplyBuff(BuffInfo buffInfo)
    {

    }

    public virtual void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResit)
    {
        IsDie = false;
        this.MaxHealth = Mathf.Max(1, attribute.Health * intensify);

        HiddenResist = dmgResit;
        InVisible = false;
        BuffDamageIntensify = 0;
        FrostIntensify = 0;
    }

    public virtual void StrategyUpdate()
    {

    }

    public virtual void ApplyFrost(float value)
    {
        CurrentFrost += value;
    }

    public virtual void UnFrost() { }

}

public class BasicEnemyStrategy : DamageStrategy
{
    public override bool IsEnemy => true;
    protected Enemy enemy;
    public float UnfrostableTime;
    public FrostEffect m_FrostEffect;

    public override float PathProgress => (float)enemy.PointIndex / enemy.PathPoints.Count;
    public override int PathIndex => enemy.PointIndex;
    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (IsDie)
            {
                UnFrost();
                enemy.OnDie();
            }

        }
    }
    public override bool IsFrost => FrostTime > 0;
    private float currentFrost;
    public override float CurrentFrost
    {
        get => currentFrost;
        set
        {
            if (UnfrostableTime > 0)
                return;

            currentFrost = value;

            if (currentFrost >= MaxFrost && enemy.gameObject.activeSelf)
            {
                currentFrost = 0;
                FrostEnemy(GameRes.EnemyFrostTime * (1 + FrostIntensify - GameRes.EnemyFrostResist));
            }
            enemy.HealthBar.FrostAmount = CurrentFrost / MaxFrost;
        }
    }
    public override float FrostIntensify
    {
        get => base.FrostIntensify;
        set
        {
            base.FrostIntensify = value;
            enemy.HealthBar.FrostIntensify = value;
        }
    }
    public override int TrapIntensify
    {
        get => base.TrapIntensify;
        set
        {
            base.TrapIntensify = value;
        }
    }

    public BasicEnemyStrategy(IDamage damageTarget) : base(damageTarget)
    {
        this.enemy = damageTarget as Enemy;
        this.ModelTrans = enemy.model;
    }

    public override void UnFrost()
    {
        if (m_FrostEffect != null)
        {
            m_FrostEffect.Broke();
            m_FrostEffect = null;
            UnfrostableTime -= FrostTime;
            FrostTime = 0;
        }
    }

    public override float BuffDamageIntensify
    {
        get => base.BuffDamageIntensify;
        set
        {
            base.BuffDamageIntensify = value;
            enemy.HealthBar.DamageIntensify = value;
            //enemy.HealthBar.ShowIcon(1, value > 0);
        }
    }


    public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
    {
        base.ResetStrategy(attribute, intensify, dmgResist);
        MaxFrost = (1 + GameRes.CurrentWave * (1 + GameRes.CurrentWave / 30f)) * attribute.Frost;
        TrapIntensify = 0;
        CurrentFrost = 0;
        StunTime = 0;
        FrostTime = 0;
        UnfrostableTime = 0;
    }

    public virtual void FrostEnemy(float time)
    {
        FrostEffect frosteffect = StaticData.Instance.FrostEffect(ModelTrans.position);
        frosteffect.transform.localScale = Vector3.one * 0.85f;
        FrostTime += time;
        UnfrostableTime += time + 3f;//免疫冻结时间
        m_FrostEffect = frosteffect;
    }

    public override void StrategyUpdate()
    {
        if (FrostTime > 0)
        {
            FrostTime -= Time.deltaTime;
            if (FrostTime <= 0.2f)
            {
                UnFrost();
            }
        }
        if (StunTime > 0)
        {
            StunTime -= Time.deltaTime;
        }
        if (UnfrostableTime > 0)
        {
            UnfrostableTime -= Time.deltaTime;
        }
    }

    public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
    {
        base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
        enemy.Buffable.OnHit();
    }


    public override void ApplyBuff(BuffInfo buffInfo)
    {
        enemy.Buffable.AddBuff(buffInfo);
    }

}



public class ArmourStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    Armor armor;
    public override float PathProgress => armor.EnemyParent.PointIndex / armor.EnemyParent.PathPoints.Count;
    public override int PathIndex => armor.EnemyParent.PointIndex;

    public override float DamageIntensify => armor.EnemyParent.DamageStrategy.DamageIntensify;

    public override bool IsStun => armor.EnemyParent.DamageStrategy.IsStun;
    public override bool IsFrost => armor.EnemyParent.DamageStrategy.IsFrost;
    public ArmourStrategy(IDamage damageTarget, float dmgResist) : base(damageTarget)
    {
        this.HiddenResist = dmgResist;
        this.damageTarget = damageTarget;
        this.armor = damageTarget as Armor;
        this.ModelTrans = armor.transform;

    }
    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (value)
                armor.DisArmor();
        }
    }

}

public class WeaponStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    public FrostEffect m_FrostEffect;

    Weapon weapon;
    public override float PathProgress => weapon.Knight.PointIndex / weapon.Knight.PathPoints.Count;
    public override int PathIndex => weapon.Knight.PointIndex;

    public override float DamageIntensify => weapon.Knight.DamageStrategy.DamageIntensify;

    public override float FrostIntensify => weapon.Knight.DamageStrategy.FrostIntensify;
    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (IsDie)
            {
                UnFrost();
            }
        }
    }
    public override bool IsFrost => FrostTime > 0;

    public float UnfrostableTime;
    private float damageCounter;

    private float currentFrost;
    public override float CurrentFrost
    {
        get => currentFrost;
        set
        {
            if (UnfrostableTime > 0)
                return;

            currentFrost = value;

            if (currentFrost >= MaxFrost && weapon.gameObject.activeSelf)
            {
                currentFrost = 0;
                FrostWeapon(GameRes.EnemyFrostTime * (1 + FrostIntensify - GameRes.EnemyFrostResist));
            }
            weapon.HealthBar.FrostAmount = CurrentFrost / MaxFrost;
        }
    }

    private void FrostWeapon(float time)
    {
        FrostEffect frosteffect = StaticData.Instance.FrostEffect(ModelTrans.position);
        frosteffect.transform.localScale = Vector3.one * 0.85f;
        FrostTime += time;
        UnfrostableTime += time + 3f;//免疫冻结时间
        m_FrostEffect = frosteffect;
    }

    public override void UnFrost()
    {
        if (m_FrostEffect != null)
        {
            m_FrostEffect.Broke();
            m_FrostEffect = null;
            UnfrostableTime -= FrostTime;
            FrostTime = 0;
        }
    }

    public override void StrategyUpdate()
    {
        if (FrostTime > 0)
        {
            FrostTime -= Time.deltaTime;
            if (FrostTime <= 0.2f)
            {
                UnFrost();
            }
        }
        if (UnfrostableTime > 0)
        {
            UnfrostableTime -= Time.deltaTime;
        }


        damageCounter += Time.deltaTime;
        if (damageCounter > 2f)
        {
            weapon.SpeedModify = 2f;
        }
        else
        {
            weapon.SpeedModify = 1f;
        }
    }

    public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
    {
        base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
        damageCounter = 0f;
    }


    public WeaponStrategy(IDamage damageTarget, float dmgResist, float frost) : base(damageTarget)
    {
        this.HiddenResist = dmgResist;
        this.damageTarget = damageTarget;
        this.weapon = damageTarget as Weapon;
        this.ModelTrans = weapon.transform;
        MaxFrost = (1 + GameRes.CurrentWave * (1 + GameRes.CurrentWave / 30f)) * frost;
        damageCounter = 2f;
    }


}

public class AircraftStrategy : DamageStrategy
{
    public override bool IsEnemy => false;
    Aircraft aircraft;
    public override float PathProgress => aircraft.boss.PointIndex / aircraft.boss.PathPoints.Count;
    public override int PathIndex => aircraft.boss.PointIndex;

    public override float DamageIntensify => 1;

    public float DmgIntensifyWhenDie = 0;

    public override bool IsStun => false;
    public override bool IsFrost => false;


    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (IsDie)
                aircraft.boss.DamageStrategy.ApplyBuffDmgIntensify(DmgIntensifyWhenDie);
        }
    }
    public AircraftStrategy(IDamage damageTarget, float dmgIntenWhenDie, float dmgResist) : base(damageTarget)
    {
        this.HiddenResist = dmgResist;
        this.damageTarget = damageTarget;
        this.aircraft = damageTarget as Aircraft;
        this.ModelTrans = aircraft.transform;
        this.DmgIntensifyWhenDie = dmgIntenWhenDie;

    }


}

public class RestorerStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    public float damagedCounter;

    public RestorerStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }

    public override void ApplyDamage(float amount, out float realDamage, Bullet bullet = null, bool acceptIntensify = true)
    {
        base.ApplyDamage(amount, out realDamage, bullet, acceptIntensify);
        damagedCounter = 0;
    }

    public override void StrategyUpdate()
    {
        base.StrategyUpdate();
        //CurrentHealth += MaxHealth * 0.025f * Time.deltaTime;
        damagedCounter += Time.deltaTime;
        if (damagedCounter > 2f)
        {
            CurrentHealth += MaxHealth * 0.05f * Time.deltaTime;
        }
    }
}

public class HamsterStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    //public override float BuffDamageIntensify { get => base.BuffDamageIntensify + Hamster.HamsterDamageIntensify; set => base.BuffDamageIntensify = value; }
    public HamsterStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }

}

public class CheetahStrateygy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    private float speedIntensified;
    private float maxSpeedIntensify = 1.5f;
    //public override float BuffDamageIntensify { get => base.BuffDamageIntensify + Hamster.HamsterDamageIntensify; set => base.BuffDamageIntensify = value; }
    public CheetahStrateygy(IDamage damageTarget) : base(damageTarget)
    {
    }

    public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
    {
        base.ResetStrategy(attribute, intensify, dmgResist);
        speedIntensified = 0f;
    }
    public override void StrategyUpdate()
    {
        base.StrategyUpdate();
        if (speedIntensified < maxSpeedIntensify)
        {
            float intent = 0.35f * Time.deltaTime;
            enemy.SpeedIntensify += intent;
            speedIntensified += intent;
        }
    }


    public override void UnFrost()
    {
        base.UnFrost();
        enemy.SpeedIntensify -= speedIntensified;
        speedIntensified = 0;
    }


}

public class LeaderStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    public float ReduceValue;
    //private float buffDmgAdjust = 2f;
    public LeaderStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }





}

public class GoldKeeperStrategy : BasicEnemyStrategy
{
    public override bool IsEnemy => true;
    int gainCount = 1;
    private float healthInterval = 0.02f;
    public override float CurrentHealth
    {
        get => base.CurrentHealth;
        set
        {
            base.CurrentHealth = value;

            int count = (int)(((1 - healthInterval * gainCount) - (currentHealth / MaxHealth)) / healthInterval);
            if (count > 0)
                GainGold(count);

        }
    }

    public override bool IsDie
    {
        get => base.IsDie;
        set
        {
            base.IsDie = value;
            if (value)
            {
                GameRes.GainPerfectBattleTurn++;
                StaticData.Instance.GainPerfectEffect(enemy.model.position, 1);
            }
        }
    }

    private void GainGold(int count)
    {
        gainCount += count;
        StaticData.Instance.GainMoneyEffect(enemy.model.position, Mathf.Min(10, Mathf.RoundToInt(GameRes.CurrentWave * 0.4f)) * count);

    }


    public GoldKeeperStrategy(IDamage damageTarget) : base(damageTarget)
    {
    }

    public override void ResetStrategy(EnemyAttribute attribute, float intensify, float dmgResist)
    {
        base.ResetStrategy(attribute, intensify, dmgResist);
        gainCount = 1;
    }
}



