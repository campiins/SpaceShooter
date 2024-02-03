using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("InGame UI")]

    [SerializeField] private TMP_Text _currentScoreTxt;
    [SerializeField] private TMP_Text _currentMoneyTxt;

    [Header("Pause Menu")]

    [SerializeField] private GameObject _pauseMenuObj;

    [Header("Shop")]

    [SerializeField] private GameObject _shopObj;
    private AbilityHolder _abilityHolder;

    [Header("Game Over")]

    [SerializeField] private GameObject _gameOverObj;
    [SerializeField] private TMP_Text _finalScoreGOText;
    [SerializeField] private TMP_Text _levelGOText;
    [SerializeField] private TMP_Text _totalKillsGOText;

    [Header("Win")]
    [SerializeField] private GameObject _winPanelObj;
    [SerializeField] private TMP_Text _finalScoreWText;
    [SerializeField] private TMP_Text _levelWText;
    [SerializeField] private TMP_Text _totalKillsWText;


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

        PlayerController _player = FindObjectOfType<PlayerController>();
        PlayerDeath _playerDeath = FindObjectOfType<PlayerDeath>();
        _playerDeath?.OnPlayerDeath.AddListener(HandlePlayerDeath);
        _abilityHolder = _player.gameObject.GetComponent<AbilityHolder>();

    }

    private void Start()
    {
        // Inicializar texto de puntuación y monedas
        _currentScoreTxt.text = "Score " + GameManager.Instance.currentScore.ToString();
        _currentMoneyTxt.text = GameManager.Instance.currentMoney.ToString() + " $";
    }

    private void HandlePlayerDeath()
    {
        Invoke("ShowGameOver", 2);
    }

    public void ShowPauseMenu()
    {
        GameManager.Instance.PauseGame();
        _pauseMenuObj.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _pauseMenuObj.SetActive(false);
        GameManager.Instance.UnpauseGame();
    }

    public void ShowGameOver()
    {
        _finalScoreGOText.text = GameManager.Instance.currentScore.ToString();
        _levelGOText.text = GameManager.Instance.currentLevel.ToString();
        _totalKillsGOText.text = GameManager.Instance.enemyKills.ToString();

        GameManager.Instance.isGameOver = true;

        _gameOverObj.SetActive(true);
    }

    public void ShowWinPanel()
    {
        _finalScoreWText.text = GameManager.Instance.currentScore.ToString();
        _levelWText.text = GameManager.Instance.currentLevel.ToString();
        _totalKillsWText.text = GameManager.Instance.enemyKills.ToString();

        _winPanelObj.SetActive(true);
    }

    public void ShowShop()
    {
        bool canShowShop = false;

        // Si el jugador tiene menos de 2 habilidades, se muestra la tienda
        if (_abilityHolder.abilities.Count < 2)
        {
            canShowShop = true;
        }
        else // Si el jugador tiene al menos 2 habilidades
        {
            // Si alguna de las habilidades del jugador no está a nivel máximo, se muestra la tienda
            foreach (SpecialAbility ability in _abilityHolder.abilities)
            {
                if (ability.currentLevel < ability.GetMaxLevel())
                {
                    canShowShop = true;
                    break;
                }
            }
        }

        if (canShowShop)
        {
            GameManager.Instance.PauseGame();
            _shopObj.SetActive(true);
        }
    }

    public void UpdateScoreText()
    {
        _currentScoreTxt.text = "Score " + GameManager.Instance.currentScore.ToString();
    }

    public void UpdateMoneyText()
    {
        _currentMoneyTxt.text = GameManager.Instance.currentMoney.ToString() + " $";
    }

    private void OnDisable()
    {
        PlayerDeath _player = FindObjectOfType<PlayerDeath>();
        _player?.OnPlayerDeath.RemoveListener(HandlePlayerDeath);
    }
}
