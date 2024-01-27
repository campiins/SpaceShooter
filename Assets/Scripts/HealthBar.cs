using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private static PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _player.OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void HandlePlayerDeath()
    {
        _slider.value = 0;
    }

    private void Start()
    {
        if (_player != null)
        {
            _slider.value = _player.Health;
        }
    }

    public void UpdateHealthBar()
    {
        if (_player != null)
        {
            _slider.value = _player.Health;
        }
    }
}
