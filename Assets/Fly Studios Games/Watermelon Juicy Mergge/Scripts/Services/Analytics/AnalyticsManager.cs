using System.Collections.Generic;
using UnityEngine;

namespace Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        private static AnalyticsManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            GameStatistics.SessionStart();
            AnalyticsEvents.LogSessionStart();
        }

        public static void LogEvent(string name, Dictionary<string, object> parameters)
        {
            if (!Application.isEditor)
            {
                GameFirebase.Analytics.LogEvent(name, parameters);
                AppmetricaAnalytics.LogEvent(name, parameters);
            }
            else
            {
                Debug.Log($"<color=orange>Log event {name}</color>");
            }
        }
    }
}
