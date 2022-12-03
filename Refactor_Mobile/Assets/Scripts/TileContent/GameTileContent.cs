using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, ElementTurret, RefactorTurret, Trap, TurretBase//, Building
}
public abstract class GameTileContent : ReusableObject
{
    public ContentStruct m_ContentStruct;
    public virtual bool IsWalkable { get => true; }
    public virtual GameTileContentType ContentType { get; }

    private GameTile m_gameTile;
    public GameTile m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual bool CanEquip => false;
    public bool IsSwitching { get; set; }

    public virtual void ContentLanded()//该content放在地上时触发
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = false;
        }
        LevelManager.Instance.GameSaveContents.Add(this);
    }

    public virtual void OnContentSelected(bool value)
    {

    }

    protected virtual void ContentLandedCheck(Collider2D col)//根据下方已有坚固格的类型决定自己的行为
    {

    }

    public virtual void CorretRotation()
    {

    }

    public virtual void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
    {

    }

    public virtual void SaveContent(out ContentStruct contentSruct)//放置下的时候，进入保存LIST
    {
        m_ContentStruct = new ContentStruct();
        contentSruct = m_ContentStruct;
        m_ContentStruct.ContentType = (int)ContentType;
        m_ContentStruct.Pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        m_ContentStruct.Direction = (int)m_gameTile.TileDirection;

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        IsSwitching = false;
        LevelManager.Instance.GameSaveContents.Remove(this);
    }

    public virtual void OnSwitch()
    {
        StaticData.SetNodeWalkable(m_GameTile, false, true);
        IsSwitching = true;
        m_GameTile.IsLanded = false;

        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.TempGroundMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = true;//元素塔可能被回收，回收时，将地板TILE设为上方没有物体
        }


    }

    public virtual void OnUndo()
    {
        if (IsSwitching)
        {
            UndoSwitching();
        }
        else
        {
            UndoUnSwitching();
        }

    }

    protected virtual void UndoSwitching()
    {
        GameTileContent emptyContent = StaticData.Instance.ContentFactory.GetBasicContent(GameTileContentType.Empty);
        m_GameTile.Content = emptyContent;
        GameTile tile = ConstructHelper.GetTileWithContent(this);
        tile.transform.position = (Vector3Int)GameRes.SwitchInfo.InitPos;
        tile.SetRotation(GameRes.SwitchInfo.InitDir);
        tile.TileLanded();
        BoardSystem.SelectingTile = m_GameTile;
        GameRes.Coin += GameRes.SwitchInfo.SwitchSpend;//直接加金币，因为不涉及加成

        GameRes.SwitchInfo = null;
        GameManager.Instance.TransitionToState(StateName.BuildingState);
    }

    protected virtual void UndoUnSwitching()
    {
        GameManager.Instance.TransitionToState(StateName.BuildingState);

    }



}
