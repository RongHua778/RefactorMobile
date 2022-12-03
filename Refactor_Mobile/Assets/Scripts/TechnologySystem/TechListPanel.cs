using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TechListPanel : IUserInterface
{
    [SerializeField] TechItem techItemPrefab = default;
    [SerializeField] RectTransform techItemParent = default;
    [SerializeField] RectTransform rect = default;
    List<TechItem> techItems = new List<TechItem>();

    public void AddTech(Technology tech)
    {
        TechItem techItem = Instantiate(techItemPrefab, techItemParent);
        techItem.SetTechItem(tech);
        techItems.Add(techItem);
        ResetPos();
    }
    private void ResetPos()
    {
        StartCoroutine(ResetCor());
    }

    IEnumerator ResetCor()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(techItemParent);
        yield return new WaitForEndOfFrame();
        rect.anchoredPosition = new Vector2(0, -rect.sizeDelta.y / 2 + 60);
    }

    public override void Show()
    {
        base.Show();
        ResetPos();
    }

    public void RemoveTech(Technology tech)
    {
        foreach (var item in techItems.ToList())
        {
            if (item.MyTech.TechName == tech.TechName)
            {
                techItems.Remove(item);
                Destroy(item.gameObject);
                ResetPos();
                break;
            }
        }
    }
}
