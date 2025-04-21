using UnityEngine;

public static class DisplayTimeFormat
{
    public static string DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int days = Mathf.FloorToInt(timeToDisplay / 86400);
        int hours = Mathf.FloorToInt((timeToDisplay % 86400) / 3600);
        int minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        string timeTextFormat = "";

        if (days > 0)
        {
            timeTextFormat = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", days, hours, minutes, seconds);
        }
        else if (hours > 0)
        {
            timeTextFormat = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            timeTextFormat = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timeTextFormat = string.Format("{0:00}", seconds);
        }

        return timeTextFormat;
    }
}
