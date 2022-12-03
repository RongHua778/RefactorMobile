using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBullet : PenetrateBullet
{
    public bool TechSkillUnlocked { get; set; }
    private const float techSkillIncreaseRate = 0.25f;
    public override bool GameUpdate()
    {
        SizeChange();
        return DistanceCheck(TargetPos);
    }

    private void SizeChange()
    {
        if (TechSkillUnlocked)
        {
            this.BulletDamageIntensify += techSkillIncreaseRate * Time.deltaTime;
        }
    }
}

