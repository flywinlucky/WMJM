using System;

public static class RewardUtils
{
    private static bool _triggeredOnce = false;

    public static void TryShow(Action adCall, bool canShowRewarded)
    {
        if (_triggeredOnce)
            return;

        _triggeredOnce = true;

        float probability = RemoteConfigManager.Instance.Get("inter_start_2", 0.2f);
        float roll01 = UnityEngine.Random.Range(0f, 1f);

        bool rollSuccessfull = probability > 0f && roll01 <= probability;
        bool show = !canShowRewarded && rollSuccessfull;

        if (show)
        {
            adCall?.Invoke();
        }
    }

    public static void TryShowAfter(Action adCall)
    {
        float probability = RemoteConfigManager.Instance.Get("inter_after_reward_chance", 0.2f);
        float roll01 = UnityEngine.Random.Range(0f, 1f);

        bool rollSuccessfull = probability > 0f && roll01 <= probability;

        if (rollSuccessfull)
        {
            adCall?.Invoke();
        }
    }
}
