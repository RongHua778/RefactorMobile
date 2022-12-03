using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyGroundTips : TileTips
{
    [SerializeField] TextMeshProUGUI costTxt = default;
    bool isShowing = false;
    public void ReadInfo(int cost)
    {
        isShowing = true;
        costTxt.text = GameMultiLang.GetTraduction("BUY") + "<sprite=7>" + cost.ToString();
    }

    public override void Update()
    {
        if (isShowing)
        {
            if (InputManager.Instance.GetKeyDown(KeyBindingActions.BuyGround))
            {
                BuyOneGroundTile();
            }
        }
    }

    public void BuyOneGroundTile()
    {
        GameManager.Instance.BuyOneGround();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        isShowing = false;
    }
}
