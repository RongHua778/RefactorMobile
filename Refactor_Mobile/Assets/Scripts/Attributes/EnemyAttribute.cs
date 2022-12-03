using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ContentAttribute
{
    public EnemyType EnemyType;
    public bool IsBoss;
    public int InitCount;
    public float CountIncrease;
    public int MaxAmount;
    public float Health;
    public float Speed;
    public float CoolDown;
    public float Frost;
    public string BackGround;
    [Header("Tips²ÎÊý")]
    public int HealthAtt;
    public int SpeedAtt;
    public int AmountAtt;
    public int ReachDamage;
    [Header("Boss¶Ô°×")]
    public string[] BossDialogues;

    //public override void MenuShowTips(Vector2 pos, StrategyBase strategy = null)
    //{
    //    base.MenuShowTips(pos);
    //    MenuManager.Instance.ShowEnemyInfoTips(this, pos);
    //}
}
