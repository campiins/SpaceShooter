using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuObj;

    [Header("Music & SFX")]

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _musicSlider, _sfxSlider;
    [SerializeField] private TMP_Text _musicVolumeTxt, _sfxVolumeTxt;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        float decibels = Mathf.Log10(volume) * 20;
        float normalizedVolume = Mathf.InverseLerp(_musicSlider.minValue, _musicSlider.maxValue, volume);

        _audioMixer.SetFloat("MusicVolume", decibels);
        _musicVolumeTxt.text = Mathf.RoundToInt(normalizedVolume * 100).ToString();
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        float decibels = Mathf.Log10(volume) * 20;
        float normalizedVolume = Mathf.InverseLerp(_sfxSlider.minValue, _sfxSlider.maxValue, volume);
        
        _audioMixer.SetFloat("SfxVolume", decibels);
        _sfxVolumeTxt.text = Mathf.RoundToInt(normalizedVolume * 100).ToString();
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }

    public void ShowSettingsMenu()
    {
        _settingsMenuObj.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        _settingsMenuObj.SetActive(false);
    }
}
