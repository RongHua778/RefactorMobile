using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAchievement : IUserInterface
{
    private Animator m_Anim;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
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
