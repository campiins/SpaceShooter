using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private int _numberOfEnemiesInWave = 10;
    [SerializeField] private int _numberOfWavesInLevel = 3;
    [SerializeField] private int _numberOfLevels = 5;

    [SerializeField] private float _timeBetweenEnemies = 1f; // in seconds
    [SerializeField] private float _timeBetweenWaves = 3f; // in seconds
    [SerializeField] private float _timeBetweenLevels = 5f; // in seconds

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
        for (int level = 1; level <= _numberOfLevels; level++)
        {
            for (int wave = 1; wave <= _numberOfWavesInLevel; wave++)
            {
                wavesText.text = $"Level {level} - Wave {wave}";
                yield return new WaitForSeconds(2f);
                wavesText.text = "";

                for (int enemy = 1; enemy <= _numberOfEnemiesInWave; enemy++)
                {
                    SpawnEnemy(Vector2.left);
                    yield return new WaitForSeconds(_timeBetweenEnemies);
                }
                yield return new WaitForSeconds(_timeBetweenWaves);
            }
            yield return new WaitForSeconds(_timeBetweenLevels);
        }
    }

    private void SpawnEnemy(Vector3 movementDirection)
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(_bounds.min.y, _bounds.max.y), transform.position.z);
        Enemy enemy = _pool.Get();
        enemy.transform.position = spawnPosition;
        enemy.Init(movementDirection, _pool);
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab);
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
