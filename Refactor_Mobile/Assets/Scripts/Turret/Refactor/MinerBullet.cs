using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBullet : GroundBullet
{
    [SerializeField] private Mine mine;
    public BasicTile TargetTile { get; set; }
    public bool IsDmgIncrease { get; set; }

    public override void TriggerDamage()
    {
        Mine newMine = ObjectPool.Instance.Spawn(mine) as Mine;
        newMine.IsDmgIncrease = IsDmgIncrease;
        newMine.transform.position = this.TargetPos;
        newMine.SetAtt(this);
        ReclaimBullet();
    }

}
