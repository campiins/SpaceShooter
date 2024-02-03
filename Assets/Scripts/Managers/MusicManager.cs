using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Music _music;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _audioSource.clip = _music.mainMenuMusic;
        }
        else
        {
            _audioSource.clip = _music.inGameMusic;
        }
        _audioSource.Play();
    }
}

[System.Serializable]
public class Music
{
    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;
    public AudioClip gameOverMusic;
    public AudioClip winMusic;
}
