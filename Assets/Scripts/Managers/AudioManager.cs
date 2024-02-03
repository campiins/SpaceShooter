using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Settings _settings;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            _settings.LoadVolume();
        }
        else
        {
            _settings.SetMusicVolume();
            _settings.SetSFXVolume();
        }
    }

    public void PlayAudioClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}

[System.Serializable]
public class PlayerSounds
{
    public AudioClip fireProjectile;
    public AudioClip deathExplosion;
    public AudioClip activateShield;
    public AudioClip deactivateShield;
    public AudioClip shieldImpact;
}

[System.Serializable]
public class EnemySounds
{
    public AudioClip fireProjectile;
    public AudioClip deathExplosion;
}
