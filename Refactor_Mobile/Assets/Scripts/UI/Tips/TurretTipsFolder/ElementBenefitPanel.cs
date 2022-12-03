using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementBenefitPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] ElementTxt = default;
    Transform root;
    public void InitializePanel(StrategyBase strategy)
    {
        root = transform.Find("Root");
        ElementTxt[0].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=0>" + StaticData.ElementDIC[ElementType.GOLD].GetIntensifyText(strategy.AttackPerGold * 100 + "%")
            + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementAttackIntensify * 100 + "%)</b>");

        ElementTxt[1].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=1>" + StaticData.ElementDIC[ElementType.WOOD].GetIntensifyText(strategy.FireratePerWood * 100 + "%")
            + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementFirerateIntensify * 100 + "%)</b>");

        ElementTxt[2].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=2>" + StaticData.ElementDIC[ElementType.WATER].GetIntensifyText(strategy.SlowPerWater.ToString())
            + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementSlowIntensify + ")</b>");

        ElementTxt[3].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=3>" + StaticData.ElementDIC[ElementType.FIRE].GetIntensifyText((strategy.CritPerFire * 100).ToString())
            + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementCritIntensify * 100 + ")</b>");

        ElementTxt[4].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=4>" + StaticData.ElementDIC[ElementType.DUST].GetIntensifyText(strategy.SplashPerDust.ToString())
            + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementSplashIntensify + ")</b>");

    }
    public void Show()
    {
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

}
