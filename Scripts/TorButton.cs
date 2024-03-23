using UnityEngine;

public class TorButton : MonoBehaviour
{
    public GameObject browserCanvas; 

    public void ToggleBrowser()
    {
        if (browserCanvas != null)
        {
            browserCanvas.SetActive(true); 
        }
    }
}
