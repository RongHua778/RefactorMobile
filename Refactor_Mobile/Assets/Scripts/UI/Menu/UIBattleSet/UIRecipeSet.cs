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
            if (item.Rare > 0)  //  ����������
                m_RecipeHolders[item.Rare - 1].AddRecipe(item);
        }
        //SaveSetting();
    }

    public void ShowRecipes(List<TurretAttribute> atts)
    {
        foreach (var holder in m_RecipeHolders)
        {
            holder.UnselectAll();   //��ȫ��ȡ��ѡ��
        }
        foreach (var item in atts)
        {
            m_RecipeHolders[item.Rare-1].SetSlotSelect(item);//Ȼ�����ѡ��Ĺ�ѡ
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

    //������ѡ����䷽����Ϊ��ս�䷽
    private bool SaveSetting()
    {
        attList.Clear();
        foreach (var item in m_RecipeHolders)
        {
            List<TurretAttribute> atts = item.GetSelectRecipe();
            if (atts.Count<=0)
            {
                //��ѡ������һ���䷽
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
