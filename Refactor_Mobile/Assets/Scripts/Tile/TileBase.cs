using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TileBase : ReusableObject
{


    [SerializeField] Vector2Int _offsetCoord;
    public Vector2Int OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    private bool isLanded = false;//�Ƿ��ڰ�ͼ״̬
    public virtual bool IsLanded { get => isLanded; set => isLanded = value; }


    public virtual void OnTileSelected(bool value)
    {

    }

    public virtual void TileDown()
    {
        GameEvents.Instance.TileClick();
    }

    public virtual void TileUp()
    {
        GameEvents.Instance.TileUp(this);
        if (DraggingActions.DraggingThis != null)
        {
            DraggingActions.DraggingThis.EndDragging();
        }
    }
}
    //protected virtual void OnMouseDown()
    //{
    //    if (!EventSystem.current.IsPointerOverGameObject())
    //    {
    //        GameEvents.Instance.TileClick();
    //    }
    //}


    //protected virtual void OnMouseUp()
    //{
    //    if (!EventSystem.current.IsPointerOverGameObject())
    //    {
    //        GameEvents.Instance.TileUp(this);
    //    }
    //}

