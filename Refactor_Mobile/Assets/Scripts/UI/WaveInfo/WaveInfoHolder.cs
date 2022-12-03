using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaveInfoHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] enemyIcons = default;
    List<EnemyAttribute> currentAtts = new List<EnemyAttribute>();
    public void SetWaveInfo(List<EnemySequence> sequences)
    {
        currentAtts.Clear();
        foreach (var obj in enemyIcons)
        {
            obj.gameObject.SetActive(false);
        }
        for (int i = 0; i < sequences.Count; i++)
        {
            enemyIcons[i].gameObject.SetActive(true);
            EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(sequences[i].EnemyType);
            currentAtts.Add(attribute);
            enemyIcons[i].sprite = attribute.Icon;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TipsManager.Instance.ShowEnemyTips(currentAtts, StaticData.MidUpPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TipsManager.Instance.HideTips();
    }
}
