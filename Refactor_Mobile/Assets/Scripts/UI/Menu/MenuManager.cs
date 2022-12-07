using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Steamworks;

public class MenuManager : Singleton<MenuManager>
{
    Canvas m_Canvas;
    //界面系统
    [SerializeField] UIMenu m_UIMenu = default;
    [SerializeField] UIMode m_UIMode = default;
    [SerializeField] UITujian m_UITujian = default;
    [SerializeField] UIAchievement m_UIAchievement = default;
    [SerializeField] UISetting m_UISetting = default;
    [SerializeField] UIBillBoard m_UIBillboard = default;
    [SerializeField] UIWechat m_UIWechat = default;
    [SerializeField] ThanksPanel m_ThanksPanel = default;

    [SerializeField] UIRecipeSet m_UIRecipeSet = default;
    [SerializeField] UIRuleSet m_UIRuleSet = default;

    //[SerializeField] TechInfoTips m_TechInfoTips = default;

    public void Initinal()
    {
        m_Canvas = this.GetComponent<Canvas>();
        LevelManager.Instance.LoadGame();//每次进入菜单页，就读取一次存档
        //UI
        m_UIMenu.Initialize();
        m_UIMode.Initialize();
        m_UITujian.Initialize();
        m_UIAchievement.Initialize();
        m_UISetting.Initialize();
        m_UIBillboard.Initialize();
        m_UIWechat.Initialize();
        m_ThanksPanel.Initialize();

        m_UIRecipeSet.Initialize();
        m_UIRuleSet.Initialize();

    }

    public void Release()
    {

    }

    public void OpenBillboard(int leaderBoardType)
    {
        m_UIBillboard.ShowLeaderBoard((LeaderBoard)leaderBoardType);
    }

    public void OpenWechat()
    {
        m_UIWechat.Show();
    }

    public void OpenMode()
    {
        m_UIMenu.ClosePanel();
        m_UIMode.Show();
    }

    public void ContinueGame()
    {
        LevelManager.Instance.ContinueLastGame();
    }

    public void ShowMenu()
    {
        m_UIMenu.Show();
    }

    public void OpenAchievement()
    {
        m_UIMenu.ClosePanel();
        m_UIAchievement.Show();
    }

    public void GameUpdate()
    {

    }

    public void OpenTujian()
    {
        m_UIMenu.ClosePanel();
        m_UITujian.Show();
    }

    public void OpenSetting()
    {
        m_UISetting.Show();
    }



    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }



}
