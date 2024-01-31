using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject shopObj;
    [SerializeField] private TMP_Text currentScoreTxt;
    [SerializeField] private TMP_Text currentCoinsTxt;

    private void Awake()
    {
        FindObjectOfType<PlayerController>().OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void Start()
    {
        // Inicializar texto de puntuación y monedas
        currentScoreTxt.text = "Score " + GameManager.Instance.currentScore.ToString();
        currentCoinsTxt.text = GameManager.Instance.currentCoins.ToString() + " $";
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
        Time.timeScale = 0.0f;
        shopObj.SetActive(true);
    }

    public void HideShop()
    {
        Time.timeScale = 1.0f;
        shopObj.SetActive(false);
    }

    public void UpdateScoreText()
    {
        currentScoreTxt.text = "Score " + GameManager.Instance.currentScore.ToString();
    }

    public void UpdateCoinsText()
    {
        currentCoinsTxt.text = GameManager.Instance.currentCoins.ToString() + " $";
    }
}
