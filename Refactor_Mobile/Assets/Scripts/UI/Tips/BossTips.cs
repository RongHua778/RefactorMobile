using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossTips : TileTips,IPointerClickHandler
{

    [SerializeField] Text ComingTxt = default;
    [SerializeField] EnemyGrid bossGrid = default;

    public void ReadSequenceInfo(EnemyType bossType,int comingWave)
    {
        bossGrid.SetEnemyInfo(StaticData.Instance.EnemyFactory.Get(bossType));
        ComingTxt.text = GameMultiLang.GetTraduction("BOSSCOMING") + comingWave + GameMultiLang.GetTraduction("WAVE");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        CloseTips();
    }
}
