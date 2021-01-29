using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : BaseUI ,IBeginDragHandler, IDragHandler
{
    protected Transform _transform;

    public void SelectTransform(Transform transform)
    {
        _transform = transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}

public interface IUIHander
{
    void SelectTransform(Transform transform);

    void OnBeginDrag(PointerEventData eventData);

    void OnDrag(PointerEventData eventData);
}
