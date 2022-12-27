using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossInfoHolder : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image BossIcon = default;
    EnemyType nextBossType;
    int nextBossWave;

    public void SetBossInfo(EnemyType bossType,int nextBossWave)
    {
        EnemyAttribute att = StaticData.Instance.EnemyFactory.Get(bossType);
        BossIcon.sprite = att.Icon;
        nextBossType = bossType;
        this.nextBossWave = nextBossWave;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TipsManager.Instance.ShowBossTips(nextBossType,nextBossWave,StaticData.MidTipsPos);
    }


}
