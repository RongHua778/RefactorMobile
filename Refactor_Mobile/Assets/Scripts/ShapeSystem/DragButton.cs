using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DraggingShape m_Shape { get; set; }
    public void OnPointerDown(PointerEventData eventData)
    {
        m_Shape.StartDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_Shape.EndDragging();
    }
}
