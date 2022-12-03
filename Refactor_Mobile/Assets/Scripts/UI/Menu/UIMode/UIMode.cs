using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMode : IUserInterface
{
    private Animator m_Anim;

    [SerializeField] UIStandardMode m_UIStandardMode = default;
    [SerializeField] UIEndlessMode m_UIEndlessMode = default;
    [SerializeField] UIChallengeMode m_UIChallengeMode = default;


    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        m_UIStandardMode.Initialize();
        m_UIEndlessMode.Initialize();
        m_UIChallengeMode.Initialize();
        m_UIStandardMode.gameObject.SetActive(true);
        m_UIEndlessMode.gameObject.SetActive(false);
        m_UIChallengeMode.gameObject.SetActive(false);
        Hide();

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
