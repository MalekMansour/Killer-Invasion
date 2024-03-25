using UnityEngine;
using UnityEngine.UI;

public class MaximizeButton : MonoBehaviour
{
    public Canvas targetCanvas; 
    private bool isMaximized = false;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private BrowserWindow browserWindow;

    void Start()
    {
        originalScale = targetCanvas.transform.localScale;
        originalPosition = targetCanvas.transform.localPosition;
        browserWindow = targetCanvas.GetComponent<BrowserWindow>();
    }

    public void ToggleMaximize()
    {
        isMaximized = !isMaximized;

        if (isMaximized)
        {
            targetCanvas.transform.localScale = new Vector3(0.165f, 0.285082f, 0.5334999f);
            browserWindow.enabled = false;
            targetCanvas.transform.localPosition = new Vector3(originalPosition.x, 11.3f, originalPosition.z);
        }
        else
        {
            targetCanvas.transform.localScale = originalScale;
            browserWindow.enabled = true;
            targetCanvas.transform.localPosition = originalPosition;
        }
    }
}

