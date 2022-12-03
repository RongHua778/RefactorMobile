using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThanksPanel : IUserInterface
{
    [SerializeField] RectTransform txtPanel = default;
    [SerializeField] Vector2 startPos = default;
    [SerializeField] Vector2 endPos = default;
    bool isShowing;

    public override void Show()
    {
        base.Show();
        isShowing = true;
        txtPanel.anchoredPosition = startPos;
    }

    public override void Hide()
    {
        base.Hide();
        isShowing = false;
    }
    public override void Update()
    {
        base.Update();
        RollPanel();
    }
    private void RollPanel()
    {
        if (isShowing)
        {
            txtPanel.anchoredPosition = Vector2.MoveTowards(txtPanel.anchoredPosition, endPos, 100f * Time.deltaTime);
            if (Vector2.SqrMagnitude(txtPanel.anchoredPosition - endPos) < 0.1f)
            {
                txtPanel.anchoredPosition = startPos;
            }
        }
    }
}
