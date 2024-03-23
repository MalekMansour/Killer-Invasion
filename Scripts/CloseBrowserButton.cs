using UnityEngine;

public class CloseBrowserButton : MonoBehaviour
{
    public GameObject browserCanvas; 

    public void ToggleBrowser()
    {
        if (browserCanvas != null)
        {
            browserCanvas.SetActive(!browserCanvas.activeSelf); 
        }
    }
}

