using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StrategyType
{
    Element, Composite, Building
}

public class StrategyBase
{
    //�ֶ�
    public ConcreteContent Concrete;
    public TurretAttribute Attribute;
    private int quality = 0;
    public int Quality { get => quality; set => quality = value; }


    public StrategyBase(TurretAttribute attribute, int quality)
    {
        this.Attribute = attribute;
        this.Quality = quality;
        this.RangeType = attribute.RangeType;
        SetQualityValue();//��������
    }


    #region ��������
    private float initAttack;
    private float initSpeed;
    private int initRange;
    private float initCriticalRate;
    private float initSputteringRange;
    private float initSlowRate;
    private float initDamageIntensify;
    public float InitAttack { get => initAttack; set => initAttack = value; }
    public float InitFireRate { get => initSpeed; set => initSpeed = value; }
    public int InitRange { get => initRange; set => initRange = value; }
    public float InitCriticalRate { get => initCriticalRate; set => initCriticalRate = value; }
    public float InitSplashRange { get => initSputteringRange; set => initSputteringRange = value; }
    public float InitSlowRate { get => initSlowRate; set => initSlowRate = value; }
    public float InitDamageIntensify { get => initDamageIntensify; set => initDamageIntensify = value; }
    #endregion
    #region ��������

    private long turnDamage;//�غ����˺�
    public long TurnDamage { get => turnDamage; set => turnDamage = value; }
    private long totalDamage;//ȫ�����˺�
    public long TotalDamage { get => totalDamage; set => totalDamage = value; }

    public int InitSkillSLot = 3;//Ԫ�ؼ��ܲ���
    public int PrivateExtraSlot = 0;
    public int FinalExtraSlot => PrivateExtraSlot;// Mathf.Max(PrivateExtraSlot, TempExtraSlot);

    public bool ContainTurretBuffSkill { get; set; }

    public int ElementSKillSlot { get => Mathf.Min(5, InitSkillSLot + FinalExtraSlot); }
    private RangeType rangeType = RangeType.Circle;
    public RangeType RangeType
    {
        get => rangeType;
        set
        {
            rangeType = value;
            switch (rangeType)
            {
                case RangeType.Circle:
                case RangeType.HalfCircle:
                    CheckAngle = 10f;
                    RotSpeed = 12f;
                    break;
                case RangeType.Line:
                    CheckAngle = 45f;
                    RotSpeed = 0f;
                    break;
            }
        }
    }
    private float initSplashPercentage = 0.5f;//�����˺���
    private float initSlowPercentage = 0.5f;//���������
    private int initTargetCount = 1;//Ŀ����
    private float rotSpeed = 15f;//����ת��
    private float checkAngle = 10f;//�������Ƕ�
    private float timeModify = 1;//�غ���ʱ��ӳ�����
    private float bulletSize = 0;//��͸�ӵ�����ӳ�
    public float BulletSize { get => bulletSize; set => bulletSize = value; }
    public float UpgradeDiscount { get; set; }  // �����ۿ�
    public float TimeModify { get => timeModify; set => timeModify = value; }
    private int shootTriggerCount = 1;//������Ч��������
    public int ShootTriggerCount { get => shootTriggerCount; set => shootTriggerCount = value; }
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    public float CheckAngle { get => checkAngle; set => checkAngle = value; }

    private int forbidRange;
    public int ForbidRange { get => forbidRange; set => forbidRange = value; }

    private float maxFirerate = 30f;
    public float MaxFireRate { get => maxFirerate; set => maxFirerate = value; }

    private int maxRange = 50;
    public int MaxRange { get => maxRange; set => maxRange = value; }

    private static float coordinatorMaxIntensify = 1.2f;
    public static float CoordinatorMaxIntensify { get => coordinatorMaxIntensify; set => coordinatorMaxIntensify = value; }
    //Эͬ�ڹ����ӳ�
    private static float cooporativeAttackIntensify;
    public static float CooporativeAttackIntensify { get => cooporativeAttackIntensify; set => cooporativeAttackIntensify = value; }

    #endregion
    #region Ԫ������
    //����Ԫ������
    public int BaseGoldCount;
    public int BaseWoodCount;
    public int BaseWaterCount;
    public int BaseFireCount;
    public int BaseDustCount;
    public int TotalBaseCount => BaseGoldCount + BaseWoodCount + BaseWaterCount + BaseFireCount + BaseDustCount;



