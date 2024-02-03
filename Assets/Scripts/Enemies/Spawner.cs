using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyType _enemyTargetPlayerType;

    [HideInInspector] public Vector2 movementDirection;

    [Header("Spawn Boundaries")]

    [SerializeField] private Boundaries _bounds;

    [Header("UI")]

    [SerializeField] private TMP_Text wavesText;

    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Enemy> _enemyTargetPlayerPool;

    public UnityEvent OnFinishLevels;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, null, OnReturnedToPool, defaultCapacity: 20);
        _enemyTargetPlayerPool = new ObjectPool<Enemy>(CreateEnemyTargetPlayer, null, TargetOnReturnedToPool, defaultCapacity: 20);

        FindObjectOfType<PlayerDeath>().OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int level = 1; level <= GameManager.Instance.numberOfLevels; level++)
        {
            if (level > 1)
            {
                UpdateSpawnProbabilities();
                Debug.Log($"Enemy1: {_enemyType.spawnProbability} Enemy2: {_enemyTargetPlayerType.spawnProbability}");

                // Aumentar el número de enemigos en 2
                GameManager.Instance.numberOfEnemiesInWave += 2;

                FindObjectOfType<MenuManager>().ShowShop();
            }

            GameManager.Instance.currentLevel = level;
            Debug.Log($"Level: {GameManager.Instance.currentLevel}");
            for (int wave = 1; wave <= GameManager.Instance.numberOfWavesInLevel; wave++)
            {
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
        OnFinishLevels.Invoke();
    }

    private void SpawnEnemy(Vector3 movementDirection)
    {
        Vector3 spawnPosition = new(transform.position.x, Random.Range(_bounds.min.y, _bounds.max.y), transform.position.z);

        bool canSpawnEnemy = _enemyType.minSpawnLevel <= GameManager.Instance.currentLevel;
        bool canSpawnTargetEnemy = _enemyTargetPlayerType.minSpawnLevel <= GameManager.Instance.currentLevel;

        if (canSpawnEnemy && canSpawnTargetEnemy) 
        {
            float totalProbability = GetTotalProbability();
            float rndValue = Random.Range(0f, totalProbability);

            if (rndValue <= _enemyType.spawnProbability)
            {
                SpawnFromPool(_enemyPool, spawnPosition, movementDirection);
            }
            else
            {
                SpawnFromPool(_enemyTargetPlayerPool, spawnPosition, movementDirection);
            }
        }
        else if (canSpawnEnemy)
        {
            SpawnFromPool(_enemyPool, spawnPosition, movementDirection);
        }
        else
        {
            SpawnFromPool(_enemyTargetPlayerPool, spawnPosition, movementDirection);
        }
    }

    private void SpawnFromPool(ObjectPool<Enemy> pool, Vector3 spawnPosition, Vector3 movementDirection)
    {
        Enemy enemy = pool.Get();

        enemy.transform.position = spawnPosition;
        enemy.Init(movementDirection, pool);
    }

    private Enemy CreateEnemy()
    {
        //int i = Random.Range(0, _enemyPrefabs.Count);
        //Enemy enemy = Instantiate(_enemyPrefabs[i]);
        //return enemy;
        Enemy enemy = Instantiate(_enemyType.enemyPrefab);
        return enemy;
    }

    private Enemy CreateEnemyTargetPlayer()
    {
        Enemy enemy = Instantiate(_enemyTargetPlayerType.enemyPrefab);
        return enemy;
    }

    private void OnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void TargetOnReturnedToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void HandlePlayerDeath()
    {
        StopAllCoroutines();
    }

    private float GetTotalProbability()
    {
        return _enemyType.spawnProbability + _enemyTargetPlayerType.spawnProbability;
    }

    private void UpdateSpawnProbabilities()
    {
        _enemyType.spawnProbability -= 0.15f; // Disminuir la probabilidad de _enemyType
        _enemyTargetPlayerType.spawnProbability += 0.15f; // Aumentar la probabilidad de _enemyTargetPlayerType

        // Asegurarse de que las probabilidades estén en el rango [0, 1]
        _enemyType.spawnProbability = Mathf.Clamp01(_enemyType.spawnProbability);
        _enemyTargetPlayerType.spawnProbability = Mathf.Clamp01(_enemyTargetPlayerType.spawnProbability);
    }
}

[System.Serializable]
public class EnemyType
{
    public Enemy enemyPrefab;
    public float spawnProbability;
    public int minSpawnLevel;
}
