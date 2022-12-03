using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffInfo
{
    public EnemyBuffName EnemyBuffName;
    public int Stacks;
    public bool IsAbnormal;
    public BuffInfo(EnemyBuffName name, int stacks,bool isAbormal=false)
    {
        this.EnemyBuffName = name;
        this.Stacks = stacks;
        this.IsAbnormal = isAbormal;
    }
}

[CreateAssetMenu(menuName = "Attribute/TrapAttribute", fileName = "TrapAttribute")]
public class TrapAttribute : ContentAttribute
{
    //public override void MenuShowTips(Vector2 pos, StrategyBase strategy = null)
    //{
    //    base.MenuShowTips(pos);
    //    MenuManager.Instance.ShowTrapTips(this, pos);
    //}

    //public override void GameShowTips(Vector2 pos, StrategyBase strategy = null)
    //{
    //    base.GameShowTips(pos);
    //    GameManager.Instance.ShowTrapTips(this, pos);
    //}
}


