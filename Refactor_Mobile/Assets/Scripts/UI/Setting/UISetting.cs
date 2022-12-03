using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UISetting : IUserInterface
{
    private Animator m_Anim;
    [SerializeField] private BasicSetting m_BasicSetting = default;
    [SerializeField] private GameSetting m_GameSetting = default;
    [SerializeField] private ShortCutsSetting m_ShortCutSetting = default;
    [SerializeField] private GameObject m_BattleContent = default;
    [SerializeField] private GameObject m_MenuContent = default;
    [SerializeField] private ThanksPanel m_ThanksPanel = default;


    [SerializeField] TextMeshProUGUI ReturnTxt = default;
    [SerializeField] TextMeshProUGUI QuitTxt = default;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        m_BasicSetting.Initialize();
        m_ShortCutSetting.Initialize();

        SetBattleContent();
    }

    public override void Show()
    {
        base.Show();
        StaticData.LockKeyboard = true;
        m_BasicSetting.ShowSetting();
        m_GameSetting.ShowSetting();
        m_BattleContent.SetActive(Game.Instance.CurrentState == "BattleState");
        m_MenuContent.SetActive(Game.Instance.CurrentState == "MenuState");
        m_Anim.SetBool("isOpen", true);
    }

    public void ThanksInfoBtnClick()
    {
        m_ThanksPanel.Show();
    }

    public override void ClosePanel()
    {
        SaveSetting();
        m_Anim.SetBool("isOpen", false);
    }

    private void SaveSetting()
    {
        if (!Game.Instance.Tutorial)
            StaticData.LockKeyboard = false;
        m_BasicSetting.SaveSetting();
    }

    private void SetBattleContent()
    {
        if (!LevelManager.Instance.CurrentLevel.CanSaveGame)
        {
            ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTUTORIAL");
            QuitTxt.text = GameMultiLang.GetTraduction("QUIT");
        }
        else
        {
            ReturnTxt.text = GameMultiLang.GetTraduction("RETURNTOMENU");
            QuitTxt.text = GameMultiLang.GetTraduction("QUIT2");
        }
    }



    

    public void RestartGameBtnClick()
    {
        SaveSetting();
        GameManager.Instance.RestartGame();
    }

    public void ReturnMenuBtnClick()
    {
        SaveSetting();
        GameManager.Instance.ReturnToMenu();
    }

    public void ExitGameBtnClick()
    {
        SaveSetting();
        GameManager.Instance.QuitGame();
    }
}
