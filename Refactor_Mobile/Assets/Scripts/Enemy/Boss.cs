using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public override string ExplosionSound => "Sound_BossExplosion";
    protected virtual bool ShakeCam => true;
    public override int MaxAmount => 20;
    public override void OnDie()
    {
        base.OnDie();
        if (ShakeCam)
            GameManager.Instance.ShakeCam();
    }

    protected void ShowBossText(float chance)
    {
        HealthBar.ShowBossTxt(m_Att,chance);
    }


}
