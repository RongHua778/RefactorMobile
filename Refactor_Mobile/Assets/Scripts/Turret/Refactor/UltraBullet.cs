using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraBullet : GroundBullet
{
    [SerializeField] UltraLava UltraLava = default;
    public float DmgBonusIncreasePersecond { get; set; }

    public override void TriggerDamage()
    {
        base.TriggerDamage();
        UltraLava lava = ObjectPool.Instance.Spawn(UltraLava) as UltraLava;
        lava.DmgBonusIncreasePersecond = DmgBonusIncreasePersecond;
        //lava.Initialize(this.turretParent, this.Target);
        lava.transform.position = this.TargetPos;
        lava.SetAtt(this);
        lava.BulletLastTime = 5f;
    }

}
