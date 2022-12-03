using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempTips : IUserInterface
{
    //public Canvas myCanvas;
    public RectTransform rect;
    public Image boxFrame;
    public Text HorizontalText;
    public Text VerticalText;

    public int minFrameWidth = 100;
    public int maxFrameWidth = 600;

    public int rowHeight = 80;

    public Color textColor = new Color(0, 0, 0, 1);
    public Color hideColor = new Color(0, 0, 0, 0);

    private float offsetDistance;



    public override void Initialize()
    {
        base.Initialize();
        rect = this.GetComponent<RectTransform>();
        offsetDistance = boxFrame.rectTransform.sizeDelta.y - HorizontalText.rectTransform.sizeDelta.y / 2;
        VerticalText.color = hideColor;
        HorizontalText.color = textColor;
    }

    private void AdjustDialogBoxSize()
    {
        if (VerticalText.rectTransform.sizeDelta.y > rowHeight)
        {
            VerticalText.color = textColor;
            HorizontalText.color = hideColor;
            boxFrame.rectTransform.sizeDelta = new Vector2(minFrameWidth + VerticalText.rectTransform.sizeDelta.x / 2, VerticalText.rectTransform.sizeDelta.y + offsetDistance);
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(minFrameWidth + VerticalText.rectTransform.sizeDelta.x / 2, VerticalText.rectTransform.sizeDelta.y + offsetDistance);
        }
        else
        {
            boxFrame.rectTransform.sizeDelta = new Vector2(minFrameWidth + HorizontalText.rectTransform.sizeDelta.x / 2, HorizontalText.rectTransform.sizeDelta.y / 2 + offsetDistance);
        }

    }

    public void SendText(string input)
    {
        VerticalText.text = input;
        HorizontalText.text = input;
        StartCoroutine(AdjustSize());
    }

    //public void SetPos(Vector2 pos)
    //{
    //    //Vector2 NewPos = pos + new Vector2(0, rect.sizeDelta.y / 2 + 30);

    //    Vector2 newPos;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, pos, myCanvas.worldCamera, out newPos);
    //    transform.position = myCanvas.transform.TransformPoint(newPos);
    //}

    IEnumerator AdjustSize()
    {
        yield return new WaitForEndOfFrame();
        AdjustDialogBoxSize();
        //SetPos(pos);
    }



}
