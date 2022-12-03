using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechState : BattleOperationState
{
    public TechState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.TechState;

    public override IEnumerator EnterState()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.ShowTechSelect(true);
        Sound.Instance.PlayUISound("Sound_Tips");
        Sound.Instance.PlayBg("Music_Preparing");
        yield return null;
    }


}
