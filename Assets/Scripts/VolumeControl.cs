using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    private string volumeParameter;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private float volumeMultipler = 30f;
    [SerializeField]
    private Toggle muteToggle;
    private bool disableToggleEvent;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(HandleSliderValueChanged);
        muteToggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value);
    }

    private void HandleSliderValueChanged(float value)
    {
        if (volumeSlider.value > 0.001)
        {
            audioMixer.SetFloat(volumeParameter, Mathf.Log10(value) * volumeMultipler);
            disableToggleEvent = true;
            muteToggle.isOn = volumeSlider.value > volumeSlider.minValue;
            disableToggleEvent = false;
        }
        else
            audioMixer.SetFloat(volumeParameter, -80.0f);
    }

    private void ToggleValueChanged(bool enableSound)
    {
        if (disableToggleEvent)
            return;

        if (enableSound)
            volumeSlider.value = volumeSlider.maxValue;
        else
            volumeSlider.value = volumeSlider.minValue;
    }

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(volumeParameter, volumeSlider.value);
    }
}
