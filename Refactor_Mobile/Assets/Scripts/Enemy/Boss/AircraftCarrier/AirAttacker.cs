using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttacker : Aircraft
{
    [SerializeField] protected float freezeTime;
    [SerializeField] protected float freezeRange;
    [SerializeField] bool isSuperAircraft = default;
    public override void Initiate(AircraftCarrier boss, float maxHealth, float dmgIntenWhenDie,float dmgResist)
    {
        base.Initiate(boss, maxHealth, dmgIntenWhenDie,dmgResist);
        freezeTime = Mathf.Min(12f, 6f + 0.5f * ((GameRes.CurrentWave + 1) / 20));
        freezeRange = isSuperAircraft ? 
            Mathf.Min(6f, 1.5f + 0.25f * ((GameRes.CurrentWave + 1) / 20)) : 0.5f;

        if (fsm == null)
        {
            this.boss = boss;
            //以下是状态机的初始化
            fsm = new FSMSystem();

            FSMState patrolState = new AirAttackerPatrolState(fsm);
            patrolState.AddTransition(Transition.AttackTarget, StateID.Track);
            patrolState.AddTransition(Transition.LureTarget, StateID.Lure);
            patrolState.AddTransition(Transition.ProtectBoss, StateID.Protect);
            PickRandomDes();

            FSMState trackState = new TrackState(fsm);
            trackState.AddTransition(Transition.Attacked, StateID.Back);
            trackState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            FSMState lureState = new AirAttackerLureState(fsm);
            lureState.AddTransition(Transition.Attacked, StateID.Back);
            lureState.AddTransition(Transition.ProtectBoss, StateID.Protect);

            FSMState backState = new BackState(fsm);
            backState.AddTransition(Transition.ProtectBoss, StateID.Protect);
            backState.AddTransition(Transition.Patrol, StateID.Patrol);


            FSMState protectState = new ProtectState(fsm);
            protectState.AddTransition(Transition.Patrol, StateID.Patrol);
            protectState.AddTransition(Transition.ProtectBoss, StateID.Protect);


            fsm.AddState(patrolState);
            fsm.AddState(trackState);
            fsm.AddState(backState);
            fsm.AddState(lureState);
            fsm.AddState(protectState);
        }
    }
    public override bool GameUpdate()
    {
        fsm.Update(this);
        return base.GameUpdate();
    }

    public override void Attack()
    {
        if (targetTurret.Activated)
        {
            StaticData.Instance.FrostTurretEffect(targetTurret.transform.position, freezeRange, freezeTime);

        }
        targetTurret = null;

    }



}
