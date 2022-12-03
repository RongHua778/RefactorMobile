using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill 
{
    protected Enemy enemy;


    //��enemy������ʱ�����
    public virtual void OnBorn()
    {

    }

    //��enemy��gameupdate�е���
    public virtual void OnGameUpdating()
    {

    }

    //enemy����ʱ����
    public virtual void OnDying()
    {

    }
}
