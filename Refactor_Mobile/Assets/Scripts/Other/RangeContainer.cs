using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeContainer : MonoBehaviour
{

    bool ShowingRange = false;

    List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();

    private List<Vector2Int> m_Points;
    public static ConcreteContent ShowingConcrete;
    public void Initialize()
    {
        for (int i = 0; i < 2000; i++)
        {
            RangeIndicator indicator = Instantiate(StaticData.Instance.RangeIndicatorPrefab, transform);
            indicator.ShowSprite(false);
            rangeIndicators.Add(indicator);
        }
    }
    public void SetRange(ConcreteContent concrete,bool value)
    {
        if (value)
        {
            ShowingConcrete = concrete;
            switch (concrete.Strategy.RangeType)
            {
                case RangeType.Circle:
                    m_Points = StaticData.GetCirclePoints(concrete.Strategy.FinalRange);
                    break;
                case RangeType.HalfCircle:
                    m_Points = StaticData.GetHalfCirclePoints(concrete.Strategy.FinalRange);
                    break;
                case RangeType.Line:
                    m_Points = StaticData.GetLinePoints(concrete.Strategy.FinalRange);
                    break;
            }
            transform.SetParent(concrete.transform);
            transform.position = concrete.transform.position;
            transform.rotation = concrete.m_GameTile.transform.rotation;
        }
        else
        {
            ShowingConcrete = null;
        }
        ShowRange(value);
    }

    private void ShowRange(bool show)
    {
        ShowingRange = show;
        if (show)
        {
            for (int i = 0; i < rangeIndicators.Count; i++)
            {
                if (i < m_Points.Count)
                {
                    rangeIndicators[i].transform.localPosition= (Vector3Int)m_Points[i];
                    rangeIndicators[i].ShowSprite(true);
                }
                else
                {
                    rangeIndicators[i].ShowSprite(false);
                }
            }

        }
        else
        {
            HideRanges();
        }


    }

    private void HideRanges()
    {
        foreach (var rangeIndicator in rangeIndicators)
        {
            rangeIndicator.ShowSprite(false);
        }

    }
}
