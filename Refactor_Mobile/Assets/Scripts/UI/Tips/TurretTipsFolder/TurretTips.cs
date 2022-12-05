using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using DG.Tweening;

public class TurretTips : TileTips
{

    [Header("Area")]
    [SerializeField] GameObject AttributeArea = default;//属性信息区
    [SerializeField] GameObject AnalysisArea = default;//伤害统计区
    [SerializeField] GameObject MainFuncArea = default;//主功能区
    [SerializeField] GameObject DesArea = default;
    [SerializeField] TurretQualitySetter TurretQualitySetter = default;
    [SerializeField] RareInfoSetter RareSetter = default;
    [SerializeField] ElementHolder ElementsHolder = default;//元素数量及元素技能区

    [SerializeField] Text RangeTypeValue = default;
    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text CritDmgValue = default;
    [SerializeField] Text SplashRangeValue = default;
    [SerializeField] Text SlowRateValue = default;
    [SerializeField] Text IntensifyValue = default;
    [SerializeField] Text AnalysisValue = default;


    [SerializeField] List<TipsElementConstruct> elementConstructs;//合成塔组成元素区
    //[SerializeField] 

    //加成值
    [SerializeField] Text AttackChangeTxt = default;
    [SerializeField] Text SpeedChangeTxt = default;
    [SerializeField] Text RangeChangeTxt = default;
    [SerializeField] Text CriticalChangeTxt = default;
    [SerializeField] Text CritDmgChangeTxt = default;
    [SerializeField] Text SplashChangeTxt = default;
    [SerializeField] Text SlowRateChangeTxt = default;
    [SerializeField] Text IntentsifyChangeTxt = default;

    //配方tips

    //infoBtn
    [SerializeField] InfoBtn CriticalInfo = default;
    [SerializeField] InfoBtn SplashInfo = default;
    [SerializeField] InfoBtn SlowInfo = default;

    [SerializeField] Image foldArrowImg = default;
    public StrategyBase m_Strategy;
    //private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    bool isFold = true;
    bool isFolding = false;
    //public bool showingBlueprint = false;


    [SerializeField] Animator TileInfo_Anim = default;

    public override void Initialize()
    {
        base.Initialize();
        CriticalInfo.SetContent(GameMultiLang.GetTraduction("CRITICALINFO"));
        SplashInfo.SetContent(GameMultiLang.GetTraduction("SPLASHINFO"));
        SlowInfo.SetContent(GameMultiLang.GetTraduction("SLOWINFO"));
    }

    public override void Show()
    {
        base.Show();
        TileInfo_Anim.SetTrigger("Show");
    }

    public override void Hide()
    {
        base.Hide();
        m_Strategy = null;
        showingTurret = false;
    }

    public void CloseBtnClick()
    {
        CloseTips();
        if (BluePrintGrid.SelectingGrid != null)
        {
            BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
        }
    }




    private void AreaSetControl(TurretAttribute att, int id, int quality)
    {
        //id==0,场上+实物
        //id==1,预览
        //id=2,非场上+实物,查看配方

        string nameTxt = GameMultiLang.GetTraduction(att.Name);
        TurretQualitySetter.gameObject.SetActive(id == 0);
        TurretQualitySetter.SetSwitchCost(GameRes.SwitchTurretCost);

        switch (att.StrategyType)
        {
            case StrategyType.Element:
                //string element = StaticData.FormElementName(att.element, quality);
                nameTxt = nameTxt + quality;

                AttributeArea.SetActive(true);
                DesArea.SetActive(true);


                TurretQualitySetter.UpgradeBtn.SetActive(false);
                ElementsHolder.gameObject.SetActive(false);
                AnalysisArea.SetActive(id == 0);
                RareSetter.gameObject.SetActive(false);
                MainFuncArea.SetActive(false);
                Description.text = GameMultiLang.GetTraduction(att.Name + "SKILL");
                Icon.sprite = att.TurretLevels[quality - 1].TurretIcon;
                break;
            case StrategyType.Composite:

                AttributeArea.SetActive(isFold);
                DesArea.SetActive(isFold);

                ElementsHolder.gameObject.SetActive(id != 1 && att.Rare > 0);//特殊重构塔不显示元素槽
                AnalysisArea.SetActive(id == 0);
                MainFuncArea.SetActive(id == 2);

                RareSetter.SetRare(att.Rare);
                TurretQualitySetter.SetLevel(m_Strategy);
                SetElementSkill();


                Description.text = StaticData.GetTurretDes(m_Strategy.TurretSkills[0]);
                Icon.sprite = att.TurretLevels[quality - 1].TurretIcon;
                break;
            case StrategyType.Building:
                AttributeArea.SetActive(false);
                DesArea.SetActive(true);

                RareSetter.gameObject.SetActive(false);

                AnalysisArea.SetActive(false);
                MainFuncArea.SetActive(id == 2);

                ElementsHolder.gameObject.SetActive(false);
                SetElementSkill();

                TurretQualitySetter.UpgradeBtn.SetActive(false);

                Icon.sprite = att.Icon;
                Description.text = StaticData.GetTurretDes(m_Strategy.TurretSkills[0]);
                //Description.text = StaticData.GetBuildingDes(m_Strategy.TurretSkills[0] as BuildingSkill);
                break;


        }

        Name.text = nameTxt;


        string rangeTypeTxt = "";
        switch (id == 1 ? att.RangeType : m_Strategy.RangeType)
        {
            case RangeType.Circle:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE1");
                break;
            case RangeType.HalfCircle:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE2");
                break;
            case RangeType.Line:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE3");
                break;
        }
        this.RangeTypeValue.text = rangeTypeTxt;

    }


