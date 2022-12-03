using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContent : RefactorTurret
{
    //public override GameTileContentType ContentType => GameTileContentType.Building;

    public override bool CanEquip => false;
    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = StaticData.UndropablePoint;

    }
    //public virtual void InitializeBuilding(BuildingStrategy strategy)
    //{
    //    Strategy = strategy;
    //    this.Strategy.Concrete = this;
    //    GenerateRange();
    //}


    //public override void OnSwitch()
    //{
    //    base.OnSwitch();
    //    GameManager.Instance.Buildings.Remove(this);
    //}
    //public override void ContentLanded()
    //{
    //    m_GameTile.tag = StaticData.OnlyRefactorTag;//这个放上面，避免装备元素技能后，把Gametile改了;导致一个被回收的gametile为OnlyRefactorTag.
    //    GameManager.Instance.Buildings.Add(this);
    //    base.ContentLanded();

    //}

    //public override void OnContentSelected(bool value)
    //{
    //    base.OnContentSelected(value);
    //    if (value)
    //    {
    //        TipsManager.Instance.ShowTurreTips(this.Strategy, StaticData.LeftTipsPos, 0);
    //    }
    //}

    //public override void SaveContent(out ContentStruct contentStruct)
    //{
    //    base.SaveContent(out contentStruct);
    //    contentStruct = m_ContentStruct;
    //    m_ContentStruct.ContentName = Strategy.Attribute.Name;
    //    BuildingSkill skill = Strategy.TurretSkills[0] as BuildingSkill;
    //    m_ContentStruct.IsAbnormalBuilding = skill.IsAbnormalBuilding;

    //}


    //protected override void ContentLandedCheck(Collider2D col)
    //{
    //    ShowLandedEffect();
    //    if (!IsSwitching)
    //        GameManager.Instance.ConfirmTechSelect();
    //    if (col != null)
    //    {
    //        GameTile tile = col.GetComponent<GameTile>();
    //        if (tile.Content.CanEquip)//装备元素技能
    //        {
    //            ConcreteContent turret = tile.Content as ConcreteContent;
    //            for (int i = 1; i < Strategy.TurretSkills.Count; i++)
    //            {
    //                if (turret.Strategy.TurretSkills.Count < turret.Strategy.ElementSKillSlot + 1)
    //                {
    //                    ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[i];
    //                    turret.Strategy.AddElementSkill(skill);
    //                    skill.OnEquip();//装备技能触发装备效果
    //                }
    //                else
    //                {
    //                    break;
    //                }
    //            }
    //            //turret.Strategy.OnEquipped();

    //            ((BasicTile)turret.m_GameTile).EquipTurret(turret.Strategy.TurretSkills.Count - 1);
    //            BoardSystem.PreviewEquipTile = null;
    //            //TechnologySystem.OnEquip(turret.Strategy);
    //            ObjectPool.Instance.UnSpawn(m_GameTile);
    //            return;//不增加总放置重构塔数
    //        }
    //        else//正常部署
    //        {
    //            ObjectPool.Instance.UnSpawn(tile);
    //        }
    //    }
    //    ((BasicTile)m_GameTile).EquipTurret(Strategy.TurretSkills.Count - 1);
    //    IsSwitching = false;
    //}

    //public void ShowLandedEffect()
    //{
    //    ReusableObject partical = ObjectPool.Instance.Spawn(StaticData.Instance.LandedEffect);
    //    partical.transform.position = transform.position + Vector3.up * 0.2f;
    //    Sound.Instance.PlayEffect("Sound_Landed");

    //}
    //public override void OnUnSpawn()
    //{
    //    base.OnUnSpawn();
    //    GameManager.Instance.Buildings.Remove(this);

    //}

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
        {
            GameManager.Instance.ConfirmChoice();
            TechSelectPanel.PlacingChoice = false;
        }
        else
        {
            if (!IsSwitching)
                GameManager.Instance.ConfirmTechSelect();
        }
        base.ContentLandedCheck(col);
    }
    protected override void UndoUnSwitching()
    {
        //GameManager.Instance.ConfirmTechSelect();

        GameManager.Instance.ShowTechSelect(true, false);
        GameManager.Instance.RemoveTech(TechSelectPanel.SelectingTech);
        TechSelectPanel.SelectingTech = null;
        base.UndoUnSwitching();
    }

}
