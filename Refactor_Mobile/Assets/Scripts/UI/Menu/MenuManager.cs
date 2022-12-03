using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Steamworks;

public class MenuManager : Singleton<MenuManager>
{
    Canvas m_Canvas;
    //����ϵͳ
    [SerializeField] UIMenu m_UIMenu = default;
    [SerializeField] UIMode m_UIMode = default;
    //[SerializeField] MenuMessage m_UIMessage = default;
    [SerializeField] UITujian m_UITujian = default;
    [SerializeField] UISetting m_UISetting = default;
    [SerializeField] UIBillBoard m_UIBillboard = default;
    [SerializeField] UIWechat m_UIWechat = default;
    [SerializeField] ThanksPanel m_ThanksPanel = default;

    [SerializeField] UIRecipeSet m_UIRecipeSet = default;
    [SerializeField] UIRuleSet m_UIRuleSet = default;

    [SerializeField] TechInfoTips m_TechInfoTips = default;

    public void Initinal()
    {
        m_Canvas = this.GetComponent<Canvas>();
        LevelManager.Instance.LoadGame();//ÿ�ν���˵�ҳ���Ͷ�ȡһ�δ浵
        //UI
        m_UIMenu.Initialize();
        //m_UIMessage.Initialize();
        m_UIMode.Initialize();
        m_UITujian.Initialize();
        m_UISetting.Initialize();
        m_UIBillboard.Initialize();
        m_UIWechat.Initialize();
        m_ThanksPanel.Initialize();

        m_UIRecipeSet.Initialize();
        m_UIRuleSet.Initialize();
        //TIPS
        //m_TurretTips.Initialize();
       // m_TrapTips.Initialize();
        //m_EnemyInfoTips.Initialize();
        m_TechInfoTips.Initialize();


        //SteamLeaderboard.DownloadScore();
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

    public void GameUpdate()
    {

    }

    public void OpenTujian()
    {
        m_UIMenu.ClosePanel();
        m_UITujian.Show();
    }

    //public void ShowTurretTips(TurretAttribute att, Vector2 pos)
    //{
    //    SetCanvasPos(m_TurretTips.transform, pos);
    //    m_TurretTips.Show();
    //    m_TurretTips.ReadAttribute(att);
    //}

    //public void ShowTurretTips(StrategyBase strategy, Vector2 pos,int showID)
    //{
    //    SetCanvasPos(m_TurretTips.transform, pos);
    //    m_TurretTips.Show();
    //    m_TurretTips.ReadTurret(strategy,showID);
    //}



    //public void ShowTrapTips(TrapAttribute att, Vector2 pos)
    //{
    //    SetCanvasPos(m_TrapTips.transform, pos);

    //    m_TrapTips.Show();
    //    m_TrapTips.ReadTrapAtt(att);
    //}



    //public void ShowEnemyInfoTips(EnemyAttribute att, Vector2 pos)
    //{
    //    SetCanvasPos(m_EnemyInfoTips.transform, pos);
    //    m_EnemyInfoTips.Show();
    //    m_EnemyInfoTips.ReadEnemyAtt(att);
    //}
    private void SetCanvasPos(Transform tr, Vector2 pos)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, pos, m_Canvas.worldCamera, out newPos);
        tr.position = m_Canvas.transform.TransformPoint(newPos);
    }
    public void OpenSetting()
    {
        //m_UIMenu.ClosePanel();
        m_UISetting.Show();
    }

    internal void HideTips()
    {
        //m_TurretTips.CloseTips();
        //m_TrapTips.CloseTips();
        //m_EnemyInfoTips.CloseTips();
        m_TechInfoTips.CloseTips();
    }

    //public void ShowMessage(string content)
    //{
    //    m_UIMessage.SetText(content);
    //}


    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }



}
