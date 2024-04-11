using UnityEngine;

public class PauseButton : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isMouseLocked = !isMouseLocked;
            UpdateMouseLock();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause time
            pauseCanvas.SetActive(true); // Show pause canvas

            // Disable flashlight or other gameplay elements
            if (flashlight != null)
            {
                flashlight.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
        }
        else
        {
            Time.timeScale = 1f; // Resume time
            pauseCanvas.SetActive(false); // Hide pause canvas

            // Enable flashlight or other gameplay elements
            if (flashlight != null)
            {
                flashlight.SetActive(true);
            }

            UpdateMouseLock(); 
        }
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
