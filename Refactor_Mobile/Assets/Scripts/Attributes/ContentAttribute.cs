using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttType
{
    Turret,
    Mark,
    Enemy,
    Technology
}

public class ContentAttribute : ScriptableObject
{
    public AttType AttType;
    public string Name;
    public Sprite Icon;
    public ReusableObject Prefab;
    public bool isLock;
    public bool initialLock;

    //public virtual void ContentShowTips(Vector2 pos)
    //{

    //}
    //public virtual void MenuShowTips(Vector2 pos, StrategyBase strategy = null)
    //{
    //}
    //public virtual void GameShowTips(Vector2 pos, StrategyBase strategy = null)
    //{
    //}

}
