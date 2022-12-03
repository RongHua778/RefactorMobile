using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class BluePrintShopUI : IUserInterface
{
    private bool isRefreshing = false;
    bool Showing = false;//控制动画
    Animator anim;
    [SerializeField] private GameObject ShopBtnObj;

    [SerializeField] BluePrintGrid bluePrintGridPrefab = default;
    [SerializeField] Text NextRefreshTurnsTxt = default;
    [SerializeField] Transform shopContent = default;
    [SerializeField] Text PerfectElementTxt = default;
    [SerializeField] TextMeshProUGUI refreshCost_Txt = default;
    [SerializeField] InfoBtn perfectInfo = default;


    public static List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();//商店配方表


    int currentLock = 0;
    private TempWord RefactorTrigger;
    public int CurrentLock
    {
        get => currentLock;
        set
        {
            currentLock = value;

            //LockCountTxt.text = GameMultiLang.GetTraduction("SHOPBLUEPRINT") + CurrentLock.ToString() + "/" + GameRes.MaxLock.ToString();
        }
    }

    public int NextRefreshTrun //下次自动刷新回合
    {
        set
        {
            NextRefreshTurnsTxt.text = GameMultiLang.GetTraduction("NEXTREFRESH") + ":" + value + GameMultiLang.GetTraduction("WAVE");
        }
    }

    public int PerfectElementCount
    {
        set => PerfectElementTxt.text = value.ToString();
    }

    public int RefreshShopCost
    {
        set => refreshCost_Txt.text = "<sprite=7>" + value.ToString();
    }



    public override void Initialize()
    {
        anim = this.GetComponent<Animator>();
        CurrentLock = 0;
        RefactorTrigger = new TempWord(TempWordType.Refactor, 0);
        perfectInfo.SetContent(GameMultiLang.GetTraduction("PERFECTINFO"));
    }

    public override void Release()
    {
        base.Release();
        ShopBluePrints.Clear();
        BluePrintGrid.SelectingGrid = null;
        BluePrintGrid.RefactorGrid = null;
    }

    public void SetShopBtnActive(bool value)
    {
        ShopBtnObj.SetActive(value);
    }

    public void LoadSaveGame()
    {
        foreach (var b in LevelManager.Instance.LastGameSave.SaveBluePrints)
        {
            RefactorStrategy strategy = ConstructHelper.GetSpecificStrategyByString(b.Name, b.ElementRequirements, b.QualityRequirements);
            strategy.AddElementSkill(TurretSkillFactory.GetElementSkill(b.ElementRequirements));

            AddBluePrint(strategy);
        }
    }

    public void RefreshShop(int cost)//刷新商店
    {
        if (isRefreshing)
            return;
        if (!GameManager.Instance.ConsumeMoney(cost))
            return;

        if (BluePrintGrid.SelectingGrid != null)
        {
            BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
            BluePrintGrid.SelectingGrid = null;
        }
        BluePrintGrid.RefactorGrid = null;
        int lockNum = 0;
        foreach (var grid in ShopBluePrints.ToList())
        {
            if (!grid.IsLock)
            {
                RemoveGrid(grid);
            }
            else
            {
                lockNum++;
            }
        }
        StartCoroutine(RefreshShopCor(lockNum));
    }

    private IEnumerator RefreshShopCor(int lockNum)
    {
        isRefreshing = true;
        for (int i = 0; i < GameRes.ShopCapacity - lockNum; i++)
        {
            RefactorStrategy strategy = ConstructHelper.GetRandomRefactorStrategy();
            AddBluePrint(strategy);
            yield return new WaitForSeconds(0.02f);
        }
        isRefreshing = false;
    }
    public void AddBluePrint(RefactorStrategy strategy)//增加蓝图，isShopblueprint判断加入商店还是拥有
    {
        BluePrintGrid bluePrintGrid = ObjectPool.Instance.Spawn(bluePrintGridPrefab) as BluePrintGrid;
        bluePrintGrid.transform.SetParent(shopContent);
        bluePrintGrid.transform.localScale = Vector3.one;
        bluePrintGrid.transform.localPosition = Vector3.zero;
        bluePrintGrid.SetElements(this, shopContent.GetComponent<ToggleGroup>(), strategy);

        bluePrintGrid.ShowLockBtn(CurrentLock < GameRes.LockCount);
        ShopBluePrints.Add(bluePrintGrid);

    }


    public void OnLockGrid(bool isLock)
    {
        if (isLock)
        {
            CurrentLock++;
            if (CurrentLock >= GameRes.LockCount)
            {
                foreach (var grid in ShopBluePrints)
                {
                    if (!grid.IsLock)
                    {
                        grid.ShowLockBtn(false);
                    }
                }
            }
        }
        else
        {
            CurrentLock--;
            if (CurrentLock < GameRes.LockCount)
            {
                foreach (var grid in ShopBluePrints)
                {
                    grid.ShowLockBtn(true);
                }
            }

        }
    }

    public void ShowAllLock(int value)
    {
        if (value > CurrentLock)
        {
            foreach (var grid in ShopBluePrints)
            {
                grid.ShowLockBtn(true);
            }
        }
        else if (value == 0)
        {
            foreach (var grid in ShopBluePrints)
            {
                grid.IsLock = false;
                grid.ShowLockBtn(false);
            }
        }
        else
        {
            int diff = CurrentLock - value;
            foreach (var grid in ShopBluePrints)
            {
                if (grid.IsLock)
                {
                    if (diff > 0)
                    {
                        grid.IsLock = false;
                        grid.ShowLockBtn(false);
                        diff--;
                    }
                }
                else
                {
                    grid.ShowLockBtn(false);
                }
            }
        }
    }

    public void ShopBtnClick()//播放商店界面打开动画
    {
        if (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Challenge
            || (LevelManager.Instance.CurrentLevel.ModeType == ModeType.Standard && LevelManager.Instance.CurrentLevel.Level == 0))
            return;
        GameEvents.Instance.TutorialTrigger(TutorialType.ShopBtnClick);
        Showing = !Showing;
        if (Showing)
            Show();
        else
            Hide();
    }

    public override void Hide()
    {
        anim.SetBool("Showing", false);
    }

    public override void Show()
    {
        anim.SetBool("Showing", true);
    }



    public void RefreshBtnClick()
    {
        GameManager.Instance.RefreshShop(GameRes.RefreshShopCost);
    }



    public void RefactorBluePrint(BluePrintGrid grid)//合成对应的配方
    {
        grid.Strategy.RefactorTurret();
        RemoveGrid(grid);
        CheckAllBluePrint();
        TipsManager.Instance.HideTips();
        //GameRes.PerfectProgress++;
        BluePrintGrid.RefactorGrid = grid;

        GameEvents.Instance.TempWordTrigger(RefactorTrigger);//重构时概率出现小姐姐对白
    }

    public void RemoveGrid(BluePrintGrid grid)//移除对应的配方，并清理列表
    {
        if (ShopBluePrints.Contains(grid))
        {
            ShopBluePrints.Remove(grid);
        }
        ObjectPool.Instance.UnSpawn(grid);
    }

    public void CheckAllBluePrint()//检查所有配方是否达成合成条件
    {
        foreach (var bluePrint in ShopBluePrints)
        {
            bluePrint.CheckElements();
        }
    }


    public void PreviewComposition(bool value, ElementType element, int quality)
    {
        foreach (var blueprint in ShopBluePrints)
        {
            blueprint.PreviewElement(value, element, quality);
        }
        //foreach (var blueprint in OwnBluePrints)
        //{
        //    blueprint.PreviewElement(value, element, quality);
        //}

    }

    public void RemoveUnlockedRecipes()
    {
        foreach (var item in ShopBluePrints.ToList())
        {
            if (!item.IsLock)
                RemoveGrid(item);
        }

    }
}
