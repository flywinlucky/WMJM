using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace GameFirebase
{
    public class FirebaseInitializer : MonoBehaviour
    {
        private static FirebaseApp _app;

        private static bool _initialized = false;
        public static bool Initialized => _initialized;

        private void Awake()
        {
            if (_initialized)
                return;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;

                    Analytics.Initialize();
                    RemoteConfig.Initialize();

                    _initialized = true;
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));

                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }
    }
}

