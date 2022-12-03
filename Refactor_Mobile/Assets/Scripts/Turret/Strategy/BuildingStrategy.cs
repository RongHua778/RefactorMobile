using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStrategy : StrategyBase
{
    public bool IsAbnormalBuilding;
    public BuildingStrategy(TurretAttribute attribute, int quality, bool isAbnormal) : base(attribute, quality)
    {
        IsAbnormalBuilding = isAbnormal;
        GetTurretSkills();
    }

    public override void GetTurretSkills()//首次获取被动技能效果
    {
        //TurretSkills.Clear();

        //TurretSkill effect = TurretSkillFactory.GetBuidlingSkill((int)Attribute.BuildingSkill, IsAbnormalBuilding);//自带技能
        //TurretSkill = effect;
        //TurretSkill.strategy = this;
        //TurretSkills.Add(effect);
        //TurretSkill.Build();

    }


}
