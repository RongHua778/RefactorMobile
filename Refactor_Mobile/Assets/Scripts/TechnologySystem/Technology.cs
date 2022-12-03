using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechnologyName
{
    TECHCOMBAT,
    TECHFIRE,
    TECHECONOMIC,
    TECHMASS,
    TECHMASTER,
    TECHINVESTMENT,
    TECHPERFECT,
    TECHLEGEND,
    TECHMAP,
    TECHTEMP,
    TECHESLOT,

    TECHCONSTRUCTOR,
    TECHRAPIDER,
    TECHSCATTER,
    TECHCOORDINATOR,
    TECHROTARY,
    TECHSNOW,
    TECHMORTAR,
    TECHSNIPER,
    TECHSUPER,
    TECHBOOMERANG,
    TECHULTRA,
    TECHCORE,

    TECHRAPID,
    TECHINSTRUMENT,

    TECHCHILLER,
    TECHFIRER,
    TECHLASER,
    TECHBOMBARD,
    TECHMINER,
    TECHNUCLEAR


}
public abstract class Technology
{
    public string SpriteResDir => "Sprites/PixelUI/Icon/" + TechName;
    public abstract TechnologyName TechnologyName { get; }
    public virtual RefactorTurretName RefactorBinding => RefactorTurretName.None;
    public virtual bool ContainGlobalBuff => false;
    public string TechName => TechnologyName.ToString();
    public string TechnologyDes => IsAbnormal ?
        StaticData.ElementDIC[ElementType.GOLD].Colorized("<sprite=8>" + GameMultiLang.GetTraduction(TechName + "INFO3")) + "\n"
        + StaticData.ElementDIC[ElementType.FIRE].Colorized("<sprite=9>" + GameMultiLang.GetTraduction(TechName + "INFO2")) :
        GameMultiLang.GetTraduction(TechName + "INFO");

    private bool isAbnormal;
    public virtual bool IsAbnormal
    {
        get => isAbnormal;
        set
        {
            isAbnormal = value;
            if (m_EnemyBuffInfo != null)
            {
                m_Buff.IsAbnormal = value;
                m_EnemyBuffInfo.IsAbnormal = value;
            }
            if (m_SkillInfo != null)
            {
                m_Skill.IsAbnormal = value;
                m_SkillInfo.IsAbnormal = value;
            }
        }
    }

    public virtual bool CanAbnormal { get; set; }//石否允许切换到异常
    public virtual float KeyValue { get; set; }
    public virtual float KeyValue2 { get; set; }
    public virtual float KeyValue3 { get; set; }

    public virtual float SaveValue { get; set; }
    public virtual string DisplayValue1 { get; }
    public virtual string DisplayValue2 { get; }
    public virtual string DisplayValue3 { get; }
    public virtual string DisplayValue4 { get; }
    public virtual string DisplayValue5 { get; }

    protected EnemyBuff m_Buff;
    protected BuffInfo m_EnemyBuffInfo;

    protected GlobalSkillInfo m_SkillInfo;
    protected GlobalSkill m_Skill;

    public virtual void InitializeTech() { }//科技初始化
    public virtual void OnGet()//获取该科技时触发
    {
        if (m_EnemyBuffInfo != null)
            EnemyBuffFactory.GlobalBuffs.Add(m_EnemyBuffInfo);
        if (m_Skill != null)
            TurretSkillFactory.AddGlobalSkill(m_SkillInfo);
    }

    public virtual bool OnGet2()//获取建筑，只触发一次，存档不触发
    {
        return false;
    }

    public virtual void OnEquip(StrategyBase strategy) { }


    public virtual void OnTurnStart()//回合开始时触发
    {

    }


    public virtual void OnTurnEnd()
    {

    }

    public virtual void OnWaveEnd()//战斗阶段结束时触发
    {

    }

    public virtual void OnRefactor(StrategyBase strategy)//每次重构时触发
    {

    }

}


//public class TechGold : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHGOLD;

//    public override float KeyValue => IsAbnormal ? StaticData.Instance.GoldAttackIntensify : StaticData.Instance.GoldAttackIntensify / 2;
//    public override float KeyValue2 => StaticData.Instance.WoodFirerateIntensify;
//    public override string DisplayValue1 => KeyValue * 100 + "%";
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";

