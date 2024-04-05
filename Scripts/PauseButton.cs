using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu canvas

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze time
            pauseMenu.SetActive(true); // Show pause menu
        }
        else
        {
            Time.timeScale = 1f; // Unfreeze time
            pauseMenu.SetActive(false); // Hide pause menu
        }
    }
}

