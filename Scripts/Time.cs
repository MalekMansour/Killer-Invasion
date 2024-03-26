using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Reference to the TextMeshPro UI component

    private const int StartHour = 8; // 8:00 PM
    private const int EndHour = 6; // 6:00 AM
    private const float TimeSpeed = 20f; // Update time every 20 seconds

    private int currentHour;
    private int currentMinute;
    private float timer;
    private bool isAM;

    private void Start()
    {
        // Initialize time to start hour and minute
        currentHour = StartHour;
        currentMinute = 0;
        timer = 0f;
        isAM = false; // Initially PM

        // Update time display initially
        UpdateTimeDisplay();
    }

    private void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check if it's time to update the time
        if (timer >= TimeSpeed)
        {
            // Reset timer
            timer = 0f;

            // Increment minute
            currentMinute++;

            // Check if hour needs to be incremented
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                // Check if hour exceeds 12, change PM to AM
                if (currentHour == 12)
                {
                    isAM = !isAM; // Toggle AM/PM
                }

                // Check if hour exceeds 12-hour format
                if (currentHour > 12)
                {
                    currentHour = 1; // Reset hour to 1 in 12-hour format
                }
            }

            // Update time display
            UpdateTimeDisplay();
        }
    }

    private void UpdateTimeDisplay()
    {
        // Determine AM/PM suffix
        string amPm = isAM ? "PM" : "AM";

        // Convert 24-hour format to 12-hour format
        int displayHour = currentHour > 12 ? currentHour - 12 : currentHour;
        if (displayHour == 0) displayHour = 12; // Handle midnight (12 AM)

        // Update TextMeshPro UI component with the current time in 12-hour format
        timeText.text = $"{displayHour:D2}:{currentMinute:D2} {amPm}";
    }
}
