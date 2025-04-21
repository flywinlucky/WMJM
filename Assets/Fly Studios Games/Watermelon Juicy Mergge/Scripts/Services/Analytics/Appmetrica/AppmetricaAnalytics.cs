using System.Collections.Generic;

public class AppmetricaAnalytics
{
    public static void LogEvent(string name, Dictionary<string, object> parameters)
    {
        AppendAdditionalParameters(ref parameters);
        AppMetrica.Instance.ReportEvent(name, parameters);
    }

    private static void AppendAdditionalParameters(ref Dictionary<string, object> parameters)
    {
        parameters.Add("total_playtime_min",        GameStatistics.TotalPlaytimeMinutes);
        parameters.Add("total_playtime_sec",        GameStatistics.TotalPlaytimeSeconds);
        parameters.Add("days_in_game",              GameStatistics.DaysInGame);
        parameters.Add("session_number",            GameStatistics.SessionNumber);
        parameters.Add("num_games",                 GameStatistics.NumGames);
        parameters.Add("total_inter_complete",      GameStatistics.TotalInterComplete);
        parameters.Add("total_rewarded_complete",   GameStatistics.TotalRewardedComplete);
    }
}

