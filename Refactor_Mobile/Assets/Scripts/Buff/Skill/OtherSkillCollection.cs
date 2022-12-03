using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class RapidBase : ElementSkill
{

    public override List<int> InitElements => new List<int> { 0, 1, 2 };
    public override float KeyValue => 1.5f;
    public override float KeyValue2 => 0.1f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    private float intensifiedValue;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration = 9999;
        strategy.TurnFixFirerate += KeyValue;
        intensifiedValue += KeyValue;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        if (strategy.Concrete.IsAttacking)
        {
            if (intensifiedValue > 0)
            {
                strategy.TurnFixFirerate -= KeyValue2 * delta;
                intensifiedValue -= KeyValue2 * delta;
            }
        }
    }

    public override void EndTurn()
    {
        base.EndTurn();
        intensifiedValue = 0;
    }




}
public class CenterBase : ElementSkill
{
    //强袭基座
    public override List<int> InitElements => new List<int> { 0, 1, 3 };


    public override float KeyValue => 20f;
    public override float KeyValue2 => 0.1f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    float intensifiedValue;
    float reduce;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration = 9999;
    }
    public override void Shoot(IDamage target = null, Bullet bullet = null)
    {
        base.Shoot(target, bullet);
        strategy.TurnFixAttack += KeyValue * bullet.BulletEffectIntensify;
        intensifiedValue += KeyValue * bullet.BulletEffectIntensify;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        reduce = intensifiedValue * KeyValue2 * delta;
        strategy.TurnFixAttack -= reduce;
        intensifiedValue -= reduce;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        reduce = 0;
        intensifiedValue = 0;
    }



}

public class FrostCounter : ElementSkill
{

    public override List<int> InitElements => new List<int> { 0, 1, 4 };
    public override float KeyValue => 1.5f;
    public override float KeyValue2 => 6f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    private float chargedTime;
    private bool charged;

    public override void StartTurn()
    {
        base.StartTurn();
        Duration = 9999;
    }

    public override void Tick(float delta)
    {
        if (chargedTime > 0)
        {
            if (!charged)
            {
                //strategy.TurnFixSplashRange += KeyValue;
                strategy.TurnAttackIntensify += KeyValue;
                strategy.TurnFireRateIntensify += KeyValue;
                charged = true;
            }
            chargedTime -= delta;
            if (chargedTime <= 0)
            {
                if (charged)
                {
                    //strategy.TurnFixSplashRange -= KeyValue;
                    strategy.TurnAttackIntensify -= KeyValue;
                    strategy.TurnFireRateIntensify -= KeyValue;
                    charged = false;
                }
            }
        }
    }
    public override void Frost()
    {
        base.Frost();
        chargedTime = 5f;
    }
    public override void EndTurn()
    {
        base.EndTurn();
        chargedTime = 0f;
        charged = false;
    }
}

public class PointStrike : ElementSkill
{
    //破冰子弹
    public override List<int> InitElements => new List<int> { 0, 2, 3 };

    public override float KeyValue => 0.35f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");


    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        if (target.DamageStrategy.IsFrost)
        {
            damage *= (1 + KeyValue * bullet.BulletEffectIntensify);
        }
        return damage;
    }
}

public class UnrealStructure : ElementSkill
{

    public override List<int> InitElements => new List<int> { 0, 2, 4 };//ACE

    public override float KeyValue => 1;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());

    public override void Build()
    {

    }
    public override void Composite()
    {
        //GameManager.Instance.GainPerfectElement(1);
        List<int> newElements = new List<int> { Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5) };
        ElementSkill newSkill = TurretSkillFactory.GetElementSkill(newElements);
        newElements = new List<int> { Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5) };
        ElementSkill newSkill2 = TurretSkillFactory.GetElementSkill(newElements);
        newSkill.Elements = newSkill2.Elements;
        newSkill.IsException = true;
        //newSkill.Elements = newElements;
        strategy.TurretSkills.Remove(this);
        strategy.AddElementSkill(newSkill);

        for (int i = 0; i < 3; i++)//设置配方基础元素需求，用于重置
        {
            ((RefactorStrategy)strategy).Compositions[i].elementRequirement = newSkill.Elements[i];
        }

        newSkill.Composite();//触发合成效果

    }


}
public class SplashBullet : ElementSkill
{

    public override List<int> InitElements => new List<int> { 0, 3, 4 };

    public override float KeyValue => 0.3f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());

    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        float distance = bullet.GetTargetDistance();
        bullet.SplashRange += KeyValue * distance * bullet.BulletEffectIntensify;
    }


}


public class FrostBullet : ElementSkill
{
    //变形子弹
    public override List<int> InitElements => new List<int> { 1, 2, 3 };
    public override float KeyValue => 0.5f;
    public override float KeyValue2 => 2f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");


    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    if (target == null)
    //        return;
    //    if (target.DamageStrategy.CurrentFrost / target.DamageStrategy.MaxFrost <= KeyValue)
    //    {
    //        bullet.SlowRate += bullet.SlowRate * bullet.BulletEffectIntensify;
    //    }
    //}

    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        if (target.DamageStrategy.CurrentFrost / target.DamageStrategy.MaxFrost <= KeyValue)
        {
            target.DamageStrategy.ApplyFrost(bullet.SlowRate);
        }
        return base.Hit(damage, target, bullet);
    }


}

public class DoubleBullet : ElementSkill
{
    //双层子弹
    public override List<int> InitElements => new List<int> { 1, 2, 4 };


    public override float KeyValue => 1f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("WHENHIT"));

    public override void Build()
    {
        base.Build();
        strategy.BaseFixBulletEffectIntensify += KeyValue;
    }

}

public class CritBase : ElementSkill
{

    public override List<int> InitElements => new List<int> { 1, 3, 4 };

    public override float KeyValue => 0.5f;
    public override float KeyValue2 => 0.35f;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");


    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    if (target == null)
    //        return;
    //    if (target.DamageStrategy.HealthPercent <= KeyValue)
    //    {
    //        bullet.isCritical = true;
    //        bullet.CriticalPercentage += KeyValue2 * bullet.BulletEffectIntensify;
    //    }
    //}

    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        if (target.DamageStrategy.HealthPercent <= KeyValue)
        {
            damage *= (1 + KeyValue2 * bullet.BulletEffectIntensify);
        }
        return base.Hit(damage, target, bullet);
    }


}
public class IceShell : ElementSkill
{
    //环状
    public override List<int> InitElements => new List<int> { 2, 3, 4 };

    public override float KeyValue => 1;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    private bool changeCircle;
    private RangeType lastType;


    public override void StartTurn()
    {
        base.StartTurn();
        if (strategy.RangeType != RangeType.Circle)//Rotary会在Initialize时将Checkangle改为360度，所以需要加入一个判断，避免再次修改攻击范围类型使其checkangle变回10度，回旋塔同理
        {
            if (!changeCircle)
            {
                changeCircle = true;
                lastType = strategy.RangeType;
                strategy.RangeType = RangeType.Circle;
            }
        }
        else
            strategy.TurnFixRange += 1;
        if (strategy.Concrete != null)
            strategy.Concrete.GenerateRange();
    }

    public override void EndTurn()
    {
        base.EndTurn();
        if (changeCircle)
        {
            strategy.RangeType = lastType;
        }
        changeCircle = false;
        strategy.Concrete.GenerateRange();
    }

}









