using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRefactor : RefactorTurret
{
    protected override void UndoUnSwitching()
    {
        base.UndoUnSwitching();
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)//撤回选项
        {
            TechSelectPanel.PlacingChoice = false;
            ObjectPool.Instance.UnSpawn(this);
            GameManager.Instance.ShowChoices(true, false, false);
        }
        else//撤回重构
        {
            Strategy.UndoStrategy();
        }

    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
        {
            GameManager.Instance.ConfirmChoice();
            TechSelectPanel.PlacingChoice = false;
        }

        base.ContentLandedCheck(col);
    }
}
