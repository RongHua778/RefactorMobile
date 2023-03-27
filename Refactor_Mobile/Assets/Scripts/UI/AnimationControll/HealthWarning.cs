using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWarning : IUserInterface
{


    public override void Show()
    {
        base.Show();
        anim.SetTrigger("Show");
    }
}
