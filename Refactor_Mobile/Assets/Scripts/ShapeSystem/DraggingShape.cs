using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggingShape : DraggingActions
{
    Vector2 lastPos;
    Transform menuTrans;
    Camera mainCam;
    private static DraggingShape pickingShape = null;
    public static DraggingShape PickingShape { get => pickingShape; set => pickingShape = value; }

    TileShape tileShape;
    public TileShape TileShape { get => tileShape; set => tileShape = value; }

    bool canDrop = false;
    bool overLapPoint = false;
    bool skillFull = false;
    bool waitingForPath = false;

    [SerializeField]
    Color wrongColor, correctColor, transparentColor, holdColor, dropColor, equipColor = default;


    protected override void Awake()
    {
        base.Awake();
        menuTrans = transform.Find("DragMenu");
        mainCam = Camera.main;
        menuTrans.Find("Btn_Confirm").GetComponent<Button>().onClick.AddListener(ConfirmShape);
        menuTrans.Find("Btn_Rotate").GetComponent<Button>().onClick.AddListener(RotateShape);
        menuTrans.Find("Btn_Reset").GetComponent<Button>().onClick.AddListener(UndoShape);
        menuTrans.Find("Btn_Move").GetComponent<DragButton>().m_Shape = this;
    }
    public void Initialized(TileShape shape)
    {
        TileShape = shape;
    }

    private void SetAllColor(Color colorToSet)//底图效果调整
    {
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.SetTileColor(colorToSet);
        }
    }

    private void SetPreviewColor(Color colorToSet)//外发光效果调整
    {
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.PreviewRenderer.color = colorToSet;
        }
    }

    private void SetTileColor(Color colorToSet, GameTile tile)
    {
        tile.SetTileColor(colorToSet);
    }

    protected override void Update()
    {
        base.Update();
        if (tileShape.IsPreviewing && InputManager.Instance.GetKeyDown(KeyBindingActions.Rotate))
        {
            RotateShape();
        }
    }



    public override void OnDraggingInUpdate()
    {
        base.OnDraggingInUpdate();
        Vector3 mousePos = MouseInWorldCoords();
        transform.position = new Vector3(Mathf.Round(mousePos.x + pointerOffset.x), Mathf.Round(mousePos.y + pointerOffset.y), transform.position.z);
        if ((Vector2)transform.position != lastPos)
        {
            if (CheckCanDrop())
            {
                StopAllCoroutines();
                StartCoroutine(TryFindPath());
            }
        }
        lastPos = transform.position;
    }

    private void LateUpdate()
    {
        SizeUpdateWithCam();
    }

    private void SizeUpdateWithCam()
    {
        menuTrans.localScale = new Vector3(mainCam.orthographicSize / 450, mainCam.orthographicSize / 450, 1);
    }

    public void ShapeFindPath()
    {
        StartCoroutine(TryFindPath());
    }

    public void ShapeSpawned()//生成模块后，检查一下可否放置和寻路
    {
        if (CheckCanDrop())
        {
            StartCoroutine(TryFindPath());
        }
        PickingShape = this;
    }

    private bool CheckCanDrop()
    {
        overLapPoint = false;
        skillFull = false;
        canDrop = true;
        Physics2D.SyncTransforms();
        //CheckAttached();
        CheckOverLap();
        CheckMapEdge();
        if (!canDrop)
        {
            SetAllColor(wrongColor);
            return false;
        }
        else
        {
            //SetAllColor(correctColor);
            return true;
        }
    }
    private void CheckMapEdge()
    {
        Vector2Int groundSize = BoardSystem._groundSize;
        int maxX = (groundSize.x - 1) / 2;
        int minX = -(groundSize.x - 1) / 2;
        int maxY = (groundSize.y - 1) / 2;
        int minY = -(groundSize.y - 1) / 2;
        foreach (GameTile tile in TileShape.tiles)
        {
            if (tile.transform.position.x > maxX ||
                tile.transform.position.x < minX ||
                tile.transform.position.y > maxY ||
                tile.transform.position.y < minY)
            {
                canDrop = false;
                break;
            }
        }
    }
    //private void CheckAttached()//检查是否相连
    //{
    //    int hits;
    //    canDrop = false;
    //    foreach (GameTile tile in TileShape.tiles)
    //    {
    //        Vector2 pos = tile.transform.position;
    //        hits = Physics2D.OverlapCircleNonAlloc(pos, 0.51f, attachedResult, LayerMask.GetMask(StaticData.ConcreteTileMask));
    //        if (hits > 0)
    //        {
    //            canDrop = true;
    //            break;
    //        }
    //    }
    //}
    private void CheckOverLap()
    {
        BoardSystem.PreviewEquipTile = null;
        foreach (var tile in TileShape.tiles)
        {
            Collider2D col = StaticData.RaycastCollider(tile.transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
            if (col == null)//没有下层TILE，正常
            {
                SetTileColor(correctColor, tile);
                continue;
            }
            if (tile.Content.ContentType != GameTileContentType.Empty)//如果是有防御塔的，就比对冲突
            {

                if (col.CompareTag(StaticData.OnlyRefactorTag))
                {

                    if (tile.Content.CanEquip)
                    {
                        //tTile是地上的，tile是Shape上的
                        //StrategyBase shapeTurretStrategy = ((ConcreteContent)tile.Content).Strategy;
                        StrategyBase shapeStrategy = ((ConcreteContent)tile.Content).Strategy;

                        GameTile tTile = col.GetComponent<GameTile>();
                        StrategyBase groundTurretStrategy = ((ConcreteContent)tTile.Content).Strategy;

                        if (shapeStrategy.TurretSkills.Count - 2 <= groundTurretStrategy.ElementSKillSlot - groundTurretStrategy.TurretSkills.Count)//地下的有足够格子
                        {
                            SetTileColor(equipColor, tile);
                            BoardSystem.PreviewEquipTile = tTile;
                            break;
                        }
                        //if (groundTurretStrategy.ElementSKillSlot + 1 > groundTurretStrategy.TurretSkills.Count)//地下的有足够格子
                        //{
                        //    SetTileColor(equipColor, tile);
                        //    BoardSystem.PreviewEquipTile = tTile;
                        //    break;
                        //}
                        else
                        {
                            canDrop = false;
                            skillFull = true;
                            break;
                        }
                    }
                    else
                    {
                        overLapPoint = true;
                        canDrop = false;
                        break;
                    }
                }


                if (col.CompareTag(StaticData.UndropablePoint))//冲突，返回，所有颜色被设为红色
                {
                    canDrop = false;
                    overLapPoint = true;
                    break;
                }
                SetTileColor(correctColor, tile);
            }
            else
            {
                SetTileColor(transparentColor, tile);
            }

        }
    }



    private IEnumerator TryFindPath()
    {
        waitingForPath = true;
        yield return new WaitForSeconds(0.1f);
        Sound.Instance.PlayUISound("Sound_Shape");
        ChangeAstarPath();
        GameEvents.Instance.SeekPath();
        yield return new WaitForSeconds(0.1f);
        waitingForPath = false;
    }
    List<GridNodeBase> ChangeNodes = new List<GridNodeBase>();

    private void ChangeAstarPath()
    {
        var grid = AstarPath.active.data.gridGraph;
        ResetChangeNode(grid);
        //foreach (var node in ChangeNodes)
        //{
        //    node.Walkable = !node.Walkable;
        //    grid.CalculateConnectionsForCellAndNeighbours(node.XCoordinateInGrid, node.ZCoordinateInGrid);
        //}
        //ChangeNodes.Clear();
        foreach (var tile in TileShape.tiles)
        {
            StaticData.CorrectTileCoord(tile);

            AstarPath.active.AddWorkItem(ctx =>
            {
                int x = tile.OffsetCoord.x;
                int z = tile.OffsetCoord.y;

                if ((z * GameRes.GroundSize + x) > (GameRes.GroundSize * GameRes.GroundSize) - 1 || (z * GameRes.GroundSize + x) < 0)
                {
                    return;
                }

                GridNodeBase node = grid.nodes[z * grid.width + x];

                if (!node.ChangeAbleNode)
                    return;
                if (node.Walkable != tile.isWalkable)
                {
                    node.Walkable = !node.Walkable;
                    ChangeNodes.Add(node);
                    grid.CalculateConnectionsForCellAndNeighbours(x, z);
                }

            });
        }
    }

    private void ResetChangeNode(GridGraph grid)
    {
        foreach (var node in ChangeNodes)
        {
            node.Walkable = !node.Walkable;
            grid.CalculateConnectionsForCellAndNeighbours(node.XCoordinateInGrid, node.ZCoordinateInGrid);
        }
        ChangeNodes.Clear();
    }

    public void RotateShape()
    {
        transform.Rotate(0, 0, -90f);
        menuTrans.Rotate(0, 0, 90f);
        foreach (GameTile tile in TileShape.tiles)
        {
            tile.CorrectRotation();
        }
        if (CheckCanDrop())
        {
            StopAllCoroutines();
            StartCoroutine(TryFindPath());
            SetPreviewColor(dropColor);
        }

    }


    public void ConfirmShape()
    {
        if (waitingForPath)
        {
            TipsManager.Instance.ShowMessage("你点的太快了");
            return;
        }
        //判断是否满足强制摆位
        if (!GameRes.CheckForcePlacement(transform.position, transform.up))
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLACE"));
            return;
        }
        if (canDrop)
        {
            if (!BoardSystem.FindPath)
            {
                TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOROUTE"));
                return;
            }
            Sound.Instance.PlayUISound("Sound_ConfirmShape");
            foreach (GameTile tile in TileShape.tiles)
            {
                tile.TileLanded();
            }
            GameManager.Instance.ConfirmShape();

            PickingShape = null;
            Destroy(this.gameObject);
        }
        else if (overLapPoint)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOOVERLAP"));
        }
        else if (skillFull)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("SKILLFULL"));
        }
        //else
        //{
        //    GameManager.Instance.ShowMessage("必须与已有区域相连");
        //}
    }

    public void UndoShape()
    {
        if (StaticData.LockKeyboard)//教程期间无法取消
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLEASE"));
            return;
        }
        BoardSystem.PreviewEquipTile = null;
        StartCoroutine(UndoShapeCor());
    }

    private IEnumerator UndoShapeCor()
    {
        //if (tileShape.tiles[0].Content.ContentType == GameTileContentType.RefactorTurret)//重构塔撤销
        //{
        //    ((RefactorStrategy)((RefactorTurret)(tileShape.tiles[0].Content)).Strategy).UndoRefactor();
        //}
        if (tileShape.shapeType == ShapeType.D)
        {
            tileShape.tiles[0].Content.OnUndo();
        }
        else
        {
            GameManager.Instance.UndoShape();
        }
        //GameManager.Instance.UndoShape();
        var grid = AstarPath.active.data.gridGraph;
        ResetChangeNode(grid);
        transform.position = Vector3.one * 1000;
        ShapeFindPath();
        while (waitingForPath)
        {
            yield return null;
        }
        tileShape.ReclaimTiles();
    }



    public override void StartDragging()
    {
        isDragging = true;
        DraggingThis = this;
        pointerOffset = transform.position - MouseInWorldCoords();
        pointerOffset.z = 0;
        SetPreviewColor(holdColor);

    }


    public override void EndDragging()
    {
        if (isDragging)
        {
            if (CheckCanDrop())
                SetPreviewColor(dropColor);
            else
                SetPreviewColor(wrongColor);
            isDragging = false;
            DraggingThis = null;
        }
    }

}
