using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using TMPro;

public class BluePrintGrid : ReusableObject//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Transform Root { get; set; }
    public static BluePrintGrid SelectingGrid = null;
    public static BluePrintGrid RefactorGrid = null;
    [SerializeField] Color UnobtainColor = default;
    [SerializeField] TextMeshProUGUI skillNameTxt = default;
    private bool isLock = false;
    public bool IsLock { get => isLock; set => isLock = value; }

    [SerializeField] Image compositeIcon = default;

    [SerializeField] Image turretBaseImg = default;
    [SerializeField] Sprite[] turretBases = default;

    [SerializeField] ElementGrid[] elementGrids = default;
    [SerializeField] Toggle m_LockToggle = default;
    private Toggle m_Toggle;
    private BluePrintShopUI m_Shop;
    public RefactorStrategy Strategy { get; set; }
    bool buildAble = false;

    private Image BGImage;
    private Tween matTween;

    private void Awake()
    {
        Root = transform.Find("Root");
        BGImage = Root.Find("BluePrintGridBG").GetComponent<Image>();
        BGImage.material = new Material(BGImage.material);
    }

    public void SetElements(BluePrintShopUI shop, ToggleGroup group, RefactorStrategy strategy)
    {
        Root.transform.position += Vector3.left * 2f;
        Root.DOLocalMoveX(2f, 0.2f);
        m_Shop = shop;
        m_Toggle = this.GetComponent<Toggle>();
        Strategy = strategy;
        Strategy.CheckElement();
        m_Toggle.group = group;
        compositeIcon.sprite = Strategy.Attribute.TurretLevels[0].TurretIcon;

        string key = "";
        foreach (var element in ((ElementSkill)strategy.TurretSkills[1]).InitElements)
        {
            key += StaticData.ElementDIC[(ElementType)element].GetElementName;
        }
        skillNameTxt.text = GameMultiLang.GetTraduction(key);

        turretBaseImg.sprite = turretBases[strategy.Attribute.Rare - 1];
        //给每一个组件设置图片
        RefreshElementsSprite();

    }

    public void PreviewElement(bool value, ElementType element, int quality)
    {
        foreach (var elementGrid in elementGrids)
        {
            elementGrid.SetPreview(value, element, quality);
        }
    }

    private void RefreshElementsSprite()
    {
        for (int i = 0; i < elementGrids.Length; i++)
        {
            if (i < Strategy.Attribute.elementNumber)
            {
                elementGrids[i].gameObject.SetActive(true);
                elementGrids[i].SetElement(Strategy.Compositions[i]);
            }
            else
            {
                elementGrids[i].gameObject.SetActive(false);
            }
        }
        buildAble = Strategy.CheckBuildable();
        if (buildAble)
        {
            matTween.Kill();
            BGImage.material.SetFloat("_ShineLocation", 0f);
            matTween = BGImage.material.DOFloat(1f, "_ShineLocation", 2f).SetLoops(-1);
        }
        else
        {
            matTween.Kill();
            BGImage.material.SetFloat("_ShineLocation", 0f);

        }
        compositeIcon.color = buildAble ? Color.white : UnobtainColor;
    }


    public void OnBluePrintSelect()
    {
        if (m_Toggle.isOn)
        {
            if (SelectingGrid != null)
                SelectingGrid.OnBluePrintDeselect();
            SelectingGrid = this;
            TipsManager.Instance.ShowTurreTips(Strategy, StaticData.LeftMidTipsPos, 2);

            if (buildAble)
            {
                Strategy.PreviewElements(true);
                GameEvents.Instance.TutorialTrigger(TutorialType.BlueprintSelect);
            }
            Sound.Instance.PlayUISound("Sound_Click");

        }
        else
        {
            if (SelectingGrid == this)
            {
                OnBluePrintDeselect();
                TipsManager.Instance.HideTips();
                //GameManager.Instance.HideTips();
                SelectingGrid = null;
            }
        }

    }



    public void OnBluePrintDeselect()
    {
        m_Toggle.isOn = false;
        if (buildAble)
            Strategy.PreviewElements(false);
    }

    public void OnLock(bool value)
    {
        IsLock = value;
        m_Shop.OnLockGrid(value);
    }

    public void CheckElements()
    {
        Strategy.CheckElement();
        RefreshElementsSprite();
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        IsLock = false;
        m_LockToggle.isOn = false;
    }

    public void ShowLockBtn(bool value)
    {
        m_LockToggle.gameObject.SetActive(value);
    }


}
