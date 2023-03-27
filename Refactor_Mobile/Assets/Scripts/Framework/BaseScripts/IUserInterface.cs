using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInterface : MonoBehaviour
{
    protected GameObject m_RootUI;
    protected bool m_Active = false;
    protected Animator anim;
    protected virtual void Awake()
    {
        m_RootUI = transform.Find("Root").gameObject;
        anim = this.GetComponent<Animator>();
    }
    public virtual void Initialize()
    {

    }
    public bool IsVisible()
    {
        return m_Active;
    }
    public virtual void Show()
    {
        m_RootUI.SetActive(true);
        m_Active = true;
    }

    public void SetActive()
    {
        m_RootUI.SetActive(true);
        m_Active = true;

    }

    public virtual void Hide()
    {
        m_RootUI.SetActive(false);
        m_Active = false;
    }

    public virtual void ClosePanel() { }

    public virtual void Release() { }
    public virtual void Update() { }
}
