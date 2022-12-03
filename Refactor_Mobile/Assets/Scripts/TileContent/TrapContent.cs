using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrapContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Trap;
    public List<Enemy> BlinkedEnemy = new List<Enemy>();
    private TrapAttribute trapAttribute;
    public TrapAttribute TrapAttribute { get => trapAttribute; }

    public bool needReset = false;//是否需要重置朝向

    public int CoinGainThisTurn = 0;

    long damageAnalysis;
    public virtual long DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    int coinAnalysis;
    public virtual int CoinAnalysis { get => coinAnalysis; set => coinAnalysis = value; }

    public virtual int DieProtect { get => -1; }

    float trapIntensify = 0;
    [ShowInInspector]
    public float TrapIntensify { get => trapIntensify; set => trapIntensify = value; }

    //美术设置
    private bool isReveal = false;
    public bool IsReveal { get => isReveal; set => isReveal = value; }

    [HideInInspector] public bool Important;
    private SpriteRenderer trapGFX;
    private Sprite initSprite;
    private Sprite unrevealSprite;


    public void Initialize(TrapAttribute att)
    {
        trapAttribute = att;
    }
    protected virtual void Awake()
    {
        trapGFX = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        initSprite = trapGFX.sprite;
        unrevealSprite = StaticData.Instance.UnrevealTrap;
        trapGFX.sprite = unrevealSprite;
    }
    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = StaticData.UndropablePoint;
        StaticData.SetNodeWalkable(m_GameTile, true);
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);

    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        contentStruct.TrapRevealed = IsReveal;
        m_ContentStruct.ContentName = TrapAttribute.Name;

    }


    public override void CorretRotation()
    {
        base.CorretRotation();
        if (needReset)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {
        base.OnContentPass(enemy);
        enemy.PassedTraps.Add(content == null ? this : (TrapContent)content);//复制闪烁陷阱
    }


    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        if (value)
        {
            TipsManager.Instance.ShowTrapContentTips(this, StaticData.LeftTipsPos);

        }

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        DamageAnalysis = 0;
    }


    protected override void ContentLandedCheck(Collider2D col)
    {
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
        {
            GameManager.Instance.ConfirmChoice();
            TechSelectPanel.PlacingChoice = false;
        }

        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
        IsSwitching = false;
    }

    public void RevealTrap()//揭示陷阱
    {
        if (!IsReveal)
        {
            if (!needReset)
                (m_GameTile).SetRandomRotation();
            trapGFX.sprite = initSprite;
            IsReveal = true;

            GameRes.MaxMark++;
        }
    }

    public virtual void ClearTurnData()
    {
        CoinGainThisTurn = 0;
        BlinkedEnemy.Clear();
    }

    public override void OnSwitch()
    {
        base.OnSwitch();

        GameRes.SwitchTrapCost += StaticData.Instance.SwitchTrapCostMultiply;
        GameTile tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
        tile.transform.position = transform.position;
        tile.TileLanded();
    }



    protected override void UndoSwitching()
    {
        base.UndoSwitching();

        GameRes.SwitchTrapCost -= StaticData.Instance.SwitchTrapCostMultiply;
        TipsManager.Instance.ShowTrapContentTips(this, StaticData.LeftTipsPos);

    }

    protected override void UndoUnSwitching()
    {
        base.UndoUnSwitching();
        GameManager.Instance.ShowChoices(true, false, false);
        ObjectPool.Instance.UnSpawn(this);
    }

}
