using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStandardMode : MonoBehaviour
{
    [SerializeField] Text difficultyInfo_Txt = default;
    [SerializeField] Text difficultyTxt = default;
    [SerializeField] BattleRecipe m_BattleRecipe = default;

    private int selectDifficulty;
    private int SelectDifficulty
    {
        get => selectDifficulty;
        set => selectDifficulty = Mathf.Clamp(value, 0, LevelManager.Instance.PassDiifcutly);
    }

    public void Initialize()
    {
        m_BattleRecipe.Initialize();
        selectDifficulty = LevelManager.Instance.PassDiifcutly;
        DifficultyBtnClick(0);
    }


    public void DifficultyBtnClick(int count)
    {
        SelectDifficulty += count;
        m_BattleRecipe.gameObject.SetActive(SelectDifficulty > 1);
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + SelectDifficulty);
        difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + SelectDifficulty.ToString();

    }

    public void StandardModeStart()
    {
        m_BattleRecipe.UpdateRecipes();
        RuleFactory.Release();//标准模式无特殊规则
        LevelManager.Instance.StartNewGame(SelectDifficulty);
    }
}
