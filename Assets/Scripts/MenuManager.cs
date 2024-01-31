using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("InGame UI")]

    [SerializeField] private TMP_Text currentScoreTxt;
    [SerializeField] private TMP_Text currentCoinsTxt;

    [Header("Shop")]

    [SerializeField] private GameObject shopObj;

    [Header("Game Over")]

    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private TMP_Text finalScoreGOText;
    [SerializeField] private TMP_Text levelGOText;
    [SerializeField] private TMP_Text totalKillsGOText;

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
        Invoke("ShowGameOver", 2);
    }

    public void ShowGameOver()
    {
        finalScoreGOText.text = GameManager.Instance.currentScore.ToString();
        levelGOText.text = GameManager.Instance.currentLevel.ToString();
        totalKillsGOText.text = GameManager.Instance.enemyKills.ToString();
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
