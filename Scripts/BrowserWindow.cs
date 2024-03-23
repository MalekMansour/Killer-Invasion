using UnityEngine;
using UnityEngine.EventSystems;

public class BrowserWindow : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    private RectTransform windowTransform;
    private Vector2 offset;

    private void Start()
    {
        windowTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = eventData.position - (Vector2)windowTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = eventData.position - offset;
        windowTransform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}

