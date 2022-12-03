using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class TMLinkArea : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI TxtArea = default;

    [SerializeField] UnityEvent m_Event = default;

    [SerializeField] TipsElementConstruct m_Ecom = default;

    private void Start()
    {
        m_Event.AddListener(() => GuideGirlSystem.Instance.ShowGuideBook(8));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Ecom.AreaID != 0)//非显示技能状态不触发
            return;
        Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(TxtArea, pos, Camera.main); //--UI相机
        if (linkIndex > -1)
        {
            m_Event.Invoke();
        }
    }
}