//    public override void OnGet()
//    {
//        base.OnGet();
//        GameRes.GameGoldIntensify += KeyValue;
//        if (IsAbnormal)
//            GameRes.GameWoodIntensify -= KeyValue2;
//    }
//}
//public class TechWood : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHWOOD;
//    public override float KeyValue => IsAbnormal ? StaticData.Instance.WoodFirerateIntensify : StaticData.Instance.WoodFirerateIntensify / 2;
//    public override float KeyValue2 => StaticData.Instance.DustSplashIntensify;
//    public override string DisplayValue1 => KeyValue * 100 + "%";
//    public override string DisplayValue2 => KeyValue2.ToString();
//    public override void OnGet()
//    {
//        base.OnGet();
//        GameRes.GameWoodIntensify += KeyValue;
//        if (IsAbnormal)
//            GameRes.GameDustIntensify -= KeyValue2;
//    }
//}
//public class TechWater : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHWATER;

//    public override float KeyValue => IsAbnormal ? StaticData.Instance.WaterSlowIntensify : StaticData.Instance.WaterSlowIntensify / 2;
//    public override float KeyValue2 => 0.1f;
//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";
//    public override void OnGet()
//    {
//        base.OnGet();
//        GameRes.GameWaterIntensify += KeyValue;
//        if (IsAbnormal)
//            GameRes.GameFireIntensify -= KeyValue2;
//    }
//}

//public class TechFire : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHFIRE;
//    public override float KeyValue => IsAbnormal ? StaticData.Instance.FireCritIntensify : StaticData.Instance.FireCritIntensify / 2f;
//    public override float KeyValue2 => StaticData.Instance.GoldAttackIntensify;
//    public override string DisplayValue1 => KeyValue * 100 + "%";
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";
//    public override void OnGet()
//    {
//        base.OnGet();
//        GameRes.GameFireIntensify += KeyValue;
//        if (IsAbnormal)
//            GameRes.GameGoldIntensify -= KeyValue2;
//    }
//}
//public class TechDust : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHDUST;

//    public override float KeyValue => IsAbnormal ? StaticData.Instance.DustSplashIntensify : StaticData.Instance.DustSplashIntensify / 2;
//    public override float KeyValue2 => StaticData.Instance.WaterSlowIntensify;
//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => KeyValue2.ToString();

//    public override void OnGet()
//    {
//        base.OnGet();
//        GameRes.GameDustIntensify += KeyValue;
//        if (IsAbnormal)
//            GameRes.GameWaterIntensify -= KeyValue2;
//    }
//}

public class TechCombat : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHCOMBAT;
    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue3 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue2 * 100 + "%";


    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechCombatSkill, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);
    }

    public override void OnGet()
    {
        base.OnGet();
        GameManager.Instance.TriggerDetectSkills();

    }

}

public class TechFire : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHFIRE;
    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechFireSkill, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }



}


public class TechEconomic : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHECONOMIC;
    public override float KeyValue => 0.05f;
    public override float KeyValue2 => 0.1f;
    //private int shopCost => 10;
    public override string DisplayValue1 => KeyValue * 100 + "%";
    //public override string DisplayValue2 => shopCost.ToString();

    public override string DisplayValue3 => KeyValue2 * 100 + "%";
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameRes.BuildDiscount += KeyValue;
        //else
        //    GameRes.RefreshShopCost += shopCost;
    }

    public override void OnRefactor(StrategyBase strategy)
    {
        base.OnRefactor(strategy);
        if (IsAbnormal)
            GameRes.BuildCost = Mathf.RoundToInt(GameRes.BuildCost * (1 - KeyValue2));
    }


    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        if (IsAbnormal)
            GameManager.Instance.RemoveUnlockedRecipes();
    }
}




public class TechMass : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHMASS;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2.ToString();
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";



    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.TechMassSkill, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        GameManager.Instance.TriggerDetectSkills();
    }

}

public class TechMaster : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHMASTER;
    public override float KeyValue => 1f;
    public override float KeyValue2 => 2f;

    public override string DisplayValue1 => KeyValue.ToString();
    public override string DisplayValue2 => KeyValue.ToString();
    public override string DisplayValue3 => KeyValue2.ToString();
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
        {
            GameRes.LockCount += (int)KeyValue;
        }
        else
        {
            GameRes.LockCount -= (int)KeyValue;
            GameRes.ShopCapacity += (int)KeyValue2;
        }
    }




}

