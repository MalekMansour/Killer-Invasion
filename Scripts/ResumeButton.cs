using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject flashlight;

    private bool isMouseLocked = true;

    public void ResumeGame()
    {
        Time.timeScale = 1f; 
        pauseCanvas.SetActive(false); 

        if (flashlight != null)
        {
            flashlight.SetActive(true);
        }

        UpdateMouseLock();
    }

    void UpdateMouseLock()
    {
        if (isMouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 
        }
    }
}

