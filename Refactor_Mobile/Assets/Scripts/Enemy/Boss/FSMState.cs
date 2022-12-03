using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition//ת������
{
    NullTransition = 0,
    AttackTarget,
    LureTarget,
    Attacked,
    BackToBoss,
    ProtectBoss,
    Patrol
}

public enum StateID//״̬id
{
    NullStateID = 0,
    Patrol,
    Track,
    Back,
    Lure,
    Protect
}

//��Ϊ���౻������̳е����޻�״̬
public abstract class FSMState
{
    private StateID stateID;
    protected FSMSystem fsm;
    public StateID StateID { get => stateID; set => stateID = value; }

    public FSMState(FSMSystem fsm)
    {
        this.fsm = fsm;
    }


    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();


    public void AddTransition(Transition trans, StateID id)//���״̬
    {
        //��ȫУ��
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("������NullTransition");
            return;
        }
        if (id == StateID.NullStateID)
        {
            Debug.LogError("������NullStateID");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("���ת������ʱ��" + trans + "�Ѿ�������map��");
            return;
        }
        //У��ͨ�������map
        map.Add(trans, id);

    }

    public void DeleteTransition(Transition trans)//ɾ��״̬
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("������NullTransition");
            return;
        }
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogError("���ת������ʱ��" + trans + "��������map��");
            return;
        }

        map.Remove(trans);
    }

    public StateID GetStateID(Transition trans)//�õ���ǰ״̬
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }

        return StateID.NullStateID;
    }


    public virtual void DoBeforeEntering() { }
    public virtual void DoAfterLeaving() { }
    public virtual void Act(Aircraft agent) { }
    public virtual void Reason(Aircraft agent) { }//�ж�ת������
    //public virtual void Act(AirProtector agent) { }
    //public virtual void Reason(AirProtector agent) { }//�ж�ת������

}