public class TechInvestment : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHINVESTMENT;
    public override float KeyValue => 100;
    public override float KeyValue2 => 1f;
    private float keyValue3 => 35f;
    public override string DisplayValue1 => KeyValue.ToString();
    public override string DisplayValue2 => KeyValue2.ToString();
    public override string DisplayValue3 => keyValue3.ToString();

    private int refactorThisTurn;
    public override float SaveValue { get => refactorThisTurn; set => refactorThisTurn = (int)value; }
    public override void OnGet()
    {
        base.OnGet();
        if (IsAbnormal)
            GameRes.LockCount -= (int)KeyValue2;
    }

    //public override void OnTurnStart()
    //{
    //    base.OnTurnStart();
    //    refactorThisTurn = 0;
    //}

    public override void OnRefactor(StrategyBase strategy)
    {
        base.OnRefactor(strategy);
        if (IsAbnormal)
        {
            GameManager.Instance.GainMoney((int)keyValue3);
        }
        //refactorThisTurn = 1;

    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        if (!IsAbnormal)
        {
            if (GameRes.Coin < 100)
            {
                GameManager.Instance.GainMoney((int)KeyValue);
                GameRes.GainGoldBattleTurn += (int)KeyValue;
            }
        }
        //else
        //{
        //    if (refactorThisTurn <= 0)
        //    {
        //        GameManager.Instance.GainMoney((int)keyValue3);
        //        GameRes.GainGoldBattleTurn += ((int)keyValue3);
        //    }
        //}

    }

}

public class TechPerfect : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHPERFECT;
    public override float KeyValue => 5;
    public override float KeyValue2 => 1;
    private float keyValue3 => 3;
    public override string DisplayValue1 => KeyValue.ToString();
    public override string DisplayValue2 => KeyValue2.ToString();
    public override string DisplayValue3 => keyValue3.ToString();

    public override float SaveValue { get => times; set => times = (int)value; }
    private int times = 0;
    public override void OnGet()
    {
        base.OnGet();
        if (IsAbnormal)
            GameRes.LockCount -= (int)KeyValue2;
        //GameRes.RefreshShopCost += (int)KeyValue2;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (!IsAbnormal)
        {
            times++;
            if (times >= (int)KeyValue)
            {
                GameRes.PerfectElementCount++;
                times = 0;
            }
        }
    }

    public override void OnRefactor(StrategyBase strategy)
    {
        base.OnRefactor(strategy);
        if (IsAbnormal)
        {
            times++;
            if (times >= (int)keyValue3)
            {
                GameRes.PerfectElementCount++;
                times = 0;
            }
        }
        //else
        //{
        //    times = 0;
        //}

    }

    //public override void OnTurnEnd()
    //{
    //    base.OnTurnEnd();
    //    if (IsAbnormal)
    //    {
    //        times++;
    //        if (times >= (int)KeyValue)
    //        {
    //            GameRes.GainPerfectBattleTurn++;
    //            GameManager.Instance.GainPerfectElement(1);
    //            times = 0;
    //        }
    //    }

    //}


}

public class TechLegend : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHLEGEND;
    public override float KeyValue2 => 10;
    public override string DisplayValue2 => KeyValue2.ToString();


    public override bool OnGet2()
    {
        if (!IsAbnormal)
        {
            ConstructHelper.GetRefactorTurretByNameAndElement("BOUNTY", 99, 99, 99);
        }
        else
        {
            ConstructHelper.GetRefactorTurretByNameAndElement("TELEPORTOR", 99, 99, 99);
        }

        GameManager.Instance.TransitionToState(StateName.PickingState);
        return true;
    }

}

public class TechMap : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHMAP;
    public override float KeyValue => IsAbnormal ? m_Buff2.KeyValue : m_Buff.KeyValue;
    public override float KeyValue2 => IsAbnormal ? m_Buff2.KeyValue2 : m_Buff.KeyValue2;
    public override string DisplayValue1 => KeyValue * 100 + "%";
    public override string DisplayValue2 => KeyValue2 * 100 + "%";
    public override string DisplayValue3 => KeyValue * 100 + "%";

    private EnemyBuff m_Buff2;
    public override void InitializeTech()
    {
        base.InitializeTech();
        m_Buff = EnemyBuffFactory.GetBuff((int)EnemyBuffName.TinyMap);
        m_Buff2 = EnemyBuffFactory.GetBuff((int)EnemyBuffName.LongMap);
    }

    public override void OnGet()
    {
        if (!IsAbnormal)
        {
            m_EnemyBuffInfo = new BuffInfo(EnemyBuffName.TinyMap, 1);
        }
        else
        {
            m_EnemyBuffInfo = new BuffInfo(EnemyBuffName.LongMap, 1);
        }
        base.OnGet();
    }


}

