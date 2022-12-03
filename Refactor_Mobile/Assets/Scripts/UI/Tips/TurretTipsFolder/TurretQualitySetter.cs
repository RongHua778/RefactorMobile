using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretQualitySetter : MonoBehaviour
{

    [SerializeField] GameObject[] levelIcons = default;
    int upgradeCost;
    [SerializeField] TextMeshProUGUI UpgradeCostValue = default;
    [SerializeField] TextMeshProUGUI SwitchTurretTxt = default;
    public GameObject UpgradeBtn = default;
    [SerializeField] TurretTips m_Turrettips = default;
    int switchCost;


    public void SetLevel(StrategyBase strategy)
    {
        if (strategy.Attribute.Rare <= 0)//不可升级的建筑
        {
            UpgradeBtn.SetActive(false);
            return;
        }
        else
        {
            UpgradeBtn.SetActive(true);
        }
        foreach (var obj in levelIcons)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < strategy.Quality; i++)
        {
            levelIcons[i].SetActive(true);
        }

        if (strategy.Quality < 3)
        {
            upgradeCost = StaticData.Instance.LevelUpCostPerRare[strategy.Attribute.Rare - 1, strategy.Quality - 1];// * m_Strategy.m_Att.Rare * m_Strategy.Quality;
            upgradeCost = (int)(upgradeCost * (1 - GameRes.TurretUpgradeDiscount - strategy.UpgradeDiscount));
            UpgradeCostValue.text = GameMultiLang.GetTraduction("UPGRADE") + "<sprite=7>" + upgradeCost.ToString();
        }
        else
        {
            UpgradeCostValue.text = "MAX";
        }
    }

    public void SetSwitchCost(int cost)
    {
        this.switchCost = cost;
        SwitchTurretTxt.text = GameMultiLang.GetTraduction("SWITCHTRAP") + "<sprite=7>" + this.switchCost;
    }

    public void UpgradeBtnClick()
    {
        m_Turrettips.UpgradeBtnClick(upgradeCost);
    }

    public void SwitchTurretBtnClick()
    {
        GameManager.Instance.SwitchConcrete(m_Turrettips.m_Strategy.Concrete, this.switchCost);
    }

}
