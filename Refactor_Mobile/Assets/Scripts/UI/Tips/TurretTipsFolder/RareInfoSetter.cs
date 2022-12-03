using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareInfoSetter : MonoBehaviour
{
    [SerializeField] GameObject[] rareSlots = default;
    [SerializeField] InfoBtn rareInfoBtn = default;

    private void Start()
    {
        rareInfoBtn.SetContent(GameMultiLang.GetTraduction("RARE"));
    }
    public void SetRare(int quality)
    {
        if (quality > 0)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            return;
        }
        foreach (var item in rareSlots)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < quality; i++)
        {
            rareSlots[i].SetActive(true);
        }
    }
}
