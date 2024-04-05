using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause canvas menu
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
            Time.timeScale = 0f; // Freeze all scenes
            pauseMenu.SetActive(true); // Activate the pause canvas menu
        }
        else
        {
            Time.timeScale = 1f; // Unfreeze all scenes
            pauseMenu.SetActive(false); // Deactivate the pause canvas menu
        }
    }
}
