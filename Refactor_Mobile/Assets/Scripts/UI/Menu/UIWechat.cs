using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWechat : IUserInterface
{
    Animator anim;

    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }
    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
    }

    public override void ClosePanel()
    {
        anim.SetBool("isOpen", false);
    }
}
