using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RemoteConfigManager : MonoBehaviour
{
    public static RemoteConfigManager Instance { get; private set; }

    [SerializeField] private List<DefaultValue> _defaultValues = new List<DefaultValue>();

    [Serializable]
    public class DefaultValue
    {
        [Required] public string parameterNameKey;
        [Required] public string defaultValue;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnInitialized(Action callback)
    {
        if (GameFirebase.RemoteConfig.IsDataReceived)
        {
            callback?.Invoke();
        }
        else
        {
            GameFirebase.RemoteConfig.OnDataReceived += callback;
        }
    }

    private string GetString(string key, string defaultValue)
    {
        string result = defaultValue;
        foreach (var set in _defaultValues)
        {
            if (set.parameterNameKey == key)
            {
                result = set.defaultValue;
                break;
            }
        }

        result = GameFirebase.RemoteConfig.GetString(key, result);
        return result;
    }

    public T Get<T>(string key, T defaultValue)
    {
        T value;
        string strValue = GetString(key, defaultValue.ToString());

        try
        {
            value = (T)Convert.ChangeType(strValue, typeof(T));
        }
        catch
        {
            value = defaultValue;
        }

        return value;
    }
}