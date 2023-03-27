using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecipeSet : IUserInterface
{
    [SerializeField] List<RecipeHolder> m_RecipeHolders = default;
    public BattleRecipe m_BattleRecipe;
    List<TurretAttribute> attList = new List<TurretAttribute>();

    public override void Initialize()
    {
        base.Initialize();
        foreach (var holder in m_RecipeHolders)
        {
            holder.Initialize();
        }

        foreach (var item in StaticData.Instance.ContentFactory.RefactorDIC.Values)
        {
            if (item.Rare > 0)  //  建筑不加入
                m_RecipeHolders[item.Rare - 1].AddRecipe(item);
        }
        //SaveSetting();
    }

    public void ShowRecipes(List<TurretAttribute> atts)
    {
        foreach (var holder in m_RecipeHolders)
        {
            holder.UnselectAll();   //先全部取消选中
        }
        foreach (var item in atts)
        {
            m_RecipeHolders[item.Rare-1].SetSlotSelect(item);//然后把已选择的勾选
        }
    }
    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }

    public override void ClosePanel()
    {
        if (SaveSetting())
        {
            anim.SetBool("isOpen", false);
        }
        else
        {
            TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("SELECTNOTENOUGH"));
        }
    }

    //将所有选择的配方保存为参战配方
    private bool SaveSetting()
    {
        attList.Clear();
        foreach (var item in m_RecipeHolders)
        {
            List<TurretAttribute> atts = item.GetSelectRecipe();
            if (atts.Count<=0)
            {
                //请选择至少一个配方
                return false;
            }
            foreach (var att in atts)
            {
                attList.Add(att);
            }
        }
        m_BattleRecipe.SetRecipes(attList);
        return true;
    }

}
