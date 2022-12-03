using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class RandomCritical : ElementSkill
{

    public override List<int> InitElements => new List<int> { 3, 3, 3 };
    public override float KeyValue => 0.25f;
    public override float KeyValue2 => 3f;
    private float frostResist => 0.35f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2 * 100 + "%");
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(frostResist * 100 + "%");

    public override void Build()
    {
        base.Build();
        strategy.BaseFixFrostResist -= frostResist;
    }

    public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    {
        base.AfterShoot(bullet, target);
        bullet.BaseAttack *= Random.Range(KeyValue, KeyValue2 * bullet.BulletEffectIntensify);
    }


    //public override void AfterShoot(Bullet bullet = null, IDamage target = null)
    //{
    //    base.AfterShoot(bullet, target);
    //    bullet.CriticalPercentage *= Random.Range(KeyValue, KeyValue2);
    //}

    //public override void PreHit(Bullet bullet = null)
    //{
    //    base.PreHit(bullet);
    //    bullet.CriticalPercentage *= Random.Range(KeyValue, KeyValue2);

    //}

}

public class FarCrit : ElementSkill
{
    //��ս����
    public override List<int> InitElements => new List<int> { 1, 1, 3 };
    public override float KeyValue => 0.15f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");

    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFrostResist += KeyValue * strategy.FireCount;
    }
}

public class HitCritical : ElementSkill
{
    //��Ӧ����
    public override List<int> InitElements => new List<int> { 2, 2, 3 };
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));

    private BuffInfo buffInfo;
    public override void Build()
    {
        base.Build();
        buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1);

    }
    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        buffInfo.Stacks = Mathf.RoundToInt(strategy.FireCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return damage;
    }
}

public class SlowCritical : ElementSkill
{
    //��������
    public override List<int> InitElements => new List<int> { 4, 4, 3 };
    public override float KeyValue => 1;
    public override float KeyValue2 => 1;

    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue.ToString());
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
    public override string DisplayValue3 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue2.ToString());

    BuffInfo buffInfo;
    public override void Build()
    {
        base.Build();
        buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1);
    }

    public override float Hit(float damage, IDamage target, Bullet bullet = null)
    {
        buffInfo.Stacks = Mathf.RoundToInt(strategy.FireCount * bullet.BulletEffectIntensify);
        target.DamageStrategy.ApplyBuff(buffInfo);
        return base.Hit(damage, target, bullet);
    }
}

public class StartCritical : ElementSkill
{
    //רע����
    public override List<int> InitElements => new List<int> { 0, 0, 3 };
    public override float KeyValue => 0.2f;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.None].Colorized("1");
    public override string DisplayValue2 => StaticData.ElementDIC[ElementType.None].Colorized(KeyValue * 100 + "%");
    public override void StartTurn2()
    {
        base.StartTurn2();
        strategy.TurnFixDamageBonus += KeyValue * strategy.FireCount;
    }
}

//public class CopySkill : ElementSkill
//{
//    //װ�������Ƶ�һ��Ԫ�ؼ���

//    public override List<int> Elements => new List<int> { 3, 3, 3 };
//    public override string SkillDescription => "COPYSKILL";

//    public override void OnEquip()
//    {
//        ElementSkill skill = strategy.TurretSkills[1] as ElementSkill;
//        if (skill.Elements.SequenceEqual(Elements))
//        {
//            Debug.Log("�����ظ��ĸ��Ƽ���");
//            return;
//        }
//        //strategy.GetComIntensify(Elements, false);
//        ElementSkill newSkill = TurretEffectFactory.GetElementSkill(skill.Elements);
//        strategy.TurretSkills.Remove(this);
//        newSkill.Composite();//�����ϳ�Ч��
//        strategy.AddElementSkill(newSkill);
//    }
//}
//public class AttackCritical : ElementSkill
//{
//    public override List<int> Elements => new List<int> { 3, 3, 0 };
//    private float attackIntensified;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                attackIntensified = 0.6f * strategy.TimeModify;
//                strategy.TurnAttackIntensify += 0.6f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnAttackIntensify -= attackIntensified;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        attackIntensified = 0;
//        isIntensify = false;
//    }

//}

//public class SpeedCritical : ElementSkill
//{
//    //������ʹ������1�빥������50%
//    public override List<int> Elements => new List<int> { 3, 3, 1 };

//    private float speedIncreased;
//    private float timeCounter;
//    private bool isIntensify;

//    public override void StartTurn()
//    {
//        Duration += 999;
//    }
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            timeCounter = 2f;
//            if (!isIntensify)
//            {
//                isIntensify = true;
//                speedIncreased = 0.5f * strategy.TimeModify;
//                strategy.TurnSpeedIntensify += 0.5f * strategy.TimeModify;
//            }
//        }
//    }
//    public override void Tick(float delta)
//    {
//        base.Tick(delta);
//        if (timeCounter >= 0)
//        {
//            timeCounter -= delta;
//            if (timeCounter <= 0)
//            {
//                isIntensify = false;
//                strategy.TurnSpeedIntensify -= speedIncreased;
//            }
//        }

//    }

//    public override void EndTurn()
//    {
//        speedIncreased = 0;
//        isIntensify = false;
//    }

//}

//public class CriticalSlow : ElementSkill
//{
//    //������ɵļ���Ч������
//    public override List<int> Elements => new List<int> { 3, 3, 2 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SlowRate *= 2f;
//        }
//    }
//}

//public class CriticalSplash : ElementSkill
//{
//    //������ɵļ���Ч������
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override void PreHit(Bullet bullet = null)
//    {
//        if (bullet.isCritical)
//        {
//            bullet.SputteringRange *= 2f;
//        }
//    }
//}








//public class CriticalAdjacent : ElementSkill
//{
//    //����ÿ���������������25%������
//    public override List<int> Elements => new List<int> { 3, 3, 4 };
//    public override string SkillDescription => "CRITICALADJACENT";

//    private int adjacentTurretCount = 0;
//    public override void Detect()
//    {
//        strategy.BaseCriticalPercentageIntensify -= 0.25f * adjacentTurretCount;//�޸��س�ʼֵ
//        adjacentTurretCount = 0;
//        List<Vector2Int> points = StaticData.GetCirclePoints(1);
//        foreach (var point in points)
//        {
//            Vector2 pos = point + (Vector2)strategy.m_Turret.transform.position;
//            Collider2D hit = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.TurretMask));
//            if (hit != null)
//                adjacentTurretCount++;
//        }
//        strategy.BaseCriticalPercentageIntensify += 0.25f * adjacentTurretCount;
//    }
//}
