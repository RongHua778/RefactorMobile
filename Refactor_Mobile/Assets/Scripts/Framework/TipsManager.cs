using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TipsManager : Singleton<TipsManager>
{
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private EnemyInfoTips m_EnemyTips = default;
    [SerializeField] private BuyGroundTips m_BuyGroundTips = default;
    [SerializeField] private BossTips m_BossTips = default;
    [SerializeField] private TechInfoTips m_TechInfoTips = default;
    [SerializeField] private UnlockBonusTips m_UnlockBonusTips = default;
    [SerializeField] private MessageUI m_MessageUI = default;
    [SerializeField] private Canvas m_Canvas = default;

    private List<TextTranslator> translators = new List<TextTranslator>();


    private void Start()
    {
        m_TempTips.Initialize();
        m_TurretTips.Initialize();
        m_TrapTips.Initialize();
        m_EnemyTips.Initialize();
        m_BuyGroundTips.Initialize();
        m_BossTips.Initialize();
        m_TechInfoTips.Initialize();
        m_UnlockBonusTips.Initialize();
        m_MessageUI.Initialize();

        translators = this.GetComponentsInChildren<TextTranslator>().ToList();
    }

    public void UpdateTranslators()
    {
        foreach (var trans in translators)
        {
            trans.UpdateTrans();
        }
    }

    public void SetCanvasCam()
    {
        m_Canvas.worldCamera = Camera.main;
    }


    private void SetCanvasPos(Transform tr, Vector2 pos)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, pos, m_Canvas.worldCamera, out newPos);
        tr.position = m_Canvas.transform.TransformPoint(newPos);

    }

    public void ShowBonusTips(GameLevelInfo info)
    {
        m_UnlockBonusTips.Show();
        m_UnlockBonusTips.SetBouns(info);
    }


    public void ShowTurreTips(StrategyBase strategy, Vector2 pos, int showID)
    {
        HideTips();
        SetCanvasPos(m_TurretTips.transform, pos);
        m_TurretTips.ReadTurret(strategy, showID);
        m_TurretTips.Show();
    }

    public void ShowTrapTips(TrapAttribute att, Vector2 pos)
    {
        SetCanvasPos(m_TrapTips.transform, pos);
        m_TrapTips.ReadTrapAtt(att);
        m_TrapTips.Show();
    }

    public void ShowTrapContentTips(TrapContent trap, Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_TrapTips.transform, pos);
        m_TrapTips.ReadTrap(trap, GameRes.SwitchTrapCost);
        m_TrapTips.Show();
    }

    public void ShowEnemyTips(List<EnemyAttribute> atts,Vector2 pos)
    {
        SetCanvasPos(m_EnemyTips.transform, pos);
        m_EnemyTips.ReadEnemyAtt(atts);
        m_EnemyTips.Show();
    }

    public void ShowTechInfoTips(Technology tech, Vector2 pos,bool preview=false)
    {
        HideTips();
        SetCanvasPos(m_TechInfoTips.transform, pos);
        m_TechInfoTips.Show();
        m_TechInfoTips.SetInfo(tech,preview);
    }




    public void ShowBuyGroundTips(Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_BuyGroundTips.transform, pos);
        m_BuyGroundTips.ReadInfo(GameRes.BuyGroundCost);
        m_BuyGroundTips.Show();
    }


    public void ShowTempTips(string text, Vector3 pos)
    {
        //m_TempTips.transform.position = pos;
        SetCanvasPos(m_TempTips.transform, pos);
        m_TempTips.Show();
        m_TempTips.SendText(text);
    }

    public void HideTempTips()
    {
        m_TempTips.Hide();
    }

    public void ShowBossTips(EnemyType bossType,int nextWave,Vector2 pos)
    {
        SetCanvasPos(m_BossTips.transform, pos);
        m_BossTips.Show();
        m_BossTips.ReadSequenceInfo(bossType, nextWave);
    }
    public void HideBossTips()
    {
        m_BossTips.Hide();
    }


    public void HideTips()
    {
        m_TurretTips.CloseTips();
        m_TrapTips.CloseTips();
        m_EnemyTips.CloseTips();
        m_BuyGroundTips.CloseTips();
        m_TechInfoTips.CloseTips();
    }

    public void ShowMessage(string text)
    {
        m_MessageUI.SetText(text);
    }

}