    public void ReadTurret(StrategyBase Strategy, int showID)//通过场上防御塔查看
    {
        m_Strategy = Strategy;
        AreaSetControl(m_Strategy.Attribute, showID, Strategy.Quality);

        if (showID == 2)
            UpdateBluePrintInfo();
        else
        {
            UpdateRealTimeValue();
            showingTurret = true;
        }
    }


    //public void ReadBluePrint(BluePrintGrid grid)
    //{
    //    m_Grid = grid;
    //    m_Strategy = grid.Strategy;
    //    AreaSetControl(m_Strategy.Attribute, 2, 1);
    //    UpdateBluePrintInfo();

    //}//通过配方查看

    public void ReadAttribute(TurretAttribute att)//通过Attribute查看
    {
        int quality = att.StrategyType == StrategyType.Element ? 5 : 3;
        AreaSetControl(att, 0, quality);
        UpdatePreviewValue(att, quality - 1);
    }


    public void SetElementSkill()
    {
        foreach (var ecom in elementConstructs)
        {
            ecom.gameObject.SetActive(false);
            ecom.SetStrategy(m_Strategy, this);
        }
        for (int i = 0; i < 5; i++)
        {
            elementConstructs[i].gameObject.SetActive(true);
            if (i < m_Strategy.TurretSkills.Count - 1)
                elementConstructs[i].SetElements((ElementSkill)m_Strategy.TurretSkills[i + 1]);//第一个是被动技能
            else if (i < m_Strategy.ElementSKillSlot)
                elementConstructs[i].SetEmpty();
            else
                elementConstructs[i].SetUnlock(i, i == m_Strategy.ElementSKillSlot);
        }
    }

    private void UpdateSkillValues()
    {
        for (int i = 0; i < m_Strategy.ElementSKillSlot; i++)
        {
            elementConstructs[i].UpdateDes();
        }
    }


    private void UpdatePreviewValue(TurretAttribute att, int quality)
    {
        AttackValue.text = att.TurretLevels[quality].AttackDamage.ToString();
        AttackChangeTxt.text = "";

        SpeedValue.text = att.TurretLevels[quality].AttackSpeed.ToString();
        SpeedChangeTxt.text = "";

        RangeValue.text = att.TurretLevels[quality].AttackRange.ToString();
        RangeChangeTxt.text = "";

        CriticalValue.text = (att.TurretLevels[quality].CriticalRate * 100).ToString();
        CriticalChangeTxt.text = "";

        CritDmgValue.text = StaticData.DefaultCritDmg * 100 + "%";

        SplashRangeValue.text = att.TurretLevels[quality].SplashRange.ToString();
        SplashChangeTxt.text = "";

        SlowRateValue.text = att.TurretLevels[quality].SlowRate.ToString();
        SlowRateChangeTxt.text = "";

        IntensifyValue.text = "0%";
        IntentsifyChangeTxt.text = "";

    }

    private void UpdateRealTimeValue()
    {
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = "";

        SpeedValue.text = m_Strategy.FinalFireRate.ToString();
        SpeedChangeTxt.text = "";

        RangeValue.text = m_Strategy.FinalRange.ToString();
        RangeChangeTxt.text = "";

        CriticalValue.text = Mathf.RoundToInt(m_Strategy.FinalCriticalRate * 100).ToString();
        CriticalChangeTxt.text = "";

        CritDmgValue.text = Mathf.RoundToInt(m_Strategy.FinalCriticalPercentage * 100) + "%";
        CritDmgChangeTxt.text = "";

        SplashRangeValue.text = m_Strategy.FinalSplashRange.ToString();
        SplashChangeTxt.text = "";

        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = "";

        IntensifyValue.text = Mathf.RoundToInt(m_Strategy.FinalBulletDamageIntensify * 100) + "%";
        IntentsifyChangeTxt.text = "";

        AnalysisValue.text = m_Strategy.TurnDamage.ToString();

        ElementsHolder.SetElementCount(m_Strategy);
    }

