using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameTile : TileBase
{
    private GameTileContent content;

    public GameTileContent Content
    {
        get => content;
        set => content = value;
    }

    GameObject previewGlow;
    Transform tileBase;
    [SerializeField] GameObject hightLightEffect = default;

    public bool isWalkable { get => Content.IsWalkable; }
    public DraggingShape m_DraggingShape { get; set; }
    public List<SpriteRenderer> TileRenderers { get; set; }
    public SpriteRenderer PreviewRenderer { get; set; }
    public Vector3 ExitPoint { get; set; }

    //tile�ĳ���
    Direction tileDirection;
    public Direction TileDirection { get => tileDirection; set => tileDirection = value; }

    //public bool IsSwitching { get; set; }


    public override bool IsLanded
    {
        get => base.IsLanded;
        set
        {
            base.IsLanded = value;
            gameObject.layer = value ? LayerMask.NameToLayer(StaticData.ConcreteTileMask) : LayerMask.NameToLayer(StaticData.TempTileMask);
        }
    }

    bool previewing;
    public bool Previewing
    {
        get => previewing;
        set
        {
            previewing = value;
            previewGlow.SetActive(value);
        }
    }


    protected virtual void Awake()
    {
        previewGlow = transform.Find("PreviewGlow").gameObject;
        PreviewRenderer = previewGlow.GetComponent<SpriteRenderer>();
        tileBase = transform.Find("TileBase");
    }

    public void SetTileColor(Color colorToSet)
    {
        foreach (var sr in TileRenderers)
        {
            sr.color = colorToSet;
        }
    }

    public virtual void TileLanded()//tile�������ͼʱ
    {
        SetBackToParent();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        StaticData.CorrectTileCoord(this);
        Previewing = false;
        GetTileDirection();

        Content.ContentLanded();//������ܻ��������
        IsLanded = true;//���������CONTENTLANDED���棬����ᵼ�»����Լ�
    }

    public virtual void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position + Vector3.forward * 0.01f;
        content.transform.localRotation = Quaternion.identity;
        content.m_GameTile = this;
        Content = content;
    }

    public virtual void OnTilePass(Enemy enemy)//������������Ч��
    {
        Content.OnContentPass(enemy);
    }

    public override void TileDown()
    {
        base.TileDown();
        if (m_DraggingShape != null)
        {
            m_DraggingShape.StartDragging();
        }
    }



    //protected override void OnMouseDown()
    //{
    //    base.OnMouseDown();
    //    if (m_DraggingShape != null)
    //    {
    //        m_DraggingShape.StartDragging();
    //    }
    //}


    //protected override void OnMouseUp()
    //{
    //    base.OnMouseUp();
    //    if (m_DraggingShape != null)
    //    {
    //        m_DraggingShape.EndDragging();
    //    }

    //}

    public override void OnSpawn()
    {
        base.OnSpawn();
        IsLanded = false;//�������������ʱ����,Gameboard����tileʱ
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (BoardSystem.SelectingTile == this)
        {
            BoardSystem.SelectingTile = null;
        }
        ObjectPool.Instance.UnSpawn(Content);
        gameObject.tag = StaticData.Untagged;
        m_DraggingShape = null;
        Previewing = false;
        SetTileColor(Color.white);
        Content = null;
        transform.rotation = Quaternion.identity;
        //IsSwitching = false;
    }

    public void SetRandomRotation(int dir = -1)
    {
        int randomDir = dir == -1 ? UnityEngine.Random.Range(0, 4) : dir;
        transform.rotation = DirectionExtensions.GetDirection(randomDir).GetRotation();
        CorrectRotation();
    }

    public void SetRotation(int direction)
    {
        transform.rotation = DirectionExtensions.GetDirection(direction).GetRotation();
        CorrectRotation();
    }

    private void GetTileDirection()
    {
        TileDirection = DirectionExtensions.GetDirection(transform.position, transform.position + transform.up);
    }

    public void CorrectRotation()
    {
        tileBase.rotation = Quaternion.identity;
        Content.CorretRotation();
    }
    public void Highlight(bool value)
    {
        hightLightEffect.SetActive(value);
    }

}
