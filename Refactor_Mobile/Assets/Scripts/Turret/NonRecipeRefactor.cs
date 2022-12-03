using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonRecipeRefactor : RefactorTurret
{
    public override bool CanEquip => false;
    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = StaticData.UndropablePoint;

    }
    protected override void UndoUnSwitching()
    {
        //GameManager.Instance.ConfirmTechSelect();
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)//³·»ØÑ¡Ïî
        {
            TechSelectPanel.PlacingChoice = false;
            ObjectPool.Instance.UnSpawn(this);
            GameManager.Instance.ShowChoices(true, false, false);
        }
        else
        {
            GameManager.Instance.ShowTechSelect(true, false);
            GameManager.Instance.RemoveTech(TechSelectPanel.SelectingTech);
            TechSelectPanel.SelectingTech = null;
        }
        base.UndoUnSwitching();

    }
    protected override void ContentLandedCheck(Collider2D col)
    {
        if (!IsSwitching)
            GameManager.Instance.ConfirmTechSelect();
        base.ContentLandedCheck(col);
    }
}
