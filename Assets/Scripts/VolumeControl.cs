using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    //The multiplier by which volume changes with a slider
    public float volumeMultipler;

    //-----------------------------------------------------------------------------Music Controls
    //Music slider controls
    public string musicVolumeParameter;
    public Slider musicVolumeSlider;
    //Music mute toggle controls
    public Toggle musicMuteToggle;
    private bool disableMusicToggleEvent;

    //-----------------------------------------------------------------------------SFX Controls
    //SFX slider controls
    public string sFXVolumeParameter;
    public Slider sFXVolumeSlider;
    //SFX mute toggle controls
    public Toggle sFXMuteToggle;
    private bool disableSFXToggleEvent;

    //Overheat alarm mute toggle controls
    public string overheatAlarmVolumeParameter;
    public Toggle overheatMuteToggle;
    private float overheatAlarmVolume;
    private bool gameToggledAlarmMute;

    private void Awake()
    {
        //Register UI events for music volume slider and mute toggle
        musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSlider);
        musicMuteToggle.onValueChanged.AddListener(MusicMuteToggle);

        //Register UI events for SFX volume slider and mute toggle
        sFXVolumeSlider.onValueChanged.AddListener(SFXVolumeSlider);
        sFXMuteToggle.onValueChanged.AddListener(SFXMuteToggle);

        //Register UI event for overheat alarm mute toggle
        overheatMuteToggle.onValueChanged.AddListener(OverheatMuteToggle);
    }

    private void OnDisable()
    {
        //Record the player's preferences when the options menu is disabled
        PlayerPrefs.SetFloat(musicVolumeParameter, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(sFXVolumeParameter, sFXVolumeSlider.value);
        PlayerPrefs.SetFloat(overheatAlarmVolumeParameter, overheatAlarmVolume);
    }

    private void MusicVolumeSlider(float value)
    {
        //If the music volume slider isn't zero, change the volume of the music
        if (musicVolumeSlider.value > 0.001)
        {
            audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(value) * volumeMultipler);
            disableMusicToggleEvent = true;
            musicMuteToggle.isOn = musicVolumeSlider.value > musicVolumeSlider.minValue;
            disableMusicToggleEvent = false;
        }
        //Otherwise, mute the music's volume
        else
            audioMixer.SetFloat(musicVolumeParameter, -80.0f);
    }

    private void MusicMuteToggle(bool enableMusic)
    {
        if (disableMusicToggleEvent)
            return;

        //Set the music volume's slider based on the toggle
        if (enableMusic)
            musicVolumeSlider.value = musicVolumeSlider.maxValue;
        else
            musicVolumeSlider.value = musicVolumeSlider.minValue;
    }

    private void SFXVolumeSlider(float value)
    {
        //If the SFX volume slider isn't zero, change the volume of the SFX
        if (sFXVolumeSlider.value > 0.001)
        {
            audioMixer.SetFloat(sFXVolumeParameter, Mathf.Log10(value) * volumeMultipler);
            disableSFXToggleEvent = true;
            sFXMuteToggle.isOn = sFXVolumeSlider.value > sFXVolumeSlider.minValue;
            disableSFXToggleEvent = false;
        }
        //Otherwise, mute the SFX volume
        else
            audioMixer.SetFloat(sFXVolumeParameter, -80.0f);
    }

    private void SFXMuteToggle(bool enableSFX)
    {
        if (disableSFXToggleEvent)
            return;

        //Set the SFX volume's slider based on the toggle
        if (enableSFX)
            sFXVolumeSlider.value = sFXVolumeSlider.maxValue;
        else
            sFXVolumeSlider.value = sFXVolumeSlider.minValue;
    }

    private void OverheatMuteToggle(bool enableOverheatAlarm)
    {
        if (disableSFXToggleEvent)
            return;

        //Set the overheat alarm volume based on the toggle
        if (enableOverheatAlarm)
        {
            overheatAlarmVolume = 0f;
            audioMixer.SetFloat(overheatAlarmVolumeParameter, overheatAlarmVolume);
        }
        else
        {
            overheatAlarmVolume = -80f;
            audioMixer.SetFloat(overheatAlarmVolumeParameter, overheatAlarmVolume);
        }
    }

    void Start()
    {
        //Get the player's preferences when the game starts
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumeParameter, musicVolumeSlider.value);
        sFXVolumeSlider.value = PlayerPrefs.GetFloat(sFXVolumeParameter, sFXVolumeSlider.value);
        overheatAlarmVolume = PlayerPrefs.GetFloat(overheatAlarmVolumeParameter, overheatAlarmVolume);

        if (overheatAlarmVolume == -80.0f)
            overheatMuteToggle.isOn = false;
    }

    void Update()
    {
        //Turn mute toggles off if volume sliders are set to zero
        if (musicVolumeSlider.value == 0f)
            musicMuteToggle.isOn = false;

        if (sFXVolumeSlider.value == 0f)
        {
            sFXMuteToggle.isOn = false;

            //Check if the player hasn't opted to turn off the overheat alarm
            //and turn it off if the SFX volume slider is set to zero
            if (overheatMuteToggle.isOn == true)
            {
                overheatMuteToggle.isOn = false;
                gameToggledAlarmMute = true;
            }

            //Also disable the overheat alarm mute toggle
            overheatMuteToggle.enabled = false;
        }
        else 
        {
            //If the player didn't turn off the overheat alarm
            //and the SFX volume slider is not set to zero, turn the alarm back on
            if (gameToggledAlarmMute)
            {
                overheatMuteToggle.isOn = true;
                gameToggledAlarmMute = false;
            }

            //And enable the overheat alarm mute toggle
            overheatMuteToggle.enabled = true;
        }

        if (overheatAlarmVolume == -80.0f)
            overheatMuteToggle.isOn = false;
    }
}
