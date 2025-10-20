using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume(musicSlider, "musicVolume", "music");
        }
        else
        {
            InitSlider(musicSlider, "music");
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume(fxSlider, "sfxVolume", "sfx");
        }
        else
        {
            InitSlider(fxSlider, "sfx");
        }

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        fxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void InitSlider(Slider slider, string param)
    {
        float currentVolume;
        if (audioMixer.GetFloat(param, out currentVolume))
        {
            slider.value = Mathf.Pow(10, currentVolume / 20);
        }
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(value, "music", "musicVolume");
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(value, "sfx", "sfxVolume");
    }

    private void SetVolume(float value, string mixerParam, string saveKey)
    {
        if (value <= 0.0001f)
            value = 0.0001f; 

        float volumeInDb = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(mixerParam, volumeInDb);
        PlayerPrefs.SetFloat(saveKey, value);
        PlayerPrefs.Save(); 
    }

    private void LoadVolume(Slider slider, string saveKey, string mixerParam)
    {
        float savedVolume = PlayerPrefs.GetFloat(saveKey);
        slider.value = savedVolume;
        
        if (savedVolume <= 0.0001f)
            savedVolume = 0.0001f;
            
        float volumeInDb = Mathf.Log10(savedVolume) * 20;
        audioMixer.SetFloat(mixerParam, volumeInDb);
    }
}