using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventPermeater : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler//,IPointerEnterHandler
{
    // �¼���͸����
    [SerializeField] private GameObject m_Target;
    [SerializeField] GameObject eventPermeaterMask = default;

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            m_Target = target;
            eventPermeaterMask.SetActive(true);
        }
        else
        {
            eventPermeaterMask.SetActive(false);
        }
    }
    // ��������
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    // ����̧��
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    //�������
    public void OnPointerClick(PointerEventData eventData)
    {
        //PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }
    ////��������
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    //}


    // ���¼�͸��ȥ
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (m_Target == results[i].gameObject)
            {
                // �����Ŀ�����壬����¼�͸����ȥ��Ȼ��break
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                break;
            }
        }
    }


}
