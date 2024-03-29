using UnityEngine;
using TMPro;

public class GameTimeUpdater : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float gameTimeInSeconds = 0f;
    private float realTimeInSeconds = 0f;
    private float gameTimeMultiplier = 2f; // 1 minute in the game = 30 seconds in real life
    private int hours = 0;
    private int minutes = 0;
    private bool isAM = true;

    void Update()
    {
        gameTimeInSeconds += Time.deltaTime * gameTimeMultiplier;
        realTimeInSeconds += Time.deltaTime;

        if (gameTimeInSeconds >= 60f)
        {
            gameTimeInSeconds = 0f;
            minutes++;
            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
                if (hours >= 12)
                {
                    hours = 0;
                    isAM = !isAM;
                }
            }
        }

        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        string amPm = isAM ? "AM" : "PM";
        string hourString = hours.ToString("00");
        string minuteString = minutes.ToString("00");
        timeText.text = hourString + ":" + minuteString + " " + amPm;
    }
}
