using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private Dictionary<StateID, FSMState> States = new Dictionary<StateID, FSMState>();
    private StateID CurrentStateID;
    private FSMState CurrentState;

    public void Update(AirAttacker agent)
    {
        if (!agent.DamageStrategy.IsDie)
        {
            CurrentState.Act(agent);
            CurrentState.Reason(agent);
        }
    }

    public void Update(AirProtector agent)
    {
        if (!agent.DamageStrategy.IsDie)
        {
            CurrentState.Act(agent);
            CurrentState.Reason(agent);
        }
    }

    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("stateΪ��");
            return;
        }

        if (CurrentState == null)
        {
            CurrentState = s;
            CurrentStateID = s.StateID;
        }

        if (States.ContainsKey(s.StateID))
        {
            Debug.LogError("״̬" + s.StateID + "�Ѿ����ڣ��޷��ظ����");
            return;
        }

        States.Add(s.StateID, s);
    }

    public void DeleteFSMState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("�޷�ɾ����״̬");
            return;
        }

        if (States.ContainsKey(id) == false)
        {
            Debug.LogError("�޷�ɾ��������״̬" + id);
            return;
        }

        States.Remove(id);
    }

    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("�޷�ִ�пյ�ת������");
            return;
        }
        StateID id = CurrentState.GetStateID(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogWarning("��ǰ״̬" + CurrentStateID + "�޷�����ת������" + trans + "����ת��");
            return;
        }

        if (States.ContainsKey(id) == false)
        {
            Debug.LogError("״̬����û�а���" + id + "���޷�����״̬ת��");
            return;
        }

        FSMState state = States[id];
        CurrentState.DoAfterLeaving();
        CurrentState = state;
        CurrentStateID = id;
        CurrentState.DoBeforeEntering();
    }
}