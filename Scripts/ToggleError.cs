using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchPage : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentPage; 
    public GameObject targetPage;  

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentPage != null)
            currentPage.SetActive(false);

        if (targetPage != null)
            targetPage.SetActive(true);
    }
}