    //��ʱԪ������
    public int TempGoldCount;
    public int TempWoodCount;
    public int TempWaterCount;
    public int TempFireCount;
    public int TempDustCount;
    //��Ԫ������
    public int GoldCount => BaseGoldCount + TempGoldCount;
    public int WoodCount => BaseWoodCount + TempWoodCount;
    public int WaterCount => BaseWaterCount + TempWaterCount;
    public int FireCount => BaseFireCount + TempFireCount;
    public int DustCount => BaseDustCount + TempDustCount;
    public int TotalElementCount => GoldCount + WoodCount + WaterCount + FireCount + DustCount;
    #endregion

    #region Ԫ�ؼӳ�

    public float AttackPerGold => StaticData.Instance.GoldAttackIntensify * StaticData.ElementBenefit[Mathf.Min(((GoldCount - 1) / 2), 3)];
    public float FireratePerWood => StaticData.Instance.WoodFirerateIntensify * StaticData.ElementBenefit[Mathf.Min(((WoodCount - 1) / 2), 3)];
    public float SlowPerWater => StaticData.Instance.WaterSlowIntensify * StaticData.ElementBenefit[Mathf.Min(((WaterCount - 1) / 2), 3)];
    public float CritPerFire => StaticData.Instance.FireCritIntensify * StaticData.ElementBenefit[Mathf.Min(((FireCount - 1) / 2), 3)];
    public float SplashPerDust => StaticData.Instance.DustSplashIntensify * StaticData.ElementBenefit[Mathf.Min(((DustCount - 1) / 2), 3)];

    public float ElementAttackIntensify => AttackPerGold * GoldCount;
    public float ElementFirerateIntensify => FireratePerWood * WoodCount;
    public float ElementSlowIntensify => SlowPerWater * WaterCount;
    public float ElementCritIntensify => CritPerFire * FireCount;
    public float ElementSplashIntensify => SplashPerDust * DustCount;

    #endregion
    #region ��������
    public float AttackAdjust { get; set; }
    public float FireRateAdjust { get; set; }
    public float SlowRateAdjust { get; set; }
    public float SplashRangeAdjust { get; set; }


    #endregion

    #region �����̶��ӳ�
    private float baseFixAttack;
    public float BaseFixAttack { get => baseFixAttack; set => baseFixAttack = value; }
    private float baseFixFirerate;
    public float BaseFixFirerate { get => baseFixFirerate; set => baseFixFirerate = value; }
    private float baseFixSlow;
    public float BaseFixSlow { get => baseFixSlow; set => baseFixSlow = value; }
    private float baseFixSplash;
    public float BaseFixSplash { get => baseFixSplash; set => baseFixSplash = value; }
    private float baseFixCrit;
    public float BaseFixCritRate { get => baseFixCrit; set => baseFixCrit = value; }
    private int baseFixRange;
    public int BaseFixRange { get => baseFixRange; set => baseFixRange = value; }

    private float baseFixDamageIntensify = 0;
    public float BaseFixDamageIntensify { get => baseFixDamageIntensify; set => baseFixDamageIntensify = value; }

    private float baseFixFrostResist = 0;
    public float BaseFixFrostResist { get => baseFixFrostResist; set => baseFixFrostResist = value; }

    private int baseFixTargetCount;
    public int BaseFixTargetCount { get => baseFixTargetCount; set => baseFixTargetCount = value; }

    private float baseFixBulletEffectIntensify = 0;
    public float BaseFixBulletEffectIntensify { get => baseFixBulletEffectIntensify; set => baseFixBulletEffectIntensify = value; }
    #endregion

    #region ��������
    public float BaseAttack { get => InitAttack + BaseFixAttack + TurnFixAttack; }
    public float BaseFirerate { get => InitFireRate + BaseFixFirerate + TurnFixFirerate; }
    public int BaseRange { get => InitRange + BaseFixRange + TurnFixRange; }
    public float BaseCriticalRate { get => InitCriticalRate + BaseFixCritRate + ElementCritIntensify + TurnFixCriticalRate; }
    public float BaseCriticalPercentage { get => StaticData.DefaultCritDmg + TurnFixCriticalPercentage; }
    public float BaseSplashRange { get => InitSplashRange + BaseFixSplash + ElementSplashIntensify + TurnFixSplashRange; }
    public float BaseSplashPercentage { get => initSplashPercentage + TurnFixSplashPercentage; }
    public float BaseSlowPercentage { get => initSlowPercentage + TurnFixSlowRatePercentage; }
    public float BaseSlowRate { get => InitSlowRate + BaseFixSlow + ElementSlowIntensify + TurnFixSlowRate; }
    public int BaseTargetCount { get => initTargetCount + BaseFixTargetCount + TurnFixTargetCount; }


    public float BaseDamageIntensify { get => InitDamageIntensify + BaseFixDamageIntensify; }

    #endregion

