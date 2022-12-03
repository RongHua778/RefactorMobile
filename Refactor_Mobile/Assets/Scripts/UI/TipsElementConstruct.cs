using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TipsElementConstruct : MonoBehaviour
{
    [SerializeField] Image[] Elements = default;
    [SerializeField] TextMeshProUGUI elementSkillName = default;
    [SerializeField] TextMeshProUGUI elementSkillDes = default;
    [SerializeField] GameObject[] areas = default;
    [SerializeField] InfoBtn emptyInfo = default;
    [SerializeField] InfoBtn unlockInfo = default;

    private StrategyBase m_Strategy;
    private TurretTips m_Tips;
    private ElementSkill m_Skill;
    //[SerializeField] Material unrealMat = default;
    //[SerializeField] Image bgImg = default;
    //[SerializeField] Image nameBgImg = default;

    [SerializeField] GameObject unlockBtn = default;
    [SerializeField] TextMeshProUGUI unlockTxt = default;

    private int unlockSlotCost;
    public int AreaID { get; set; }

    private void Start()
    {
        emptyInfo.SetContent(GameMultiLang.GetTraduction("EMPTYSLOT"));
        unlockInfo.SetContent(GameMultiLang.GetTraduction("UNLOCKINFO"));
    }
    public void SetStrategy(StrategyBase strategy, TurretTips turretTips)
    {
        m_Strategy = strategy;
        m_Tips = turretTips;
    }
    public void SetElements(ElementSkill skill)
    {
        SetArea(0);
        m_Skill = skill;
        elementSkillName.text = m_Skill.SkillName;

        if (m_Skill.IsException)
        {
            //bgImg.material = unrealMat;
            //nameBgImg.material = unrealMat;
            elementSkillName.text += GameMultiLang.GetTraduction("EXCEPTION");
        }
        //else
        //{
        //    bgImg.material = null;
        //    nameBgImg.material = null;
        //}

        for (int i = 0; i < skill.Elements.Count; i++)
        {
            Elements[i].sprite = StaticData.Instance.ElementSprites[skill.Elements[i] % 10];
        }

        UpdateDes();
    }

    public void UpdateDes()
    {
        if (m_Skill != null)
            elementSkillDes.text = string.Format(m_Skill.SkillDescription,
            "<b>" + m_Skill.DisplayValue + "</b>", "<b>" + m_Skill.DisplayValue2 + "</b>", "<b>" + m_Skill.DisplayValue3 + "</b>"
            , "<b>" + m_Skill.DisplayValue4 + "</b>", "<b>" + m_Skill.DisplayValue5 + "</b>");
    }

    public void SetEmpty()//显示空槽状态
    {
        m_Skill = null;
        SetArea(1);
    }

    public void SetUnlock(int slotID, bool canUnlock)
    {
        SetArea(2);
        unlockBtn.SetActive(canUnlock);
        //switch (slotID)
        //{
        //    case 3:
        //        unlockSlotCost = 1;
        //        break;
        //    case 4:
        //        unlockSlotCost = 2;
        //        break;
        //    default:
        //        break;
        //}
        unlockSlotCost = 1;
        unlockTxt.text = GameMultiLang.GetTraduction("UNLOCKSLOT") +
            "<sprite=10>" + GameRes.SkillChip + "/" + unlockSlotCost;
    }

    private void SetArea(int id)
    {
        AreaID = id;
        foreach (var area in areas)
        {
            area.SetActive(false);
        }
        areas[id].SetActive(true);
    }

    public void UnlockBtnClick()
    {
        if (m_Strategy.Concrete == null || !m_Strategy.Concrete.Dropped)    //未重构或未放下则不可解锁
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
            return;
        }
        if (GameRes.SkillChip >= unlockSlotCost)
        {
            GameRes.SkillChip -= unlockSlotCost;
            m_Strategy.PrivateExtraSlot++;
            m_Tips.SetElementSkill();
        }
        else
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKCHIP"));
        }
    }
}
