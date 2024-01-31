using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _gameMusic;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        _audioSource.clip = _gameMusic;
        _audioSource.Play();
    }
}
