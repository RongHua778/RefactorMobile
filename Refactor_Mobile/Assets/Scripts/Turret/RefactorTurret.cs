using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.RefactorTurret;

    public override bool CanEquip => true;

    public override void OnSwitch()
    {
        base.OnSwitch();
        GameManager.Instance.refactorTurrets.Remove(this);
    }

    public override void ContentLanded()
    {
        GameManager.Instance.refactorTurrets.Add(this);//������������װ��Ԫ�ؼ��ܺ󣬻��ո����Ժ��ּ���list
        m_GameTile.tag = StaticData.OnlyRefactorTag;//��������棬����װ��Ԫ�ؼ��ܺ󣬰�Gametile����;����һ�������յ�gametileΪOnlyRefactorTag.
        base.ContentLanded();
    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        m_ContentStruct.ContentName = Strategy.Attribute.Name;
        m_ContentStruct.Quality = Strategy.Quality;
        m_ContentStruct.ExtraSlot = Strategy.PrivateExtraSlot;
        m_ContentStruct.SkillList = new Dictionary<string, List<int>>();
        m_ContentStruct.ElementsList = new Dictionary<string, List<int>>();
        m_ContentStruct.IsException = new Dictionary<string, bool>();

        for (int i = 1; i < Strategy.TurretSkills.Count; i++)//��2����ʼ����Ԫ�ؼ���
        {
            ElementSkill skill = Strategy.TurretSkills[i] as ElementSkill;
            m_ContentStruct.SkillList.Add(i.ToString(), skill.InitElements);//Litjson�洢Key����ΪString
            m_ContentStruct.ElementsList.Add(i.ToString(), skill.Elements);
            m_ContentStruct.IsException.Add(i.ToString(), skill.IsException);
        }
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        ShowLandedEffect();

        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile.Content.CanEquip)//װ��Ԫ�ؼ���
            {
                ConcreteContent turret = tile.Content as ConcreteContent;

                for (int i = 1; i < this.Strategy.TurretSkills.Count; i++)
                {
                    ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[i];
                    turret.Strategy.AddElementSkill(skill);
                    skill.OnEquip();//װ�����ܴ���װ��Ч��
                }

                //ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[1];
                //turret.Strategy.AddElementSkill(skill);
                //skill.OnEquip();//װ�����ܴ���װ��Ч��

                ((BasicTile)turret.m_GameTile).EquipTurret(turret.Strategy.TurretSkills.Count - 1);
                BoardSystem.PreviewEquipTile = null;

                //TechnologySystem.OnEquip(turret.Strategy);
                ObjectPool.Instance.UnSpawn(m_GameTile);
                TechnologySystem.OnRefactor(turret.Strategy);
                return;//�������ܷ����ع�����
            }
            else//��������
            {
                ObjectPool.Instance.UnSpawn(tile);
            }
        }
        if (!IsSwitching)
        {
            GameRes.TotalLandedRefactor++;
            TechnologySystem.OnRefactor(this.Strategy);
        }
        ((BasicTile)m_GameTile).EquipTurret(Strategy.TurretSkills.Count - 1);
        IsSwitching = false;
    }



    public void ShowLandedEffect()
    {
        ReusableObject partical = ObjectPool.Instance.Spawn(StaticData.Instance.LandedEffect);
        partical.transform.position = transform.position + Vector3.up * 0.2f;
        Sound.Instance.PlayEffect("Sound_Landed");

    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        GameManager.Instance.refactorTurrets.Remove(this);

    }

    protected override void UndoUnSwitching()//�ǻ�λ����
    {
        base.UndoUnSwitching();
    }


}