    #region �ӳ��ܺ�
    public float TotalAttackIntensify => ElementAttackIntensify + AttackIntensify + TurnAttackIntensify + Mathf.Min(CoordinatorMaxIntensify, CooporativeAttackIntensify);

    public float TotalFirerateIntensify => ElementFirerateIntensify + FirerateIntensify + TurnFireRateIntensify;
    #endregion
    #region ��������
    public float FinalAttack { get => Mathf.Max(0, BaseAttack * (1 + TotalAttackIntensify)); }
    public float FinalFireRate { get => Mathf.Min(MaxFireRate, BaseFirerate * (1 + TotalFirerateIntensify)); }
    public virtual int FinalRange { get => Mathf.Min(MaxRange, BaseRange); }
    public float FinalCriticalRate { get => BaseCriticalRate; }
    public float FinalCriticalPercentage { get => BaseCriticalPercentage + FinalCriticalRate * 2f; }

    public float FinalSplashRange { get => BaseSplashRange; }
    public float FinalSplashPercentage { get => BaseSplashPercentage; }
    public float FinalSlowRate { get => BaseSlowRate; }

    public float FianlSlowPercentageOfSplash { get => BaseSlowPercentage; }

    public int FinalTargetCount { get => BaseTargetCount; }

    public float FinalBulletDamageIntensify { get => BaseDamageIntensify + TurnFixDamageBonus; }

    public float FinalBulletSize { get => BulletSize + TurnBulletSize; }

    public float FinalFrostResist { get => TurnFrostResist + BaseFixFrostResist; }

    public float FinalBulletEffectIntensify { get => BaseFixBulletEffectIntensify + TurnFixBulletEffectIntensify; }
    #endregion
    #region ȫ�ּӳ�
    //private float finalAttackIntensify;
    //public float FinalAttackIntensify { get => finalAttackIntensify; set => finalAttackIntensify = value; }
    //private float finalFirerateIntensify;
    //public float FinalFirerateIntensify { get => finalFirerateIntensify; set => finalFirerateIntensify = value; }

    //private float finalSlowIntensify;
    //public float FinalSlowIntensify { get => finalSlowIntensify; set => finalSlowIntensify = value; }

    //private float finalRangeIntensify;
    //public float FinalRangeIntensify { get => finalRangeIntensify; set => finalRangeIntensify = value; }

    //private float finalSplashIntensify;
    //public float FinallSplashIntensify { get => finalSplashIntensify; set => finalSplashIntensify = value; }
    #endregion

    #region ս���й̶��ӳ�
    private float turnFixAttack;
    private float turnFixSpeed;
    private int turnFixRange;
    private float turnFixSplashRange;
    private float turnFixSplashPercentage;
    private float turnFixCriticalRate;
    private float turnFixCriticalPercentage;
    private float turnFixSlowRate;
    private float turnFixSlowRatePercentage;
    private int turnFixTargetCount;
    private float turnFixDamageBonus;
    private float turnFixBulletEffectIntensify;
    public float TurnFixDamageBonus { get => turnFixDamageBonus; set => turnFixDamageBonus = value; }

    public float TurnFixAttack { get => turnFixAttack; set => turnFixAttack = value; }
    public float TurnFixFirerate { get => turnFixSpeed; set => turnFixSpeed = value; }
    public int TurnFixRange { get => turnFixRange; set => turnFixRange = value; }
    public float TurnFixSplashRange { get => turnFixSplashRange; set => turnFixSplashRange = value; }
    public float TurnFixSplashPercentage { get => turnFixSplashPercentage; set => turnFixSplashPercentage = value; }
    public float TurnFixCriticalRate { get => turnFixCriticalRate; set => turnFixCriticalRate = value; }
    public float TurnFixCriticalPercentage { get => turnFixCriticalPercentage; set => turnFixCriticalPercentage = value; }
    public float TurnFixSlowRate { get => turnFixSlowRate; set => turnFixSlowRate = value; }
    public float TurnFixSlowRatePercentage { get => turnFixSlowRatePercentage; set => turnFixSlowRatePercentage = value; }
    public int TurnFixTargetCount { get => turnFixTargetCount; set => turnFixTargetCount = value; }
    public float TurnFixBulletEffectIntensify { get => turnFixBulletEffectIntensify; set => turnFixBulletEffectIntensify = value; }

    #endregion

    #region �����ٷֱȼӳ�
    private float attackIntensify;
    private float firerateIntensify;
    public float AttackIntensify { get => attackIntensify; set => attackIntensify = value; }
    public float FirerateIntensify { get => firerateIntensify; set => firerateIntensify = value; }

    #endregion

