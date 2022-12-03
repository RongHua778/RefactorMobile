using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorStrategy : RefactorStrategy
{


    public ConstructorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
    {
    }

    //public int ConstructorEslot => GameRes.ConstructorExtraSlot > 0 ? 1 : 0;
    //public override int TempExtraSlot { get => Mathf.Max(base.TempExtraSlot, ConstructorEslot); }


    //public override void OnEquipped()
    //{
    //    if (TurretSkills.Count > 5) //5������
    //    {
    //        PrivateExtraSlot++;//��õ�5�����⼼�ܲ�
    //        GameRes.ConstructorExtraSlot++;//�ָ�1����ũ��4��������
    //        GameRes.GlobalExtraSlot--;//����5��������
    //    }
    //    else
    //    if (TurretSkills.Count > 4) //4������
    //    {
    //        if (GameRes.ConstructorExtraSlot > 0)//�������ļ�ũ�ڼ���
    //        {
    //            GameRes.ConstructorExtraSlot--;//����1����ũ������
    //            PrivateExtraSlot++;
    //        }
    //        else if (GameRes.GlobalExtraSlot > 0)//û�м�ũ���������ȫ������
    //        {
    //            GameRes.GlobalExtraSlot--;
    //            PrivateExtraSlot+= GameRes.GlobalExtraSlotValue;//���1��2�����⼼�ܲ�
    //        }
    //    }

    //}


}
