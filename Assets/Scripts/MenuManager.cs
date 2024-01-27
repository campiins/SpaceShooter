using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject shopObj;

    private void Awake()
    {
        FindObjectOfType<PlayerController>().OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void HandlePlayerDeath()
    {
        ShowGameOver();
    }

    public void ShowGameOver()
    {
        gameOverObj.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverObj.SetActive(false);
    }

    public void ShowShop()
    {
        shopObj.SetActive(true);
    }

    public void HideShop()
    {
        Time.timeScale = 1.0f;
        shopObj.SetActive(false);
    }
}
