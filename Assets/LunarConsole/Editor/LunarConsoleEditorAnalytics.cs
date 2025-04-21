using LunarConsolePluginInternal;
using UnityEditor;

namespace LunarConsoleEditorInternal
{
    static class LunarConsoleEditorAnalytics
    {
        private static readonly string kPrefsLastKnownVersion = Constants.EditorPrefsKeyBase + ".LastKnownVersion";


        public static void TrackPluginVersionUpdate()
        {
            if (LunarConsoleConfig.consoleEnabled && LunarConsoleConfig.consoleSupported)
            {
                var lastKnownVersion = EditorPrefs.GetString(kPrefsLastKnownVersion);
                if (lastKnownVersion != Constants.Version)
                {
                    EditorPrefs.SetString(kPrefsLastKnownVersion, Constants.Version);

                }
            }
        }


    }
}