    #region ս���аٷֱȼӳ�
    private float turnAttackIntensify = 0;
    private float turnSpeedIntensify = 0;
    //private float turnCritRateIntensify = 0;
    //private float turnSplashRangeIntensify = 0;
    //private float turnSlowRateIntensify = 0;
    private float turnBulletSize = 0;
    private float turnFrostResist = 0;
    public float TurnFrostResist { get => turnFrostResist; set => turnFrostResist = value; }
    public virtual float TurnAttackIntensify { get => turnAttackIntensify; set => turnAttackIntensify = value; }
    public float TurnFireRateIntensify { get => turnSpeedIntensify; set => turnSpeedIntensify = value; }
    //public float TurnCritRateIntensify { get => turnCritRateIntensify; set => turnCritRateIntensify = value; }
    //public float TurnSplashRangeIntensify { get => turnSplashRangeIntensify; set => turnSplashRangeIntensify = value; }
    //public float TurnSlowRateIntensify { get => turnSlowRateIntensify; set => turnSlowRateIntensify = value; }
    public float TurnBulletSize { get => turnBulletSize; set => turnBulletSize = value; }

    #endregion


    #region �¼�����
    public float NextAttack { get => (Attribute.TurretLevels[Quality].AttackDamage + TurnFixAttack + BaseFixAttack) * (1 + TotalAttackIntensify); }
    public float NextFirarate { get => (Attribute.TurretLevels[Quality].AttackSpeed + TurnFixFirerate + BaseFixFirerate) * (1 + TotalFirerateIntensify); }
    public float NextSplashRange { get => Attribute.TurretLevels[Quality].SplashRange + ElementSplashIntensify; }
    public float NextCriticalRate { get => Attribute.TurretLevels[Quality].CriticalRate + ElementCritIntensify; }
    public float NextSlowRate { get => Attribute.TurretLevels[Quality].SlowRate + ElementSlowIntensify; }

    public float NextDmgIntentisfy { get => Attribute.TurretLevels[Quality].DamageIntensify; }
    #endregion

    public TurretSkill TurretSkill { get; set; }

    public List<TurretSkill> TurretSkills = new List<TurretSkill>();

    public List<GlobalSkill> GlobalSkills = new List<GlobalSkill>();

    //�ӵ�BUFF
    //public List<TurretBuffInfo> BulletBuffInfos = new List<TurretBuffInfo>();


    public virtual void GetTurretSkills()//�״λ�ȡ��������Ч��
    {
        TurretSkills.Clear();

        TurretSkill effect = TurretSkillFactory.GetInitialSkill((int)Attribute.RefactorName);//�Դ�����
        TurretSkill = effect;
        TurretSkill.strategy = this;
        TurretSkills.Add(effect);
        TurretSkill.Build();

    }

