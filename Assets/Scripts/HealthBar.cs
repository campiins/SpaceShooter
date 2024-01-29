using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _background;
    [SerializeField] private Image _fill;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Color backgroundColor = _background.color;
            Color fillColor = _fill.color;

            backgroundColor.a = 0.5f;
            fillColor.a = 0.5f;

            _background.color = backgroundColor;
            _fill.color = fillColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Color backgroundColor = _background.color;
            Color fillColor = _fill.color;

            backgroundColor.a = 1f;
            fillColor.a = 1f;

            _background.color = backgroundColor;
            _fill.color = fillColor;
        }
    }
}