public class TechTemp : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHTEMP;
    public override float KeyValue => 0.35f;
    public override float KeyValue2 => 0.25f;
    public override float KeyValue3 => 0.4f;


    public override string DisplayValue1 => KeyValue * 100 + "%";
    public override string DisplayValue2 => KeyValue2 * 100 + "%";
    public override string DisplayValue3 => KeyValue * 100 + "%";

    public override void OnGet()
    {
        base.OnGet();
        if (IsAbnormal)
        {
            GameRes.EnemyFrostResist -= KeyValue3;
            GameRes.TurretFrostResist -= KeyValue2;
        }
        else
        {
            GameRes.TurretFrostResist += KeyValue;

        }

    }

}

//public class TechESlot : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHESLOT;
//    public override float KeyValue => 1; //获得芯片数量
//    public override float KeyValue2 => 80;//第80波获得额外2个芯片

//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => "1";
//    public override string DisplayValue3 => KeyValue2.ToString();


//    public override bool OnGet2()
//    {
//        if (IsAbnormal)
//        {
//            GameRes.Life = 1;
//            if (GameRes.CurrentWave >= 80)
//                GameRes.GainSkillChip(2);
//            //GameRes.SkillChip += GameRes.CurrentWave / (int)KeyValue2;
//        }
//        else
//        {
//            GameRes.SkillChip += (int)KeyValue;
//        }
//        return false;
//    }


//    public override void OnTurnStart()
//    {
//        base.OnTurnStart();
//        if (IsAbnormal)
//        {
//            //if (GameRes.CurrentWave % (int)KeyValue2 == 0)
//            //    GameRes.GainSkillChip(1);
//            if (GameRes.CurrentWave == 80)
//                GameRes.GainSkillChip(2);
//        }
//    }


//}

//public class TechPrism : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHPRISM;
//    public override float KeyValue => 1f;
//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => "2";
//    public override string DisplayValue3 => "20";
//    public override string DisplayValue4 => "10";
//    public override bool OnGet2()
//    {
//        ConstructHelper.GetBuildingByName("PRISM", IsAbnormal);
//        GameManager.Instance.TransitionToState(StateName.PickingState);
//        return true;
//    }

//}

//public class TechTiny : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHTINY;
//    public override bool ContainGlobalBuff => true;

//    public override float KeyValue => m_Buff.KeyValue;
//    public override float KeyValue2 => m_Buff.KeyValue2;
//    public override string DisplayValue1 => KeyValue * 100 + "%";
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";
//    public override string DisplayValue3 => m_Buff.BasicDuration.ToString();

//    public override void InitializeTech()
//    {
//        base.InitializeTech();
//        m_Buff = EnemyBuffFactory.GetBuff((int)EnemyBuffName.TinyMap);
//        m_EnemyBuffInfo = new BuffInfo(EnemyBuffName.TinyMap, 1, IsAbnormal);

//    }

//}



//public class TechIceBreak : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHICEBREAK;
//    // public override bool ContainGlobalSkill => true;
//    public override float KeyValue => m_Skill.KeyValue;
//    public override float KeyValue2 => m_Skill.KeyValue2;
//    public override string DisplayValue1 => KeyValue * 100 + "%";
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";

//    public override void InitializeTech()
//    {
//        base.InitializeTech();
//        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.IceBreak, IsAbnormal);
//        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

//    }
//    public override void OnGet()
//    {
//        base.OnGet();
//        if (IsAbnormal)
//            GameRes.EnemyFrostAdjust += KeyValue2;
//    }


//}

//public class TechRhythm : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHRHYTHM;
//    public override float KeyValue => IsAbnormal ? 3 : 5;
//    public override float KeyValue2 => 1;
//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => KeyValue2.ToString();
//    public override string DisplayValue3 => KeyValue2.ToString();
//    private int noRefactorTurns;
//    public override float SaveValue { get => noRefactorTurns; set => noRefactorTurns = (int)value; }
//    public override void OnGet()
//    {
//        base.OnGet();
//        if (IsAbnormal)
//            GameRes.LockCount -= (int)KeyValue2;
//    }

//    public override void OnTurnEnd()
//    {
//        base.OnTurnEnd();
//        noRefactorTurns++;
//        if (noRefactorTurns >= KeyValue)
//        {
//            GameRes.GainPerfectBattleTurn++;
//            GameManager.Instance.GainPerfectElement(1);
//            noRefactorTurns = 0;
//        }
//    }

