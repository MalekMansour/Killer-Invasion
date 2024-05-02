using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject flashlight;

    private bool isPaused = false;
    private bool isMouseLocked = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isMouseLocked = !isMouseLocked;
            UpdateMouseLock();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

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
