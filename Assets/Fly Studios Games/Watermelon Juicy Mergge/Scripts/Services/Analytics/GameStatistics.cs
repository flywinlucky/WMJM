using Analytics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameStatistics
{
    private static readonly List<int> _timeStamps = 
        new() { 0, 1, 30, 60, 120, 240, 480, 960, 1800, 3600, 7200, 14400, 28800 };

    #region Properties

    public static int TotalPlaytimeMinutes
    {
        get => TotalPlaytimeSeconds / 60;
    }

    public static int TotalPlaytimeSeconds
    {
        get => PlayerPrefs.GetInt("total_playtime_sec", 0);
        set => PlayerPrefs.SetInt("total_playtime_sec", value);
    }

    public static int SessionNumber
    {
        get => PlayerPrefs.GetInt("session_number", 0);
        set => PlayerPrefs.SetInt("session_number", value);
    }

    public static DateTime RegistrationDate
    {
        get
        {
            string strDate = PlayerPrefs.GetString("registration_date");

            if (string.IsNullOrEmpty(strDate))
                return DateTime.Now;
            else 
                return DateTime.Parse(strDate);
        }
        set
        {
            PlayerPrefs.SetString("registration_date", value.ToString());
        }
    }

    public static int DaysInGame
    {
        get => (DateTime.Now - RegistrationDate).Days;
    }

    public static int NumGames
    {
        get => PlayerPrefs.GetInt("num_games", 0);
        set => PlayerPrefs.SetInt("num_games", value);
    }

    public static int TotalInterComplete
    {
        get => PlayerPrefs.GetInt("total_inter_complete", 0);
        set => PlayerPrefs.SetInt("total_inter_complete", value);
    }

    public static int TotalRewardedComplete
    {
        get => PlayerPrefs.GetInt("total_rewarded_complete", 0);
        set => PlayerPrefs.SetInt("total_rewarded_complete", value);
    }

    public static int HighestSphereNumber
    {
        get => PlayerPrefs.GetInt("highest_sphere_number_ever", 0);
        set => PlayerPrefs.SetInt("highest_sphere_number_ever", value);
    }

    public static float LastGameStartTime
    {
        get;
        set;
    }

    #endregion

    public static void SessionStart()
    {
        if (SessionNumber == 0)
            RegistrationDate = DateTime.Now;

        SessionNumber++;
        CalculateFunnel();
    }

    public static void GameStart()
    {
        LastGameStartTime = Time.time;
        NumGames++;
    }

    private static async void CalculateFunnel()
    {
        if (!Application.isPlaying)
            return;

        await Task.Delay(TimeSpan.FromSeconds(1f));
        
        int sec = ++TotalPlaytimeSeconds;
        if (_timeStamps.Contains(sec))
            AnalyticsEvents.LogTimeFunnel(sec);

        Log10Minutes(sec);

        CalculateFunnel();
    }

    private static void Log10Minutes(int seconds)
    {
        if (seconds == 600)
            AnalyticsEvents.LogMinutesPlaytime(10);
    }

    #region ECPM calculations

    public static double GetTotalRevenue(string adType)
    {
        return double.Parse(PlayerPrefs.GetString($"{adType}_total_revenue", "0"));
    }

    public static double IncTotalRevenue(string adType, double revenue)
    {
        double total = GetTotalRevenue(adType) + revenue;
        PlayerPrefs.SetString($"{adType}_total_revenue", total.ToString());

        return total;
    }

    public static int GetAdsWatched(string adType)
    {
        return PlayerPrefs.GetInt($"{adType}_ads_watched", 0);
    }

    public static int IncAdsWatched(string adType)
    {
        int watched = GetAdsWatched(adType) + 1;
        PlayerPrefs.SetInt($"{adType}_ads_watched", watched);

        return watched;
    }

    #endregion ECPM calculations
}
