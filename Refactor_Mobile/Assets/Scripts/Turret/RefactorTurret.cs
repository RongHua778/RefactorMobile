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
        GameManager.Instance.refactorTurrets.Add(this);//这个放上面避免装备元素技能后，回收该塔以后又加入list
        m_GameTile.tag = StaticData.OnlyRefactorTag;//这个放上面，避免装备元素技能后，把Gametile改了;导致一个被回收的gametile为OnlyRefactorTag.
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

        for (int i = 1; i < Strategy.TurretSkills.Count; i++)//第2个开始都是元素技能
        {
            ElementSkill skill = Strategy.TurretSkills[i] as ElementSkill;
            m_ContentStruct.SkillList.Add(i.ToString(), skill.InitElements);//Litjson存储Key必须为String
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
            if (tile.Content.CanEquip)//装备元素技能
            {
                ConcreteContent turret = tile.Content as ConcreteContent;

                for (int i = 1; i < this.Strategy.TurretSkills.Count; i++)
                {
                    ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[i];
                    turret.Strategy.AddElementSkill(skill);
                    skill.OnEquip();//装备技能触发装备效果
                }

                //ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[1];
                //turret.Strategy.AddElementSkill(skill);
                //skill.OnEquip();//装备技能触发装备效果

                ((BasicTile)turret.m_GameTile).EquipTurret(turret.Strategy.TurretSkills.Count - 1);
                BoardSystem.PreviewEquipTile = null;

                //TechnologySystem.OnEquip(turret.Strategy);
                ObjectPool.Instance.UnSpawn(m_GameTile);
                TechnologySystem.OnRefactor(turret.Strategy);
                return;//不增加总放置重构塔数
            }
            else//正常部署
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

    protected override void UndoUnSwitching()//非换位重置
    {
        base.UndoUnSwitching();
    }


}
