using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float gameMinutes = 0f;
    private float realSeconds = 0f;

    void Start()
    {
        UpdateTimeText();
    }

    void Update()
    {
        realSeconds += Time.deltaTime;

        if (realSeconds >= 30f)
        {
            realSeconds = 0f;
            gameMinutes++;
            UpdateTimeText();
        }

        if (gameMinutes >= 360) // 6 hours = 360 minutes
        {
            gameMinutes = 0f;
            Debug.Log("6:00 AM");
        }
    }

    void UpdateTimeText()
    {
        int hours = Mathf.FloorToInt(gameMinutes / 60f) + 12; // Start from 12:00 AM
        int minutes = Mathf.FloorToInt(gameMinutes % 60f);

        string timeString = hours.ToString("00") + ":" + minutes.ToString("00") + " AM"; 
        timeText.text = timeString;
    }
}
