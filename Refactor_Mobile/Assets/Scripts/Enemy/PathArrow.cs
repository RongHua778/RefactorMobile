using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrow : PathFollower
{
    private float progressOffset;
    //public override bool GameUpdate()
    //{
    //    Progress += Time.deltaTime * ProgressFactor;
    //    while (Progress >= 1f - progressOffset)
    //    {
    //        if (PointIndex == PathPoints.Count - 1)
    //        {
    //            SpawnOn(0);
    //            return true;
    //        }
    //        PrepareNextState();
    //    }
    //    if (DirectionChange == DirectionChange.None)
    //    {
    //        transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, Progress);
    //    }
    //    else
    //    {
    //        float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, Progress);
    //        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    //    }
    //    return true;


    //}

    public void PathArrowUpdate(float progress)
    {
        Progress = progress;
        if (Progress >= 1f - progressOffset)
        {
            if (PointIndex == PathPoints.Count - 1)
            {
                SpawnOn(0);
                return;
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
    }

    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        PositionFrom = (Vector3)CurrentPoint.PathPos - Direction.GetHalfVector();
        progressOffset = 0;
    }

    protected override void PrepareOutro()
    {
        base.PrepareOutro();
        PositionTo = (Vector3)CurrentPoint.PathPos + Direction.GetHalfVector();
        progressOffset = 0.5f;

    }

}
