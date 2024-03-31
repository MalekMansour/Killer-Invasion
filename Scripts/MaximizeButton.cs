using UnityEngine;
using UnityEngine.UI;

public class MaximizeButton : MonoBehaviour
{
    public Canvas targetCanvas;
    public Vector3 newScale = new Vector3(5.12f, 5.12f, 1.1f); 
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
            targetCanvas.transform.localScale = newScale;
            browserWindow.enabled = false;
            targetCanvas.transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z);
        }
        else
        {
            targetCanvas.transform.localScale = originalScale;
            browserWindow.enabled = true;
            targetCanvas.transform.localPosition = originalPosition;
        }
    }
}
