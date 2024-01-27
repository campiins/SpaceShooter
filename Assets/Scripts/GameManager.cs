using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;
    public int numberOfEnemiesInWave = 10;
    public float timeBetweenEnemies = 1f; // in seconds

    [SerializeField] private MenuManager menuManager;

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
        if (currentLevel == 2)
        {
            Time.timeScale = 0.0f;
            menuManager.ShowShop();
        }
    }

    public void AddMultipleProjectileAbility()
    {
        FindObjectOfType<PlayerController>().currentSpecialAbility = SpecialAbility.multipleProjectiles;
        menuManager.HideShop();
    }
    public void AddShieldAbility()
    {
        FindObjectOfType<PlayerController>().currentSpecialAbility = SpecialAbility.shield;
        menuManager.HideShop();
    }

    public void RemovePlayerAbility()
    {
        FindObjectOfType<PlayerController>().currentSpecialAbility = SpecialAbility.none;
    }
}
