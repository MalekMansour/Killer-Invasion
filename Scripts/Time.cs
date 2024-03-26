using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float gameTimeInSeconds = 0f;

    void Start()
    {
        InvokeRepeating("UpdateTime", 0f, 0.5f); // Update time every 0.5 seconds
    }

    void UpdateTime()
    {
        // Update game time based on real-time
        gameTimeInSeconds += Time.deltaTime;

        // Convert game time to minutes and seconds
        int minutes = Mathf.FloorToInt(gameTimeInSeconds / 30f); // 30 seconds per in-game minute
        int seconds = Mathf.FloorToInt(gameTimeInSeconds % 30f); // 30 seconds per in-game minute

        // Convert minutes to hours and minutes for display
        int hours = minutes / 60;
        minutes = minutes % 60;

        // Convert 24-hour format to 12-hour format
        int displayHours = hours % 12;
        if (displayHours == 0) displayHours = 12; // Handle midnight (12 AM)

        // Determine whether it's AM or PM
        string amPm = hours < 12 ? "AM" : "PM";

        // Display the time in TextMeshPro
        timeText.text = string.Format("{0:D2}:{1:D2} {2}", displayHours, minutes, amPm);

        // Reset game time after 6:00 AM
        if (hours >= 6)
        {
            gameTimeInSeconds = 0f; // Reset game time
        }
    }
}
