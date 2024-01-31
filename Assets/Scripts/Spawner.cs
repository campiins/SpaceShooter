using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private List<Enemy> _enemyPrefabs = new List<Enemy>();

    [NonSerialized] public Vector2 movementDirection;

    [Header("Spawn Boundaries")]

    [SerializeField] private Boundaries _bounds;

    [Header("UI")]

    [SerializeField] private TMP_Text wavesText;

    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(CreateEnemy, null, OnReturnedToPool, defaultCapacity: 20);
        FindObjectOfType<PlayerController>().OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {

    }

    private IEnumerator SpawnEnemies()
    {
        for (int level = 1; level <= GameManager.Instance.numberOfLevels; level++)
        {
            if (level > 1)
            {
                FindObjectOfType<MenuManager>().ShowShop();
            }

            for (int wave = 1; wave <= GameManager.Instance.numberOfWavesInLevel; wave++)
            {
                GameManager.Instance.currentLevel = level;
                wavesText.text = $"Level {GameManager.Instance.currentLevel} - Wave {wave}";
                yield return new WaitForSeconds(3f);
                wavesText.text = "";

                for (int enemy = 1; enemy <= GameManager.Instance.numberOfEnemiesInWave; enemy++)
                {
                    SpawnEnemy(Vector2.left);
                    yield return new WaitForSeconds(GameManager.Instance.timeBetweenEnemies);
                }
                yield return new WaitForSeconds(GameManager.Instance.timeBetweenWaves);
            }
            yield return new WaitForSeconds(GameManager.Instance.timeBetweenLevels);
        }
    }

    private void SpawnEnemy(Vector3 movementDirection)
    {
        Vector3 spawnPosition = new (transform.position.x, Random.Range(_bounds.min.y, _bounds.max.y), transform.position.z);
        Enemy enemy = _pool.Get();
        enemy.transform.position = spawnPosition;
        enemy.Init(movementDirection, _pool);
    }

    private Enemy CreateEnemy()
    {
        int i = Random.Range(0, _enemyPrefabs.Count);
        Enemy enemy = Instantiate(_enemyPrefabs[i]);
        return enemy;
    }

    private void OnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void HandlePlayerDeath()
    {
        StopAllCoroutines();
    }
}
