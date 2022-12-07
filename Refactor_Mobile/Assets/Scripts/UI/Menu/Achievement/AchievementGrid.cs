using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class AchievementGrid : MonoBehaviour
{
    [SerializeField] private Image achIcon = default;
    [SerializeField] private TextMeshProUGUI achNameTxt = default;
    [SerializeField] private TextMeshProUGUI achDesTxt = default;

    public void SetAch(Achievement ach)
    {
        achIcon.sprite = ach.AchIcon;
        achNameTxt.text = GameMultiLang.GetTraduction(ach.AchKey);
        achDesTxt.text = GameMultiLang.GetTraduction(ach.AchKey + "INFO");
        achIcon.color = ach.IsGet ? Color.white : Color.black;
    }
}
