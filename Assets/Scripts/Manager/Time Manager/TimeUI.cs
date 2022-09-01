using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    // Dictionary that holds the equivalent standard time format of 24 hour format. Ex. 24 = 12 am
    Dictionary<int, int> standardTimeFormat = new Dictionary<int, int>()
    {
        {1, 1},
        {2, 2},
        {3,  3},
        {4, 4 },
        {5, 5 },
        {6, 6 },
        {7, 7 },
        {8, 8 },
        {9, 9 },
        {10, 10 },
        {11, 11 },
        {12, 12 },
        {13, 1 },
        {14, 2},
        {15, 3 },
        {16, 4 },
        {17, 5 },
        {18, 6 },
        {19, 7 },
        {20, 8 },
        {21, 9 },
        {22, 10 },
        {23, 11 },
        {24, 12 }
    };
    [SerializeField] private TMPro.TextMeshProUGUI m_TimeOfDay;
    [SerializeField] private TMPro.TextMeshProUGUI m_DayOfEvent;

    void OnEnable()
    {
        TimeManager.OnHourChanged += UpdateTimeUI;
        TimeManager.OnMinuteChanged += UpdateTimeUI;
    }

    void OnDisable()
    {
        TimeManager.OnHourChanged -= UpdateTimeUI;
        TimeManager.OnMinuteChanged -= UpdateTimeUI;
    }

    public void UpdateTimeUI ()
    {
        int minute = TimeManager.instance.playerData.playerTime.m_ActualMinuteInRealLife;

        // Check if day time.
        string isDaytime = TimeManager.instance.playerData.playerTime.m_IsDaytime ? "AM" : "PM";

        // Check if the minute is less than 10, if it is, then we add 0 to front of the string.
        string minuteFormat = minute < 10 ? "0" + minute.ToString() : minute.ToString();

        this.m_TimeOfDay.text = $"{standardTimeFormat[TimeManager.instance.playerData.playerTime.m_ActualHourInRealLife]}:{minuteFormat} { isDaytime }";
        this.m_DayOfEvent.text = "Day " + TimeManager.instance.playerData.playerTime.m_DayEvent;
    }
}
