using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreStrategy : RefactorStrategy
{
    public CoreStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }

    //public int CoreExtraSlot => GameRes.CoreExtraSlot > 0 ? 1 : 0;
    //public override int TempExtraSlot { get => Mathf.Max(base.TempExtraSlot, CoreExtraSlot); }


    //public override void OnEquipped()
    //{
    //    if (TurretSkills.Count > 5) //5������
    //    {
    //        PrivateExtraSlot++;//��õ�5�����⼼�ܲ�
    //        GameRes.CoreExtraSlot++;//�ָ�1�����Ļ�4��������
    //        GameRes.GlobalExtraSlot--;//����5��������
    //    }
    //    else
    //    if (TurretSkills.Count > 4) //4������
    //    {
    //        if (GameRes.CoreExtraSlot > 0)//�������ĺ��Ļ�����
    //        {
    //            GameRes.CoreExtraSlot--;//����1�����Ļ�����
    //            PrivateExtraSlot++;
    //        }
    //        else if (GameRes.GlobalExtraSlot > 0)//û�к��Ļ��������ȫ������
    //        {
    //            GameRes.GlobalExtraSlot--;
    //            PrivateExtraSlot += GameRes.GlobalExtraSlotValue;//���1��2�����⼼�ܲ�
    //        }
    //    }

    //}

}
