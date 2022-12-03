using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecipeHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rareTxt = default;
    [SerializeField] int rareLevel = default;
    [SerializeField] RecipeSlot recipeSlotPrefab = default;

    ToggleGroup toggleGroup;
    Transform m_RecipeParent;

    List<RecipeSlot> m_RecipeSlots = new List<RecipeSlot>();
    public void Initialize()
    {
        m_RecipeParent = transform.Find("RecipeParent");
        toggleGroup = this.GetComponent<ToggleGroup>();
        rareTxt.text = GameMultiLang.GetTraduction("TURRETLEVEL") + rareLevel + GameMultiLang.GetTraduction("RECIPE");
    }

    public void AddRecipe(TurretAttribute att)
    {
        RecipeSlot rSlot = Instantiate(recipeSlotPrefab, m_RecipeParent);
        m_RecipeSlots.Add(rSlot);

        rSlot.Initialize(att, toggleGroup);
    }
    public void UnselectAll()
    {
        foreach (var slot in m_RecipeSlots)
        {
            slot.OnSelect(false);
        }
    }
    public void SetSlotSelect(TurretAttribute att)
    {
        foreach (var slot in m_RecipeSlots)
        {
            if (slot.m_Att == att)
            {
                slot.OnSelect(true);
                break;
            }
        }
    }

    public List<TurretAttribute> GetSelectRecipe()
    {
        List<TurretAttribute> returnList = new List<TurretAttribute>();
        foreach (var item in m_RecipeSlots)
        {
            if (item.IsSelected)
                returnList.Add(item.m_Att);
        }
        return returnList;
    }

}
