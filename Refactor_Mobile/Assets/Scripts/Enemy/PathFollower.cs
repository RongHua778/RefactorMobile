using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PathFollower : ReusableObject, IGameBehavior
{
    protected virtual bool IsPathfollower => true;
    private List<PathPoint> pathPoints;
    [ReadOnly]
    public int PointIndex = 0;

    Direction direction;
    public Direction Direction { get => direction; set => direction = value; }
    DirectionChange directionChange;
    public virtual DirectionChange DirectionChange { get => directionChange; set => directionChange = value; }

    [HideInInspector] public Transform model;
    protected PathPoint CurrentPoint;
    protected Vector3 positionFrom, positionTo;
    private float progress;
    protected float directionAngleFrom, directionAngleTo;
    private float pathOffset = 0;

    protected float speed = 0.25f;
    private float adjust;
    private float progressFactor;

    public Vector3 PositionFrom { get => positionFrom; set => positionFrom = value; }
    public Vector3 PositionTo { get => positionTo; set => positionTo = value; }
    public float DirectionAngleFrom { get => directionAngleFrom; set => directionAngleFrom = value; }
    public float DirectionAngleTo { get => directionAngleTo; set => directionAngleTo = value; }
    public virtual float Speed { get => speed; }
    public float Adjust { get => adjust; set => adjust = value; }
    public virtual float ProgressFactor { get => progressFactor; set => progressFactor = value; }
    public float Progress { get => progress; set => progress = value; }
    public List<PathPoint> PathPoints { get => pathPoints; set => pathPoints = value; }
    public float PathOffset { get => pathOffset; set => pathOffset = value; }

    protected virtual void Awake()
    {
        model = transform.Find("Model");
    }

    public virtual bool GameUpdate()
    {
        Progress += Time.deltaTime * ProgressFactor;
        while (Progress >= 1f)
        {
            if (PointIndex == PathPoints.Count - 1)
            {
                SpawnOn(0);
                return true;
            }
            Progress = 0;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, Progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, Progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    public virtual void SpawnOn(int pointIndex, List<PathPoint> path = null)
    {
        if (path != null)
        {
            pathPoints = path;
        }
        if (PathPoints.Count <= 1)
        {
            Debug.Log("û��·����");
        }
        PointIndex = pointIndex;
        CurrentPoint = PathPoints[PointIndex];
        Progress = 0f;
        PrepareIntro();
    }



    protected virtual void PrepareIntro()
    {
        positionFrom = CurrentPoint.PathPos;
        transform.position = CurrentPoint.PathPos;
        if (PointIndex == PathPoints.Count - 1)
        {
            positionTo = CurrentPoint.PathPos;
            //Direction = PathPoints[PointIndex-2].PathDirection;
            transform.localRotation = PathPoints[PointIndex - 2].PathDirection.GetRotation();
            Progress = 1;
        }
        else
        {
            positionTo = CurrentPoint.ExitPoint;
            Direction = CurrentPoint.PathDirection;
            DirectionChange = DirectionChange.None;
            model.localPosition = new Vector3(PathOffset, 0);
            directionAngleFrom = directionAngleTo = Direction.GetAngle();
            transform.localRotation = CurrentPoint.PathDirection.GetRotation();
        }
        Adjust = 2f;
        ProgressFactor = Adjust * Speed;
    }

    protected virtual void PrepareOutro()
    {
        positionFrom = positionTo;
        positionTo = CurrentPoint.PathPos;
        DirectionChange = DirectionChange.None;
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(PathOffset, 0);
        transform.localRotation = Direction.GetRotation();
        Adjust = 2f;
        ProgressFactor = Adjust * Speed;
    }

    protected virtual void PrepareNextState()
    {
        PointIndex++;
        CurrentPoint = PathPoints[PointIndex];

        if (PointIndex >= PathPoints.Count - 1)
        {
            PrepareOutro();
            return;
        }

        positionFrom = positionTo;
        positionTo = CurrentPoint.ExitPoint;
        DirectionChange = Direction.GetDirectionChangeTo(CurrentPoint.PathDirection);
        Direction = CurrentPoint.PathDirection;
        directionAngleFrom = directionAngleTo;

        switch (DirectionChange)
        {
            case DirectionChange.None:
                PrepareForward();
                break;
            case DirectionChange.TurnRight:
                PrepareTurnRight();
                break;
            case DirectionChange.TurnLeft:
                PrepareTurnLeft();
                break;
            case DirectionChange.TurnAround:
                PrepareTurnAround();
                break;
        }

    }

    void PrepareForward()
    {
        transform.localRotation = Direction.GetRotation();
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(PathOffset, 0f);
        Adjust = 1f;
        ProgressFactor = Adjust * Speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(PathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        Adjust = 1 / (Mathf.PI * 0.5f * (0.5f - PathOffset));
        ProgressFactor = Adjust * Speed;
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(PathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        Adjust = 1 / (Mathf.PI * 0.5f * (0.5f + PathOffset));
        ProgressFactor = Adjust * Speed;
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (PathOffset > 0f ? 180f : -180f);
        model.localPosition = new Vector3(PathOffset, 0);
        transform.localPosition = positionFrom;
        Adjust = 1 / (Mathf.PI * Mathf.Max(Mathf.Abs(PathOffset), 0.2f));
        ProgressFactor = Adjust * Speed;
    }


    public override void OnSpawn()
    {
        base.OnSpawn();
        model.localPosition = Vector3.zero;
    }
}
