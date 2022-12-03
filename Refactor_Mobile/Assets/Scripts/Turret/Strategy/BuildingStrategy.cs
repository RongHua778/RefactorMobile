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

    public override void GetTurretSkills()//�״λ�ȡ��������Ч��
    {
        //TurretSkills.Clear();

        //TurretSkill effect = TurretSkillFactory.GetBuidlingSkill((int)Attribute.BuildingSkill, IsAbnormalBuilding);//�Դ�����
        //TurretSkill = effect;
        //TurretSkill.strategy = this;
        //TurretSkills.Add(effect);
        //TurretSkill.Build();

    }


}
