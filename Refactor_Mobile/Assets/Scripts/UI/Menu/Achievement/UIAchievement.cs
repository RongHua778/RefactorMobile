using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIAchievement : IUserInterface
{
    [SerializeField] private AchievementGrid achGridPrefab = default;
    [SerializeField] private Transform achGridParent = default;
    [SerializeField] private Image achProgress = default;
    [SerializeField] private TextMeshProUGUI achProgressTxt = default;
    private List<AchievementGrid> generatedGrids = new List<AchievementGrid>();
    private Animator m_Anim;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        SetAchievements();
    }

    private void SetAchievements()
    {
        foreach (var item in generatedGrids)
        {
            Destroy(item.gameObject);
        }
        generatedGrids.Clear();
        int getAch = 0;
        foreach (var ach in AchievementManager.Instance.AchList)
        {
            AchievementGrid achGrid = Instantiate(achGridPrefab, achGridParent);
            achGrid.SetAch(ach);
            getAch += ach.IsGet ? 1 : 0;
            generatedGrids.Add(achGrid);
        }

        float progress = (float)getAch / AchievementManager.Instance.AchList.Count;
        achProgress.fillAmount = progress;
        achProgressTxt.text = string.Format("{0:N0}", progress * 100) + "%";
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
    }

    public override void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
        MenuManager.Instance.ShowMenu();
    }
}
