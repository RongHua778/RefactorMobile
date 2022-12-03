using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BattleRecipe : MonoBehaviour
{
    [SerializeField] UIRecipeSet m_UIRecipeSet = default;
    [SerializeField] TurretItemSlot turretSlotPrefab = default;
    [SerializeField] private Transform slotParent;
    private List<TurretItemSlot> currentSlots = new List<TurretItemSlot>();

    ToggleGroup m_ToggleGroup;
    public List<TurretAttribute> CurrentRecipes { get; set; }
    public void Initialize()
    {
        m_ToggleGroup = this.GetComponent<ToggleGroup>();
        SetRecipes(StaticData.Instance.ContentFactory.BattleRecipes);
    }

    public void SetRecipes(List<TurretAttribute> recipes)
    {
        foreach (var item in currentSlots)
        {
            Destroy(item.gameObject);
        }
        currentSlots.Clear();
        CurrentRecipes = recipes;
        for (int i = 0; i < recipes.Count; i++)
        {
            TurretItemSlot slot = Instantiate(turretSlotPrefab, slotParent);
            slot.SetContent(recipes[i], m_ToggleGroup);
            currentSlots.Add(slot);
        }
    }

    public void UpdateRecipes()
    {
        StaticData.Instance.ContentFactory.BattleRecipes = CurrentRecipes;
    }

    public void OpenSetPanel()
    {
        m_UIRecipeSet.Show();
        m_UIRecipeSet.m_BattleRecipe = this;
        m_UIRecipeSet.ShowRecipes(CurrentRecipes);
    }

}
