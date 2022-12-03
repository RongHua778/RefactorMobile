using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHolder : MonoBehaviour
{
    //圆型
    [SerializeField] DetectRange circleDetectRange = default;
    //半圆型
    [SerializeField] DetectRange halfCircleDetectRange = default;
    //直线
    [SerializeField] DetectRange lineDetectRange = default;

    ConcreteContent ConcreteContent;
    private void Awake()
    {
        ConcreteContent = this.transform.root.GetComponentInChildren<ConcreteContent>();
    }
    public void SetRange()
    {

        switch (ConcreteContent.Strategy.RangeType)
        {
            case RangeType.Circle:
                circleDetectRange.gameObject.SetActive(true);
                halfCircleDetectRange.gameObject.SetActive(false);
                lineDetectRange.gameObject.SetActive(false);
                circleDetectRange.GetComponent<BoxCollider2D>().size = Vector2.one * 2 * (ConcreteContent.Strategy.FinalRange + 0.2f) * Mathf.Cos(45 * Mathf.Deg2Rad);
                break;
            case RangeType.HalfCircle:
                circleDetectRange.gameObject.SetActive(false);
                halfCircleDetectRange.gameObject.SetActive(true);
                lineDetectRange.gameObject.SetActive(false);
                halfCircleDetectRange.transform.localScale = Vector2.one * (ConcreteContent.Strategy.FinalRange + 0.2f);
                break;
            case RangeType.Line:
                circleDetectRange.gameObject.SetActive(false);
                halfCircleDetectRange.gameObject.SetActive(false);
                lineDetectRange.gameObject.SetActive(true);
                lineDetectRange.GetComponent<BoxCollider2D>().size = new Vector2(0.7f, ConcreteContent.Strategy.FinalRange - 0.3f);
                lineDetectRange.GetComponent<BoxCollider2D>().offset = new Vector2(0, 1 + 0.5f * (ConcreteContent.Strategy.FinalRange - 1));
                break;
        }
    }

    //public void SetRange(int range, int fRange, RangeType rangeType)
    //{
    //    RecycleRanges();
    //    List<Vector2Int> points = null;
    //    switch (rangeType)
    //    {
    //        case RangeType.Circle:
    //            circleDetectRange.gameObject.SetActive(true);
    //            circleForbidRange.gameObject.SetActive(true);
    //            halfCircleDetectRange.gameObject.SetActive(false);
    //            halfCircleForbidRange.gameObject.SetActive(false);
    //            lineDetectRange.gameObject.SetActive(false);

    //            circleDetectRange.GetComponent<BoxCollider2D>().size = Vector2.one * 2 * (range + 0.2f) * Mathf.Cos(45 * Mathf.Deg2Rad);
    //            circleForbidRange.GetComponent<BoxCollider2D>().size = Vector2.one * 2 * fRange * Mathf.Cos(45 * Mathf.Deg2Rad);
    //            points = StaticData.GetCirclePoints(range, fRange);
    //            break;
    //        case RangeType.HalfCircle:
    //            circleDetectRange.gameObject.SetActive(false);
    //            circleForbidRange.gameObject.SetActive(false);
    //            halfCircleDetectRange.gameObject.SetActive(true);
    //            halfCircleForbidRange.gameObject.SetActive(true);
    //            lineDetectRange.gameObject.SetActive(false);

    //            halfCircleDetectRange.transform.localScale = Vector2.one * (range + 0.2f);
    //            halfCircleForbidRange.transform.localScale = Vector2.one * fRange;
    //            points = StaticData.GetHalfCirclePoints(range, fRange);
    //            break;
    //        case RangeType.Line:
    //            circleDetectRange.gameObject.SetActive(false);
    //            circleForbidRange.gameObject.SetActive(false);
    //            halfCircleDetectRange.gameObject.SetActive(false);
    //            halfCircleForbidRange.gameObject.SetActive(false);
    //            lineDetectRange.gameObject.SetActive(true);
    //            points = StaticData.GetLinePoints(range, fRange);
    //            lineDetectRange.GetComponent<BoxCollider2D>().size = new Vector2(0.7f, range - fRange - 0.3f);
    //            lineDetectRange.GetComponent<BoxCollider2D>().offset = new Vector2(0, fRange + 1 + 0.5f * (range - fRange - 1));

    //            break;
    //    }

    //    for (int i = 0; i < points.Count; i++)
    //    {
    //        RangeIndicator rangeIndecator = Instantiate(rangeIndicatorPrefab, transform);
    //        rangeIndecator.transform.localPosition = (Vector3Int)points[i];
    //        rangeIndicators.Add(rangeIndecator);
    //    }
    //    ShowRange(ShowingRange);
    //}


    public void AddTarget(TargetPoint target)
    {

        ConcreteContent.AddTarget(target);
    }

    public void RemoveTarget(TargetPoint target)
    {
        ConcreteContent.RemoveTarget(target);
    }





}
