using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITujian_ListHolder : MonoBehaviour
{
    [SerializeField] ContentAttribute[] attributes = default;
    [SerializeField] ItemSlot itemSlotPrefab = default;
    Transform itemParent;

    public void SetContent(ToggleGroup group)
    {
        itemParent = transform.Find("ListPanel");
        foreach (var item in attributes)
        {
            ItemSlot slot = Instantiate(itemSlotPrefab, itemParent);
            slot.SetContent(item, group);
        }
    }

}
