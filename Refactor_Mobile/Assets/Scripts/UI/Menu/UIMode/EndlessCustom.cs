using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessCustom : MonoBehaviour
{
    [SerializeField] BattleRule m_BattleRule = default;
    [SerializeField] BattleRecipe m_BattleRecipe = default;
    public void Initialize()
    {
        m_BattleRecipe.Initialize();
    }

    public void EndlessModeStart()
    {
        m_BattleRecipe.UpdateRecipes();
        m_BattleRule.UpdateRules();

        LevelManager.Instance.StartNewGame(11);//11简单无尽，12困难无尽
    }
}
