using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollEventPasser : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    private ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnEndDrag(eventData);
    }
    public void OnScroll(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnScroll(eventData);
    }
}
