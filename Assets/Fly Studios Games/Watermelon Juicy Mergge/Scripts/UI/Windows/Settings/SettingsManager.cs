using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [Space]
    public AudioMixerGroup audioMixerGroup;
    [Space]
    public Toggle Sound_Toggle;
    public bool Sound_Bool; //Sound Efects Boll
    [Space]
    public Toggle Music_Toggle;
    public bool Music_Bool; //Music Sound Bool

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Sound_FX_Toggle"))
        {
            if ((PlayerPrefs.GetInt("Sound_FX_Toggle") == 1))
            {
                audioMixerGroup.audioMixer.SetFloat("sfx_volume", 0); //Set SFX volume
                Sound_Toggle.isOn = true;
                Sound_Bool = true;
            }
            else
            {
                audioMixerGroup.audioMixer.SetFloat("sfx_volume", -80); //Set SFX volume
                Sound_Toggle.isOn = false;
                Sound_Bool = false;
            }
        }

        if (PlayerPrefs.HasKey("Music_Toggle"))
        {
            if ((PlayerPrefs.GetInt("Music_Toggle") == 1))
            {
                audioMixerGroup.audioMixer.SetFloat("music_volume", 0); //Set music volume
                Music_Toggle.isOn = true;
                Music_Bool = true;
            }
            else
            {
                audioMixerGroup.audioMixer.SetFloat("music_volume", -80); //Set music volume
                Music_Toggle.isOn = false;
                Music_Bool = false;
            }
        }
    }

    private void Start()
    {
        UpdateSettingsData();
    }

    public void UpdateSoundSettings()
    {
        if (Sound_Toggle.isOn == true)
        {
            Sound_Bool = true;
            Sound_Toggle.isOn = true;
            audioMixerGroup.audioMixer.SetFloat("sfx_volume", 0); //Set SFX volume
            PlayerPrefs.SetInt("Sound_FX_Toggle", 1);
        }
        else
        {
            Sound_Bool = false;
            Sound_Toggle.isOn = false;
            audioMixerGroup.audioMixer.SetFloat("sfx_volume", -80); //Set SFX volume
            PlayerPrefs.SetInt("Sound_FX_Toggle", 0);
        }
    }

    public void UpdateMusicSettings()
    {
        if (Music_Toggle.isOn == true)
        {
            Music_Bool = true;
            Music_Toggle.isOn = true;
            audioMixerGroup.audioMixer.SetFloat("music_volume", 0); //Set music volume
            PlayerPrefs.SetInt("Music_Toggle", 1);
        }
        else
        {
            Music_Bool = false;
            Music_Toggle.isOn = false;
            audioMixerGroup.audioMixer.SetFloat("music_volume", -80); //Set music volume
            PlayerPrefs.SetInt("Music_Toggle", 0);
        }
    }

    private void UpdateSettingsData()
    {
        UpdateSoundSettings();
        UpdateMusicSettings();
    }
}