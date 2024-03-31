using UnityEngine;

public class IconOpener : MonoBehaviour
{
    public GameObject browserCanvas; 

    public void ToggleBrowser()
    {
        if (browserCanvas != null)
        {
            browserCanvas.SetActive(true);

            browserCanvas.transform.SetAsLastSibling(); 
        }
    }
}
