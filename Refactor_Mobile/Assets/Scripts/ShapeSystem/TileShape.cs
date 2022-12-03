using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum ShapeType
{
    J, L, T, O, I, Z, S, D
}
public class TileShape : MonoBehaviour
{
    public ShapeInfo m_ShapeInfo;
    private Animator m_Anim;
    public bool IsPreviewing = false;
    List<TileSlot> tilePos = new List<TileSlot>();
    List<TileSlot> turretPos = new List<TileSlot>();
    Camera renderCam;
    GameObject bgObj;
    public DraggingShape draggingShape;
    public ElementTurret m_ElementTurret;

    [HideInInspector]
    public ShapeSelectUI m_ShapeSelectUI;
    public List<GameTile> tiles = new List<GameTile>();
    public ShapeType shapeType = default;



    private void Awake()
    {
        m_Anim = this.GetComponent<Animator>();
        tilePos = transform.GetComponentsInChildren<TileSlot>().ToList();
        foreach (var slot in tilePos)
        {
            if (slot.IsTurretPos)
            {
                turretPos.Add(slot);
            }
        }
        renderCam = transform.Find("RenderCam").GetComponent<Camera>();
        bgObj = transform.Find("BG").gameObject;
        draggingShape = this.GetComponent<DraggingShape>();
    }


    //��shape���������
    public void SetTile(GameTile specialTile, int posID = -1, int dir = -1)
    {

        if (specialTile.Content.ContentType == GameTileContentType.ElementTurret)
        {
            m_ElementTurret = specialTile.Content as ElementTurret;//Ԥ���䷽����
        }
        if (shapeType == ShapeType.D)
        {
            specialTile.transform.position = tilePos[0].transform.position;
            specialTile.transform.SetParent(this.transform);
            specialTile.m_DraggingShape = draggingShape;
            tiles.Add(specialTile);
            draggingShape.Initialized(this);
            SetPreviewPlace();
        }
        else
        {
            GameTile tile;
            TileSlot turretSlot = tilePos[posID == -1 ? Random.Range(0, tilePos.Count) : posID];
            tile = specialTile;
            ElementTurret turret = tile.Content as ElementTurret;
            SetTilePos(tile, turretSlot.transform.position, dir);
            tilePos.Remove(turretSlot);

            for (int i = 0; i < tilePos.Count; i++)
            {
                tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                SetTilePos(tile, tilePos[i].transform.position);
            }
            draggingShape.Initialized(this);
        }

    }

    private void SetTilePos(GameTile tile, Vector3 pos, int dir=-1)
    {
        tile.transform.position = pos;
        tile.SetRandomRotation(dir);
        tile.transform.SetParent(this.transform);
        tile.m_DraggingShape = draggingShape;
        tiles.Add(tile);
    }

    public void ReclaimTiles()
    {
        foreach (GameTile tile in tiles)
        {
            ObjectPool.Instance.UnSpawn(tile);
        }
        Destroy(this.gameObject);
    }


    public void SetUIDisplay(int displayID, RenderTexture texture)
    {
        IsPreviewing = false;
        transform.position = new Vector3(1000f + 10f * displayID, 0, -12f);
        renderCam.targetTexture = texture;
        renderCam.gameObject.SetActive(true);
        bgObj.SetActive(true);
    }

    public void SetPreviewPlace()
    {
        IsPreviewing = true;
        m_Anim.SetTrigger("Land");
        bgObj.SetActive(false);
        Vector2 pos = Camera.main.transform.position;
        renderCam.gameObject.SetActive(false);
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), -1f);
        draggingShape.ShapeSpawned();
        foreach (GameTile tile in tiles)
        {
            tile.Previewing = true;
        }
    }



}
