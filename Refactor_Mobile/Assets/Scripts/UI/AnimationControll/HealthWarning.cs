using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWarning : IUserInterface
{

    private Animator anim;
    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        anim.SetTrigger("Show");
    }
}
