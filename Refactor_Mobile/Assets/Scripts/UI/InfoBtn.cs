using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour,IPointerClickHandler
{
    [TextArea(2, 3)]
    [SerializeField] string content = default;
    [SerializeField] Vector2 offset = default;

    public void SetContent(string content)
    {
        this.content = content;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (content == "")
            return;
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);

        TipsManager.Instance.ShowTempTips(content, pos + offset);
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (content == "")
    //        return;
    //    Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);

    //    TipsManager.Instance.ShowTempTips(content, pos + offset);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    TipsManager.Instance.HideTempTips();
    //}


}