    private void UpdateBluePrintInfo()
    {
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = (m_Strategy.FinalAttack > m_Strategy.InitAttack ?
            "+" + (m_Strategy.FinalAttack - m_Strategy.InitAttack) : "");

        SpeedValue.text = m_Strategy.FinalFireRate.ToString();
        SpeedChangeTxt.text = (m_Strategy.FinalFireRate > m_Strategy.InitFireRate ?
            "+" + (m_Strategy.FinalFireRate - m_Strategy.InitFireRate) : "");

        RangeValue.text = m_Strategy.FinalRange.ToString();
        RangeChangeTxt.text = (m_Strategy.FinalRange > m_Strategy.InitRange ? "+" + (m_Strategy.FinalRange - m_Strategy.InitRange) : "");

        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString();
        CriticalChangeTxt.text = (m_Strategy.FinalCriticalRate > m_Strategy.InitCriticalRate ?
            "+" + (m_Strategy.FinalCriticalRate - m_Strategy.InitCriticalRate) * 100 : "");

        CritDmgValue.text = m_Strategy.FinalCriticalPercentage * 100 + "%";
        CritDmgChangeTxt.text = (m_Strategy.FinalCriticalPercentage > StaticData.DefaultCritDmg ?
            "+" + Mathf.RoundToInt((m_Strategy.FinalCriticalPercentage - StaticData.DefaultCritDmg) * 100) + "%" : "");

        SplashRangeValue.text = m_Strategy.FinalSplashRange.ToString();
        SplashChangeTxt.text = (m_Strategy.FinalSplashRange > m_Strategy.InitSplashRange ?
            "+" + (m_Strategy.FinalSplashRange - m_Strategy.InitSplashRange) : "");

        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = (m_Strategy.FinalSlowRate > m_Strategy.InitSlowRate ?
            "+" + (m_Strategy.FinalSlowRate - m_Strategy.InitSlowRate) : "");

        IntensifyValue.text = Mathf.RoundToInt(m_Strategy.FinalBulletDamageIntensify * 100) + "%";

        ElementsHolder.SetElementCount(m_Strategy);
    }

    public void UpdateLevelUpInfo()
    {
        if (m_Strategy.Quality >= 3)//满级不预览
            return;
        float attackIncrease = m_Strategy.NextAttack - m_Strategy.FinalAttack;
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = (attackIncrease > 0 ? "+" + attackIncrease : "");

        float firerateIncrease = m_Strategy.NextFirarate - m_Strategy.FinalFireRate;
        SpeedValue.text = m_Strategy.FinalFireRate.ToString();
        SpeedChangeTxt.text = (firerateIncrease > 0 ? "+" + firerateIncrease : "");

        float criticalIncrease = m_Strategy.NextCriticalRate - m_Strategy.BaseCriticalRate;
        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString();
        CriticalChangeTxt.text = (criticalIncrease > 0 ? "+" + criticalIncrease * 100 : "");

        float sputteringIncrease = m_Strategy.NextSplashRange - m_Strategy.BaseSplashRange;
        SplashRangeValue.text = m_Strategy.FinalSplashRange.ToString();
        SplashChangeTxt.text = (sputteringIncrease > 0 ? "+" + sputteringIncrease : "");

        float slowRateIncrease = m_Strategy.NextSlowRate - m_Strategy.BaseSlowRate;
        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = (slowRateIncrease > 0 ? "+" + slowRateIncrease : "");

        float dmgIntentIncrease = m_Strategy.NextDmgIntentisfy - m_Strategy.BaseDamageIntensify;
        IntensifyValue.text = m_Strategy.FinalBulletDamageIntensify * 100 + "%";
        IntentsifyChangeTxt.text = (dmgIntentIncrease > 0 ? "+" + dmgIntentIncrease * 100 + "%" : "");
    }


    public void UpgradeBtnClick(int cost)
    {
        if (m_Strategy.Quality < 3 && GameManager.Instance.ConsumeMoney(cost))
        {
            m_Strategy.Quality++;
            //m_Strategy.ExtraSkillSlot++;
            m_Strategy.SetQualityValue();
            m_Strategy.Concrete.SetGraphic();
            m_Strategy.Concrete.m_ContentStruct.Quality = m_Strategy.Quality;
            AreaSetControl(m_Strategy.Attribute, 0, m_Strategy.Quality);
            UpdateRealTimeValue();
            ((RefactorTurret)(m_Strategy.Concrete)).ShowLandedEffect();

        }
    }

    public void FoldElementArea()
    {
        if (isFolding)
            return;
        isFold = !isFold;
        StartCoroutine(FoldCor());
    }


    IEnumerator FoldCor()
    {
        isFolding = true;
        foldArrowImg.rectTransform.localScale = isFold ? new Vector2(1, -1) : new Vector2(1, 1);

        RectTransform rect = ElementsHolder.GetComponent<RectTransform>();
        float sizeX = rect.sizeDelta.x;
        ElementsHolder.GetComponent<RectTransform>().DOSizeDelta(new Vector2(sizeX, isFold ? 390 : 790), 0.3f);
        AttributeArea.SetActive(isFold);
        DesArea.SetActive(isFold);
        yield return new WaitForSeconds(0.3f);
        isFolding = false;
    }


    private void FixedUpdate()
    {
        if (showingTurret)
        {
            UpdateRealTimeValue();
            UpdateSkillValues();
        }

    }


    public void CompositeBtnClick()
    {
        GameManager.Instance.CompositeShape(BluePrintGrid.SelectingGrid);
    }

    //public void SwitchBtnClick()
    //{
    //    GameManager.Instance.SwitchConcrete(m_Strategy.Concrete,GameRes.SwitchInfo)
    //}
}
