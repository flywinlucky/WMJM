using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameFirebase
{
    public class RemoteConfig
    {
        private static Dictionary<string, string> _data = new();

        private static bool _isDataReceived = false;
        public static bool IsDataReceived => _isDataReceived;

        public static event Action OnDataReceived;

        public static void Initialize()
        {
            FetchDataAsync();
        }

        public static Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");

            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private static void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var config = FirebaseRemoteConfig.DefaultInstance;
            var info = config.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            config.ActivateAsync()
              .ContinueWithOnMainThread(
                task =>
                {
                    Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                    ParseFetchedData();
                });
        }

        private static void ParseFetchedData()
        {
            var config = FirebaseRemoteConfig.DefaultInstance;

            foreach(var set in config.AllValues)
            {
                Debug.Log($"FirebaseRemoteConfig entry: [{set.Key}:{set.Value.StringValue}]");
                _data[set.Key] = set.Value.StringValue;
            }

            _isDataReceived = true;
            OnDataReceived?.Invoke();
            OnDataReceived = null;
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (_data.ContainsKey(key))
                return _data[key];

            return defaultValue;
        }
    }
}
