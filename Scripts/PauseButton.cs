using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject flashlight;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
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

            // Pause other game systems or functions
            // For example, you might call a function like PauseGameSystems() here
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

            // Resume other game systems or functions
            // For example, you might call a function like ResumeGameSystems() here
        }
    }
}
