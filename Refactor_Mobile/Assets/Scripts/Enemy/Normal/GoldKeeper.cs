using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeeper : Boss
{
    public override EnemyType EnemyType => EnemyType.GoldKeeper;
    public override int MaxAmount => 1;
    protected override bool ShakeCam => false;


    protected override void SetStrategy()
    {
        DamageStrategy = new GoldKeeperStrategy(this);
    }
    //protected override void OnEnemyUpdate()
    //{
    //    base.OnEnemyUpdate();
    //    if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 1 - 0.05f * LifeCount)
    //    {
    //        GainMoney();
    //    }
    //}

    //private void GainMoney()
    //{
    //    //int gainCount = (int)((DamageStrategy.MaxHealth * (1 - 0.05f * LifeCount) - DamageStrategy.CurrentHealth) / 0.05f);
    //    LifeCount ++;
    //    StaticData.Instance.GainMoneyEffect(model.position, Mathf.Min(30, Mathf.RoundToInt(GameRes.CurrentWave * 1.2f)));
    //}

    //public override void OnDie()
    //{
    //    GameRes.GainPerfectBattleTurn++;
    //    StaticData.Instance.GainPerfectEffect(model.position, 1);
    //    base.OnDie();
    //}

}
