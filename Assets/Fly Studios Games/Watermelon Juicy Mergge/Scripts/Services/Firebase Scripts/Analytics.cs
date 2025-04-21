using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;

namespace GameFirebase
{
    public class Analytics
    {
        private static List<CachedEvent> _cachedEvents = new();
        private static bool _isInitialized = false;

        public static bool IsInitialized => _isInitialized;

        public static void Initialize()
        {
            _isInitialized = true;
        }

        public static void LogEvent(string name, Dictionary<string, object> data)
        {
            Parameter[] parameters = DictToParams(data);

            if (!IsInitialized)
            {
                CacheEvent(name, parameters);
                return;
            }

            LogCachedEvents();

            try
            {
                FirebaseAnalytics.LogEvent(name, parameters);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }

        #region Event caching

        private struct CachedEvent
        {
            public string name;
            public Parameter[] parameters;
        }

        private static void CacheEvent(string name, Parameter[] parameters)
        {
            Debug.Log($"Event (\"{name}\") cached");

            _cachedEvents.Add(new CachedEvent()
            {
                name = name,
                parameters = parameters
            });
        }

        private static void LogCachedEvents()
        {
            if (!IsInitialized)
                return;

            foreach(var cached in _cachedEvents)
            {
                Debug.Log($"Log cached event (\"{cached.name}\")");

                try
                {
                    FirebaseAnalytics.LogEvent(cached.name, cached.parameters);
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            }

            _cachedEvents.Clear();
        }

        #endregion

        #region Parse dictionary

        private static Parameter[] DictToParams(Dictionary<string, object> data)
        {
            if (data == null)
                return null;

            Parameter[] parameters = new Parameter[data.Count];

            int i = 0;
            foreach (KeyValuePair<string, object> pair in data)
            {
                parameters[i] = CreateParameter(pair);
                i++;
            }

            return parameters;
        }

        private static Parameter CreateParameter(KeyValuePair<string, object> pair)
        {
            if (pair.Value is int @int)
                return new Parameter(pair.Key, @int);

            if (pair.Value is float @float)
                return new Parameter(pair.Key, @float);

            if (pair.Value is string @string)
                return new Parameter(pair.Key, @string);

            return new Parameter("unknown", 0);
        }

        #endregion
    }
}
