using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossComeAnim : IUserInterface
{
    //[SerializeField] Image lineMaterial1 = default;
    [SerializeField] Material material = default;
    private Vector2 flowSpeed = new Vector2(0.2f, 0);


    public override void Show()
    {
        base.Show();
        anim.SetTrigger("BossCome");
        Sound.Instance.PlayUISound("Sound_Warning");
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_Active)
            material.mainTextureOffset += flowSpeed * Time.deltaTime;
    }
}