//    public override void OnRefactor()
//    {
//        base.OnRefactor();
//        noRefactorTurns = 0;
//    }
//}



//public class TechRecycle : Technology
//{
//    public override TechnologyName TechnologyName => TechnologyName.TECHRECYCLE;
//    public override float KeyValue => 1;
//    public override float KeyValue2 => IsAbnormal ? 0.12f : 0.08f;
//    public override string DisplayValue1 => KeyValue.ToString();
//    public override string DisplayValue2 => KeyValue2 * 100 + "%";


//    public override void OnRefactor()
//    {
//        base.OnRefactor();
//        GameRes.BuildCost = Mathf.RoundToInt(GameRes.BuildCost * (1 - KeyValue2));
//    }
//    public override void OnTurnEnd()
//    {
//        base.OnTurnEnd();
//        if (IsAbnormal)
//            GameManager.Instance.RemoveUnlockedRecipes();
//    }

//}



public class TechConstructor : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHCONSTRUCTOR;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Constructor;


    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2.ToString();
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ConstructorBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }

}
public class TechRapider : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHRAPIDER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Rapider;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RapiderBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}
public class TechScatter : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHSCATTER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Scatter;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";

    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ScatterBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechCoordinator : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHCOORDINATOR;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Coordinator;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";

    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.CoordinatorBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

    public override void OnGet()
    {
        base.OnGet();
        //if (!IsAbnormal)
        //    StrategyBase.CoordinatorMaxIntensify += m_Skill.KeyValue;
        //else
        if (IsAbnormal)
            StrategyBase.CoordinatorMaxIntensify += m_Skill.KeyValue3;
    }

}

public class TechRotary : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHROTARY;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Rotary;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RotaryBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}

public class TechSnow : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHSNOW;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Snow;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SnowBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechMortar : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHMORTAR;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Mortar;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.MortarBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechSniper : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHSNIPER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Sniper;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SniperBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechSuper : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHSUPER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Super;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.SuperBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechBoomerang : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHBOOMERANG;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Boomerrang;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.BoomerrangBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechUltra : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHULTRA;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Ultra;

    public override string DisplayValue1 => (m_Skill.KeyValue * 100).ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    //public override string DisplayValue4 => GameMultiLang.GetTraduction("BASICCRITBUFF");

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.UltraBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}

public class TechCore : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHCORE;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Core;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.CoreBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}



public class TechRadpid : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHRAPID;
    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.RapidBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechInstrument : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHINSTRUMENT;
    public override float KeyValue2 => 5;
    public override string DisplayValue2 => KeyValue2.ToString();


    public override bool OnGet2()
    {
        if (!IsAbnormal)
        {
            ConstructHelper.GetRefactorTurretByNameAndElement("PRISM", 99, 99, 99);
        }
        else
        {
            ConstructHelper.GetRefactorTurretByNameAndElement("AMPLIFIER", 99, 99, 99);
        }

        GameManager.Instance.TransitionToState(StateName.PickingState);
        return true;
    }

}

public class TechChiller : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHCHILLER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Chiller;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.ChillerBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}

public class TechFirer : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHFIRER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Firer;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.FirerBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}

public class TechLaser : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHLASER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Laser;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.LaserBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}


public class TechBombard : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHBOMBARD;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Bombard;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3.ToString();

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.BombardBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}

public class TechMiner : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHMINER;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Miner;

    public override string DisplayValue1 => m_Skill.KeyValue.ToString();
    public override string DisplayValue2 => m_Skill.KeyValue2.ToString();
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.MinerBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }

}

public class TechNuclear : Technology
{
    public override TechnologyName TechnologyName => TechnologyName.TECHNUCLEAR;
    public override RefactorTurretName RefactorBinding => RefactorTurretName.Nuclear;

    public override string DisplayValue1 => m_Skill.KeyValue * 100 + "%";
    public override string DisplayValue2 => m_Skill.KeyValue2 * 100 + "%";
    public override string DisplayValue3 => m_Skill.KeyValue3 * 100 + "%";

    public override void InitializeTech()
    {
        base.InitializeTech();
        m_SkillInfo = new GlobalSkillInfo(GlobalSkillName.NuclearBuff, IsAbnormal);
        m_Skill = TurretSkillFactory.GetGlobalSkill(m_SkillInfo);

    }
    public override void OnGet()
    {
        base.OnGet();
        if (!IsAbnormal)
            GameManager.Instance.TriggerDetectSkills();
    }
}









