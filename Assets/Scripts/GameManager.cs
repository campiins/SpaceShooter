using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private const int MAX_SCORE = 999999;

    [HideInInspector] public int currentLevel = 0;
    [HideInInspector] public int currentScore = 0;
    [HideInInspector] public int currentCoins = 0;
    [HideInInspector] public int enemyKills = 0;

    [Header("Enemy Spawner Settings")]

    public int numberOfEnemiesInWave = 10;
    public float timeBetweenEnemies = 1f; // in seconds
    public int numberOfWavesInLevel = 3;
    public float timeBetweenWaves = 3f; // in seconds
    public int numberOfLevels = 5;
    public float timeBetweenLevels = 5f; // in seconds

    [Header("Object References")]

    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Shop shop;


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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddCoins(1000);
        }
    }

    public void AddScore(int score)
    {
        if ((currentScore + score) < MAX_SCORE)
        {
            currentScore += score;
        }
        else
        {
            currentScore = MAX_SCORE;
        }
        menuManager.UpdateScoreText();
    }

    public void AddCoins(int coins)
    {
        currentCoins += coins;
        menuManager.UpdateCoinsText();
        shop.UpdateCoinsText();
    }
}