    public void GainRandomTempElement(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int random = UnityEngine.Random.Range(0, 5);
            switch (random)
            {
                case 0:
                    TempGoldCount++;
                    break;
                case 1:
                    TempWoodCount++;
                    break;
                case 2:
                    TempWaterCount++;
                    break;
                case 3:
                    TempFireCount++;
                    break;
                case 4:
                    TempDustCount++;
                    break;
            }
        }
    }

    public void AddElementSkill(ElementSkill skill)
    {
        if (skill == null)
            return;
        skill.strategy = this;
        TurretSkills.Add(skill);
        skill.Build();
    }

    public void AddGlobalSkill(GlobalSkill skill)
    {
        skill.strategy = this;
        GlobalSkills.Add(skill);
        skill.Build();
    }

    public void OnEquipSkill()
    {
        foreach (var skill in TurretSkills.ToList())
        {
            skill.OnEquip();
        }
    }

    public virtual void SetQualityValue()
    {
        InitAttack = Attribute.TurretLevels[Quality - 1].AttackDamage;//���Ļ��ڻغ������������㹥���������Ը�Ϊ�ӷ�
        InitFireRate = Attribute.TurretLevels[Quality - 1].AttackSpeed;
        InitRange = Attribute.TurretLevels[Quality - 1].AttackRange;
        InitCriticalRate = Attribute.TurretLevels[Quality - 1].CriticalRate;
        InitSplashRange = Attribute.TurretLevels[Quality - 1].SplashRange;
        InitSlowRate = Attribute.TurretLevels[Quality - 1].SlowRate;
        InitDamageIntensify = Attribute.TurretLevels[Quality - 1].DamageIntensify;
        //ForbidRange = m_Att.TurretLevels[Quality - 1].ForbidRange;
    }

    public void StartTurnSkills()
    {
        foreach (var skill in TurretSkills)
        {
            skill.StartTurn();
        }
        foreach (var skill in GlobalSkills)
        {
            skill.StartTurn();
        }

    }

    public void StartTurnSkill2()
    {
        foreach (var skill in TurretSkills)
        {
            skill.StartTurn2();
        }
        foreach (var skill in GlobalSkills)
        {
            skill.StartTurn2();
        }
    }

    public void StartTurnSkill3()
    {
        foreach (var skill in TurretSkills)
        {
            skill.StartTurn3();
        }
        foreach (var skill in GlobalSkills)
        {
            skill.StartTurn3();
        }
    }


    public void ClearTurnAnalysis()
    {
        TurnDamage = 0;
    }

    public void ClearTurnIntensify()
    {
        ////�غ�ʱ������
        //TimeModify = 1;
        TempGoldCount = 0;
        TempWoodCount = 0;
        TempWaterCount = 0;
        TempFireCount = 0;
        TempDustCount = 0;

        //�غϹ̶��ӳ�
        TurnFixAttack = 0;
        TurnFixFirerate = 0;
        TurnFixRange = 0;
        TurnFixSplashRange = 0;
        TurnFixSplashPercentage = 0;
        TurnFixCriticalRate = 0;
        TurnFixCriticalPercentage = 0;
        TurnFixSlowRate = 0;
        TurnFixTargetCount = 0;
        TurnFrostResist = 0;
        TurnFixSlowRatePercentage = 0;
        TurnBulletSize = 0;
        TurnFixDamageBonus = 0;
        TurnFixBulletEffectIntensify = 0;

        //�غϰٷֱȼӳ�
        TurnAttackIntensify = 0;
        TurnFireRateIntensify = 0;
        //TurnCritRateIntensify = 0;
        //TurnSplashRangeIntensify = 0;
        //TurnSlowRateIntensify = 0;



        foreach (TurretSkill skill in TurretSkills)
        {
            skill.EndTurn();
        }

        foreach (GlobalSkill skill in GlobalSkills)
        {
            skill.EndTurn();
        }
    }

    public void DetectSkills()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Detect();//����Ч��
        }

        foreach (var skill in GlobalSkills)
        {
            skill.Detect();
        }
    }

    public void DrawTurretSkill()
    {
        foreach (var skill in TurretSkills)
        {
            skill.Draw();//��ȡЧ��
        }
    }

    public void OnEquippedSkill()//��װ��Ч��
    {
        foreach (var skill in TurretSkills)
        {
            skill.OnEquipped();
        }
        foreach (var skill in GlobalSkills)
        {
            skill.OnEquipped();
        }
    }
    public void CompositeSkill()
    {
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].Composite();
        }
        for (int i = 0; i < GlobalSkills.Count; i++)
        {
            GlobalSkills[i].Composite();
        }
    }

    public void EnterSkill(IDamage target)
    {
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].OnEnter(target);
        }
        for (int i = 0; i < GlobalSkills.Count; i++)
        {
            GlobalSkills[i].OnEnter(target);
        }
    }

    public void ExitSkill(IDamage target)
    {
        for (int i = 0; i < TurretSkills.Count; i++)
        {
            TurretSkills[i].OnExit(target);
        }
        for (int i = 0; i < GlobalSkills.Count; i++)
        {
            GlobalSkills[i].OnExit(target);
        }
    }

    public List<ElementType> GetRandomElementsOfthisTurret(int amount)
    {
        List<ElementType> returnElements = new List<ElementType>();
        List<ElementType> allElements = new List<ElementType>();
        for (int i = 0; i < GoldCount; i++)
        {
            allElements.Add(ElementType.GOLD);
        }
        for (int i = 0; i < WoodCount; i++)
        {
            allElements.Add(ElementType.WOOD);
        }
        for (int i = 0; i < WaterCount; i++)
        {
            allElements.Add(ElementType.WATER);
        }
        for (int i = 0; i < FireCount; i++)
        {
            allElements.Add(ElementType.FIRE);
        }
        for (int i = 0; i < DustCount; i++)
        {
            allElements.Add(ElementType.DUST);
        }

        List<int> returnIndex = StaticData.SelectNoRepeat(amount, allElements.Count);
        for (int i = 0; i < amount; i++)
        {
            returnElements.Add(allElements[returnIndex[i]]);
        }
        return returnElements;
    }

    public virtual void UndoStrategy()
    {

    }

    public void RemoveGlobalSkill(GlobalSkill skill)
    {
        foreach (var item in GlobalSkills)
        {
            if (item.GlobalSkillName == skill.GlobalSkillName)
            {
                GlobalSkills.Remove(item);
            }
        }
    }
}
