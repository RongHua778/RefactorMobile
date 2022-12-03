using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElementHolder : MonoBehaviour
{
    [SerializeField] Text[] elementCountTxts = default;
    [SerializeField] ElementsInfo elementsInfo = default;
    [SerializeField] Color normalColor = default;

    public void SetElementCount(StrategyBase strategy)
    {
        elementsInfo.m_Strategy = strategy;
        elementCountTxts[0].text = strategy.GoldCount.ToString();
        elementCountTxts[0].color = strategy.GoldCount > 0 ? normalColor : Color.gray;

        elementCountTxts[1].text = strategy.WoodCount.ToString();
        elementCountTxts[1].color = strategy.WoodCount > 0 ? normalColor : Color.gray;

        elementCountTxts[2].text = strategy.WaterCount.ToString();
        elementCountTxts[2].color = strategy.WaterCount > 0 ? normalColor : Color.gray;

        elementCountTxts[3].text = strategy.FireCount.ToString();
        elementCountTxts[3].color = strategy.FireCount > 0 ? normalColor : Color.gray;

        elementCountTxts[4].text = strategy.DustCount.ToString();
        elementCountTxts[4].color = strategy.DustCount > 0 ? normalColor : Color.gray;

    }

}
