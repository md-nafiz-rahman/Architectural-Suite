
// ScrollEventPasser.cs is responsible for passing of scroll events to a parent ScrollRect to handle nested scrolling effectively. 
// The script specifically handles the scroll event of inventory system as it is populated by furniture prefab buttons that blocks the scroll event to the parent scrollrect of the scrollview. 

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
