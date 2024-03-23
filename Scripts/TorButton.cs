using UnityEngine;
using UnityEngine.UI;

public class TorButton : MonoBehaviour
{
    public GameObject browserCanvas; 

    private bool isBrowserOpen;

    private void Start()
    {
        isBrowserOpen = false;
        browserCanvas.SetActive(false); 
    }

    public void ToggleBrowser()
    {
        if (!isBrowserOpen) 
        {
            isBrowserOpen = true;
            browserCanvas.SetActive(true); 
        }
    }
}
