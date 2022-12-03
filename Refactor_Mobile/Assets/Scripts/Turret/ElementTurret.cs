using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ElementTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.ElementTurret;

    public override void OnSwitch()
    {
        base.OnSwitch();
        GameManager.Instance.elementTurrets.Remove(this);
    }
    public override void ContentLanded()
    {
        GameManager.Instance.elementTurrets.Add(this);
        base.ContentLanded();
        m_GameTile.tag = StaticData.UndropablePoint;
        IsSwitching = false;
    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        m_ContentStruct.Element = (int)Strategy.Attribute.element;
        m_ContentStruct.Quality = Strategy.Quality;
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.TempGroundMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = true;//Ԫ�������ܱ����գ�����ʱ�����ذ�TILE��Ϊ�Ϸ�û������
        }
        GameManager.Instance.elementTurrets.Remove(this);
    }


}
