using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RuleName
{
    RULE_MICRO,
    RULE_COLD,
    RULE_TRAP,
    RULE_AMOUNT,
    RULE_REVERSE,
    RULE_STRIKE,
    RULE_HEAVY,
    RULE_MACRO,
    RULE_RICH,
    RULE_FROST,
    RULE_AIRBORNE,
    RULE_INCREASE,
    RULE_DECREASE,
    RULE_FAST,
    RULE_FORBID,
    RULE_RESTORE
}

public abstract class Rule
{
    public abstract RuleName RuleName { get; }
    public string Description => GameMultiLang.GetTraduction(RuleName.ToString());

    public virtual void OnGameInit()
    {

    }

    public virtual void OnGameLoad()
    {

    }
}

public class MicroRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_MICRO;
    public override void OnGameInit()
    {
        GameRes.GroundSize = 15;
    }

    public override void OnGameLoad()
    {

    }
}

public class MacroRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_MACRO;
    public override void OnGameInit()
    {
        GameRes.GroundSize = 35;
    }

    public override void OnGameLoad()
    {

    }
}

public class ColdRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_COLD;
    public override void OnGameInit()
    {
        GameRes.EnemyFrostResist -= 1f;
        GameRes.TurretFrostResist -= 1f;
    }

    public override void OnGameLoad()
    {
    }
}

public class TrapRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_TRAP;
    public override void OnGameInit()
    {
        GameRes.TrapDistanceAdjust += 1;
        //GameRes.DamageAdjust -= 0.5f;
    }

    public override void OnGameLoad()
    {
    }
}

public class AmountRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_AMOUNT;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleAmountBuff, 1));
    }

    public override void OnGameLoad()
    {
    }
}

public class ReverseRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_REVERSE;
    public override void OnGameInit()
    {
        GameRes.Reverse = true;
    }

    public override void OnGameLoad()
    {
    }
}

public class StrikeRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_STRIKE;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleStrikeBuff, 1));
        //TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.Firerate, false));
    }

}

public class HeavyRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_HEAVY;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleLowStrikeBuff, 1));

    }

    public override void OnGameLoad()
    {
    }
}

public class RichRule : Rule
{
    public override RuleName RuleName => RuleName.RULE_RICH;
    public override void OnGameInit()
    {
        GameRes.CoinAdjust += 1f;
    }

    public override void OnGameLoad()
    {

    }
}

public class RuleFrost : Rule
{
    public override RuleName RuleName => RuleName.RULE_FROST;
    public override void OnGameInit()
    {
        TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.RuleFrostBuff, false));
    }


}

public class RuleAirborne : Rule
{
    public override RuleName RuleName => RuleName.RULE_AIRBORNE;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleAirborneBuff, 1));

    }


}

public class RuleIncrease : Rule
{
    public override RuleName RuleName => RuleName.RULE_INCREASE;
    public override void OnGameInit()
    {
        GameRes.IntentLineID = 2;
        //GameRes.EnemyIntensifyAdjust -= 0.5f;
    }


}

public class RuleDecrease : Rule
{
    public override RuleName RuleName => RuleName.RULE_DECREASE;
    public override void OnGameInit()
    {
        GameRes.IntentLineID = 1;

        //GameRes.EnemyAmoundAdjust -= 0.5f;
    }


}

public class RuleFast : Rule
{
    public override RuleName RuleName => RuleName.RULE_FAST;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleFastBuff, 1));

    }


}

public class RuleForbid : Rule
{
    public override RuleName RuleName => RuleName.RULE_FORBID;
    public override void OnGameInit()
    {
        TurretSkillFactory.AddGlobalSkill(new GlobalSkillInfo(GlobalSkillName.FirerateLimit, false));

    }


}

public class RuleResist : Rule
{
    public override RuleName RuleName => RuleName.RULE_RESTORE;
    public override void OnGameInit()
    {
        EnemyBuffFactory.GlobalBuffs.Add(new BuffInfo(EnemyBuffName.RuleRestoreBuff, 1));

    }


}

