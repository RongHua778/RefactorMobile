using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Empty;


    public override void ContentLanded()
    {
        base.ContentLanded();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        m_ContentStruct.ContentName = "Empty";
    }



    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)//��������и��ӣ����������������������Ϊ��Ѱ·
        {
            ObjectPool.Instance.UnSpawn(m_GameTile);
            return;
        }
        else
        {
            StaticData.SetNodeWalkable(m_GameTile, true);
        }
    }
}
