using System.Collections.Generic;

namespace Analytics
{
    public static class AnalyticsEvents
    {
        private static void LogEvent(string name, Dictionary<string, object> parameters = null)
        {
            parameters ??= new();

            AnalyticsManager.LogEvent(name, parameters);
        }

        public static void LogSessionStart()
        {
            // might need to rename this one because apparently it's already reserved?
            LogEvent("session_start");
        }

        public static void LogGameStart()
        {
            LogEvent("game_start");
        }

        public static void LogGameOver(int gameTimeSeconds)
        {
            Dictionary<string, object> parameters = new()
            {
                { "game_time_sec", gameTimeSeconds },
            };

            LogEvent("game_over", parameters);
        }

        public static void LogFirstMergeFruitEver(int fruitLevel)
        {
            Dictionary<string, object> parameters = new()
            {
                { "fruit_num", fruitLevel },
            };

            LogEvent("first_merge_fruit_ever", parameters);
        }

        public static void LogFirstMergeFruitGame(int fruitLevel)
        {
            Dictionary<string, object> parameters = new()
            {
                { "fruit_num", fruitLevel },
            };

            LogEvent("first_merge_fruit_game", parameters);
        }

        public static void LogTimeFunnel(int timeStamp)
        {
            Dictionary<string, object> parameters = new()
            {
                { "time_stamp", timeStamp },
            };

            LogEvent("time_funnel", parameters);
        }

        #region Ad events

        public static void LogVideoAdsStarted(string adType, string placement, string adNetwork)
        {
            Dictionary<string, object> parameters = new()
            {
                { "ad_type", adType },
                { "placement", placement },
                { "ad_network", adNetwork },
            };

            LogEvent("video_ads_started", parameters);
        }

        public static void LogAdRevenue(string adUnitName, string adPlatform, string adSource, string adFormat, double revenue, string currencyCode)
        {

            Dictionary<string, object> parameters = new()
            {
                { "ad_platform", adPlatform },
                { "ad_source", adSource },
                { "ad_unit_name", adUnitName },
                { "ad_format", adFormat },
                { "value", revenue },
                { "currency", currencyCode },
            };

            LogEvent("ad_impression_revenue", parameters);
        }

        public static void LogMinutesPlaytime(int minutes)
        {
            LogEvent($"playtime_{minutes}m");
        }

        public static void LogRewardedECPM(string adType, double totalRevenue, int adsWatched)
        {
            double ecpm = totalRevenue / adsWatched * 1000;

            Dictionary<string, object> properties = new()
            {
                { "ecpm", ecpm }
            };

            UnityEngine.Debug.Log($"{adType}_ecpm [{ecpm}|{totalRevenue}|{adsWatched}]");
            LogEvent($"{adType}_ecpm", properties);
        }

        #endregion
    }
}
