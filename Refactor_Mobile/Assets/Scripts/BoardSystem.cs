using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;
using DG.Tweening;


[Serializable]
public struct PathPoint
{
    public Vector2 PathPos;
    public Direction PathDirection;
    public Vector2 ExitPoint;
    public PathPoint(Vector2 pos, Direction dir, Vector2 exit)
    {
        PathPos = pos;
        PathDirection = dir;
        ExitPoint = exit;
    }
}

public class BoardSystem : IGameSystem
{
    //计算点击选中
    #region 装备预览
    static GameObject equipAnim;
    private static TileBase previewEquipTile;
    public static TileBase PreviewEquipTile
    {
        get => previewEquipTile;
        set
        {
            previewEquipTile = value;
            if (previewEquipTile != null)
                equipAnim.transform.position = previewEquipTile.transform.position;
            equipAnim.SetActive(previewEquipTile != null);
        }
    }
    #endregion
    #region 选择Tile高亮
    static GameObject selection;
    float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.2f; }
    private float moveDis;
    private Vector3 startPos;
    private Vector2Int sizeOffset;


    private static TileBase selectingTile;
    public static TileBase SelectingTile
    {
        get => selectingTile;
        set
        {
            if (selectingTile != null)
            {
                selectingTile.OnTileSelected(false);
                selectingTile = selectingTile == value ? null : value;
                TipsManager.Instance.HideTips();
            }
            else
            {
                selectingTile = value;
            }
            if (selectingTile != null)
            {
                selection.transform.position = selectingTile.transform.position;
                selectingTile.OnTileSelected(true);
            }
            selection.SetActive(selectingTile != null);
            Sound.Instance.PlayUISound("Sound_Click");
        }

    }
    #endregion

    public Vector2Int _startSize = new Vector2Int(3, 3); //初始大小
    public static Vector2Int _groundSize; //地图大小
    public static int PathLength => shortestPoints.Count;

    //[SerializeField] PathFollower PathFollowerPrefab = default;
    //private GameBehaviorCollection followers = new GameBehaviorCollection();

    [SerializeField] PathArrow pathArrowPrefab = default;
    private List<PathArrow> pathArrows = new List<PathArrow>();

    List<Vector2Int> tilePoss = new List<Vector2Int>();


    public static List<BasicTile> shortestPath = new List<BasicTile>();

    public static List<PathPoint> shortestPoints = new List<PathPoint>();
    private List<PathPoint> arrowPoints = new List<PathPoint>();

    public static Path path;

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }


    private List<TrapContent> battleTraps = new List<TrapContent>();
    //新手引导指引格子
    private List<GameObject> tutorialObjs = new List<GameObject>();

    [SerializeField] Material pathArrowMat = default;
    [SerializeField] SpriteRenderer groundBg = default;
    [SerializeField] IntentLine intentLinePrefab = default;



    public static bool FindPath { get; set; }

    //Trap设置
    public static TrapContent LastTrap;

    public override void Initialize()
    {
        selection = transform.Find("Selection").gameObject;
        equipAnim = transform.Find("EquipAnim").gameObject;

        GameEvents.Instance.onSeekPath += SeekPath;
        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;
        //SetGameBoard();
    }

    public override void Release()
    {
        GameEvents.Instance.onSeekPath -= SeekPath;
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;

    }

    private float pathProgress;

    public override void GameUpdate()
    {
        //followers.GameUpdate();
        pathProgress += Time.deltaTime * 0.8f;

        foreach (var item in pathArrows)
        {
            item.PathArrowUpdate(pathProgress);
        }
        if (pathProgress >= 1f)
        {
            pathProgress = 0;
        }

    }
    private void TileClick()
    {
        startPos = Input.mousePosition;
    }

    private void TileUp(TileBase tile)
    {
        moveDis = Vector2.SqrMagnitude(Input.mousePosition - startPos);
        if (moveDis < 1000f)
        {
            SelectingTile = tile;
        }
    }

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (FindPath)
    //        ShowPath();
    //}


    public void SetGameBoard()
    {
        _groundSize = new Vector2Int(GameRes.GroundSize, GameRes.GroundSize);//设置地图大小

        sizeOffset = new Vector2Int((int)((_startSize.x - 1) * 0.5f), (int)((_startSize.y - 1) * 0.5f));
        StaticData.BoardOffset = new Vector2Int((int)((_groundSize.x - 1) * 0.5f), (int)((_groundSize.y - 1) * 0.5f));
        groundBg.size = _groundSize;
        GenerateGroundTiles(_groundSize);

        GenereteIntentLine();

        Physics2D.SyncTransforms();//涉及物理检测前，需要调用
        var grid = AstarPath.active.data.gridGraph;
        //grid.center = transform.position;
        //grid.width = _groundSize.x;
        //grid.depth = _groundSize.y;
        grid.SetDimensions(_groundSize.x, _groundSize.y, 1f);
        grid.Scan();

        //AstarPath.active.graphs[0].Scan();

        //AstarPath.active.Scan();
    }

    private void PathCheck()
    {
        Physics2D.SyncTransforms();
        SeekPath();
        ShowPath();
        CheckPathTrap();
    }
    public void FirstGameSet()
    {
        SetGameBoard();
        if (LevelManager.Instance.CurrentLevel.PreTiles.Count > 0)
        {
            GenerateFixedTiles(LevelManager.Instance.CurrentLevel.PreTiles);
        }
        else
        {
            GenerateStartTiles(_startSize, sizeOffset);
        }
        GenerateTrapTiles(sizeOffset, _startSize);
        PathCheck();
    }

    public void LoadSaveGame()
    {
        SetGameBoard();
        try
        {
            foreach (var content in LevelManager.Instance.LastGameSave.SaveContents)
            {
                GameTile tile = null;
                //Vector2Int pos = new Vector2Int(content.posX, content.posY);
                Vector2Int pos = content.Pos;
                switch (content.ContentType)
                {
                    case 0:
                    default:
                        tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                        break;
                    case 1:
                        tile = ConstructHelper.GetDestinationPoint();
                        DestinationPoint = tile;
                        break;
                    case 2:
                        tile = ConstructHelper.GetSpawnPoint();
                        SpawnPoint = tile;
                        break;
                    case 3:
                        tile = ConstructHelper.GetElementTurret(content);
                        break;
                    case 4:
                        tile = ConstructHelper.GetRefactorTurret(content);
                        break;
                    case 5://陷阱
                        tile = ConstructHelper.GetTrap(content.ContentName, content.TrapRevealed);
                        break;
                        //case 7://建筑
                        //    tile = ConstructHelper.GetBuilding(content);
                        //    break;
                }
                tile.SetRotation(content.Direction);
                tile.transform.position = (Vector3Int)pos;
                tile.TileLanded();
                Physics2D.SyncTransforms();
            }
        }
        catch
        {
            Debug.Log("Error when load game");

        }

        PathCheck();
        ShowPath();
    }

    private void GenerateFixedTiles(List<ContentStruct> tileInfos)
    {
        for (int i = 0; i < tileInfos.Count; i++)
        {
            GameTile tile = null;
            Vector2Int pos = new Vector2Int((int)tileInfos[i].Pos.x, (int)tileInfos[i].Pos.y);

            switch (tileInfos[i].ContentType)
            {
                case 0:
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                    break;
                case 1:
                    tile = ConstructHelper.GetDestinationPoint();
                    DestinationPoint = tile;
                    break;
                case 2:
                    tile = ConstructHelper.GetSpawnPoint();
                    SpawnPoint = tile;
                    break;
                case 3:
                    tile = ConstructHelper.GetElementTurret(tileInfos[i]);
                    break;
                case 4:
                    tile = ConstructHelper.GetRefactorTurret(tileInfos[i]);
                    break;
                case 5:
                    break;
            }
            tile.SetRotation(tileInfos[i].Direction);
            tile.transform.position = (Vector3Int)pos;
            tile.TileLanded();
            Physics2D.SyncTransforms();
        }
    }

    private void GenerateStartTiles(Vector2Int size, Vector2Int offset)
    {
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = null;
                Vector2Int pos = new Vector2Int(x, y) - offset;
                if (pos.x == 0 && pos.y != 0)
                    continue;
                if (pos.x == -1 && pos.y == 0)//SpawnPoint
                {
                    tile = ConstructHelper.GetSpawnPoint();
                    SpawnPoint = tile;
                }
                else if (pos.x == 1 && pos.y == 0)//Destination
                {
                    tile = ConstructHelper.GetDestinationPoint();
                    DestinationPoint = tile;
                }
                else//空格子
                {
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                }
                tile.transform.position = (Vector3Int)pos;
                tile.TileLanded();
                Physics2D.SyncTransforms();
            }
        }
    }

    private void SeekPath()
    {
        var p = ABPath.Construct(SpawnPoint.transform.position, DestinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
        AstarPath.BlockUntilCalculated(p);
    }

    private IEnumerator SeekPathCor()
    {
        yield return new WaitForSeconds(0.1f);
        var p = ABPath.Construct(SpawnPoint.transform.position, DestinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
        AstarPath.BlockUntilCalculated(p);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            FindPath = true;
            if (path != null && p.vectorPath.SequenceEqual(path.vectorPath))
            {
                //Debug.Log("Found Same Path");
                return;
            }
            path = p;
            ShowPath();
            //Debug.Log("Find Path!");
        }
        else
        {
            path = p;
            HidePath();
            //Debug.LogError("No Path Found");
            FindPath = false;
        }
    }

    public static List<PathPoint> GetReversePath()
    {
        List<PathPoint> pathPoints = new List<PathPoint>();
        Direction dir = Direction.up;
        PathPoint point = default;
        for (int i = path.vectorPath.Count - 1; i >= 0; i--)
        {
            if (i > 0)
            {
                dir = DirectionExtensions.GetDirection(path.vectorPath[i], path.vectorPath[i - 1]);
                point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i] + dir.GetHalfVector());
            }
            else
            {
                dir = DirectionExtensions.GetDirection(path.vectorPath[i + 1], path.vectorPath[i]);
                point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i]);
            }
            pathPoints.Add(point);
        }
        return pathPoints;
    }
    public void GetPathPoints()
    {
        shortestPoints.Clear();
        arrowPoints.Clear();
        Direction dir = Direction.up;
        PathPoint point;
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            if (i < path.vectorPath.Count - 1)
            {
                dir = DirectionExtensions.GetDirection(path.vectorPath[i], path.vectorPath[i + 1]);
                point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i] + dir.GetHalfVector());
            }
            else//终点格的出发和结束点是同一点
            {
                dir = DirectionExtensions.GetDirection(path.vectorPath[i - 1], path.vectorPath[i]);
                point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i]);

            }
            arrowPoints.Add(point);
            shortestPoints.Add(point);
        }
        if (GameRes.Reverse)
        {
            for (int i = path.vectorPath.Count - 1; i >= 0; i--)
            {

                if (i == path.vectorPath.Count - 1)
                {
                    dir = DirectionExtensions.GetDirection(path.vectorPath[i], path.vectorPath[i - 1]);
                    point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i]);
                    shortestPoints.Add(point);
                    point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i] + dir.GetHalfVector());

                }
                else if (i > 0)
                {
                    dir = DirectionExtensions.GetDirection(path.vectorPath[i], path.vectorPath[i - 1]);
                    point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i] + dir.GetHalfVector());
                }
                else
                {
                    dir = DirectionExtensions.GetDirection(path.vectorPath[1], path.vectorPath[0]);
                    point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i]);

                }
                shortestPoints.Add(point);
            }
        }
    }


    public void ShowPath()
    {
        GetPathPoints();
        HidePath();
        //for (int i = 0; i < shortestPoints.Count - 1; i++)
        //{
        //    PathFollower follower = ObjectPool.Instance.Spawn(PathFollowerPrefab) as PathFollower;
        //    follower.SpawnOn(i, shortestPoints);
        //    followers.Add(follower);
        //}
        for (int i = 0; i < arrowPoints.Count - 1; i++)
        {
            PathArrow arrow = ObjectPool.Instance.Spawn(pathArrowPrefab) as PathArrow;
            arrow.SpawnOn(i, arrowPoints);
            pathArrows.Add(arrow);
        }
        pathProgress = 0.5f;
    }


    private void HidePath()
    {
        //foreach (PathFollower pl in followers.behaviors)
        //{
        //    ObjectPool.Instance.UnSpawn(pl);
        //}
        //followers.behaviors.Clear();
        foreach (PathArrow pl in pathArrows)
        {
            ObjectPool.Instance.UnSpawn(pl);
        }
        pathArrows.Clear();
    }

    public void TransparentPath(Color color, float time)
    {
        pathArrowMat.DOColor(color, time);
    }

    public void GetPathTiles()
    {
        foreach (var tile in shortestPath)
        {
            tile.isPath = false;
        }
        shortestPath.Clear();
        foreach (var point in shortestPoints)
        {
            BasicTile tile = StaticData.RaycastCollider(point.PathPos, LayerMask.GetMask(StaticData.ConcreteTileMask)).GetComponent<BasicTile>();
            tile.isPath = true;
            shortestPath.Add(tile);
        }
        foreach (var trap in battleTraps)
        {
            trap.ClearTurnData();
        }
    }

    //private void PreCheckPath()
    //{
    //    LastTrap = null;
    //    for (int i = 0; i < shortestPath.Count; i++)
    //    {
    //        shortestPath[i].CheckContent(i,shortestPath);
    //    }
    //}

    private void GenerateTrapTiles(Vector2Int offset, Vector2Int size)
    {
        List<Vector2Int> traps = new List<Vector2Int>();
        List<Vector2Int> tempPoss = tilePoss.ToList();


        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int pos = new Vector2Int(x, y) - offset;
                tempPoss.Remove(pos);
            }
        }
        for (int i = 0; i < LevelManager.Instance.CurrentLevel.TrapCount; i++)
        {
            int index = UnityEngine.Random.Range(0, tempPoss.Count);
            Vector2Int temp = tempPoss[index];
            traps.Add(temp);
            List<Vector2Int> neibor = StaticData.GetCirclePoints((int)(4 - GameRes.TrapDistanceAdjust));
            for (int k = 0; k < neibor.Count; k++)
            {
                neibor[k] = neibor[k] + tempPoss[index];
            }
            tempPoss = tempPoss.Except(neibor).ToList();
            tempPoss.Remove(temp);
            if (tempPoss.Count <= 0)//没有多余的空位了
                break;
        }
        foreach (Vector2Int pos in traps)
        {
            GameTile tile = ConstructHelper.GetRandomTrap();
            tile.transform.position = (Vector3Int)pos;
            tile.TileLanded();
            //tile.SetRandomRotation();
        }
    }
    private void GenerateGroundTiles(Vector2Int groundSize)
    {
        for (int i = 0, y = 0; y < groundSize.y; y++)
        {
            for (int x = 0; x < groundSize.x; x++, i++)
            {
                GroundTile groundTile = ConstructHelper.GetGroundTile();
                Vector2Int pos = new Vector2Int(x, y) - StaticData.BoardOffset;
                groundTile.transform.position = (Vector3Int)pos;
                groundTile.transform.position += Vector3.forward * 0.1f;
                StaticData.CorrectTileCoord(groundTile);
                tilePoss.Add(pos);
            }
        }
    }

    public void BuyOneEmptyTile()
    {
        if (StaticData.LockKeyboard)//教程期间无法购买地板
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLEASE"));
            return;
        }
        if (GameManager.Instance.OperationState.StateName == StateName.WaveState)
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        if (StaticData.GetNodeWalkable(SelectingTile))
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("ALREADYGROUND"));
            return;
        }
        if (GameRes.FreeGroundTileCount > 0)
        {
            GameRes.FreeGroundTileCount--;
        }
        else if (!GameManager.Instance.ConsumeMoney(GameRes.BuyGroundCost))
        {
            return;
        }
        else
        {
            GameRes.BuyGroundCost += StaticData.Instance.BuyGroundCostMultyply;
        }
        GameTile tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
        tile.transform.position = SelectingTile.transform.position;
        tile.TileLanded();
        Physics2D.SyncTransforms();
        if (DraggingShape.PickingShape != null)
        {
            DraggingShape.PickingShape.ShapeFindPath();//只有用这个才找的到路
        }
        else
        {
            PathCheck();
        }
        TipsManager.Instance.HideTips();
    }


    public void CheckPathTrap()//检查是否有陷阱加入到路线中
    {
        battleTraps.Clear();
        foreach (var pathPoint in shortestPoints)
        {
            Collider2D col = StaticData.RaycastCollider(pathPoint.PathPos, LayerMask.GetMask(StaticData.TrapMask));
            if (col != null)
            {
                TrapContent trap = col.GetComponent<TrapContent>();
                trap.RevealTrap();
                battleTraps.Add(trap);
            }
        }
    }

    //public void SwitchTrap(TrapContent trap)
    //{
    //    GameRes.SwitchTrapCost += StaticData.Instance.SwitchTrapCostMultiply;
    //    Vector2 pos = trap.m_GameTile.transform.position;
    //    ObjectPool.Instance.UnSpawn(trap.m_GameTile);
    //    GameTile tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
    //    tile.transform.position = pos;
    //    tile.TileLanded();
    //    ConstructHelper.GetTrapShapeByName(trap.TrapAttribute.Name);
    //}

    public void SwitchContent(GameTileContent content)
    {
        LevelManager.Instance.GameSaveContents.Remove(content);
        content.OnSwitch();
        ConstructHelper.GetShapeByContent(content);
    }

    public void SetTutorialPoss(bool value)
    {
        if (!value)
        {
            DestoryTutorialPoss();
        }
        else
        {
            if (GameRes.ForcePlace != null)
            {
                foreach (var pos in GameRes.ForcePlace.GuidePos)
                {
                    GameObject GO = Instantiate(StaticData.Instance.TileFactory.GetTutorialPrefab(), pos, Quaternion.identity, this.transform);
                    tutorialObjs.Add(GO);
                }
            }
        }
    }

    private List<GameTile> previewSlotTiles = new List<GameTile>();
    public void HighlightEmptySlotTurrets(bool value)
    {
        if (value)
        {
            foreach (var item in GameManager.Instance.refactorTurrets.behaviors)
            {
                StrategyBase strategy = ((RefactorTurret)item).Strategy;
                if (strategy.TurretSkills.Count < strategy.ElementSKillSlot + 1)
                {
                    strategy.Concrete.m_GameTile.Highlight(true);
                    previewSlotTiles.Add(strategy.Concrete.m_GameTile);
                }
            }
        }
        else
        {
            foreach (var item in previewSlotTiles)
            {
                item.Highlight(false);
            }
            previewSlotTiles.Clear();
        }

    }


    private void DestoryTutorialPoss()
    {
        foreach (var obj in tutorialObjs)
        {
            Destroy(obj);
        }
        tutorialObjs.Clear();
    }

    private void GenereteIntentLine()
    {
        if (GameRes.IntentLineID == 0)
            return;
        IntentLine line;
        Color color = GameRes.IntentLineID == 1 ? Color.red : Color.cyan;
        color.a = 0.5f;
        int distance = (Mathf.CeilToInt(GameRes.GroundSize / 3)) % 2 == 0 ?
            GameRes.GroundSize / 3 + 1 : GameRes.GroundSize / 3;
        for (int i = -distance / 2; i < distance / 2 + 2; i += distance)
        {
            //x方向
            line = Instantiate(intentLinePrefab, new Vector2(i - 0.5f, 0), Quaternion.Euler(0, 0, 0));
            line.GetComponent<SpriteRenderer>().size = new Vector2(0.2f, GameRes.GroundSize);
            line.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, GameRes.GroundSize);
            line.GetComponent<SpriteRenderer>().color = color;
            line.IntensifyValue = GameRes.IntentLineID == 1 ? -1f : 1f;
            //y方向
            line = Instantiate(intentLinePrefab, new Vector2(0, i - 0.5f), Quaternion.Euler(0, 0, 90));
            line.GetComponent<SpriteRenderer>().size = new Vector2(0.2f, GameRes.GroundSize);
            line.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, GameRes.GroundSize);
            line.GetComponent<SpriteRenderer>().color = color;
            line.IntensifyValue = GameRes.IntentLineID == 1 ? -1f : 1f;


        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (FindPath)
            ShowPath();
    }


}


