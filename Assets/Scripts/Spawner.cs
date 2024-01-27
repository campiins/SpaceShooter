using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [Header("Spawn Boundaries")]

    [SerializeField] private Boundaries _bounds;

    [NonSerialized] public Vector2 movementDirection;
    private ObjectPool<Enemy> _pool;


    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(CreateEnemy, null, OnReturnedToPool, defaultCapacity: 20);
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
        for (int level = 1; level <= 5; level++)
        {
            for (int wave = 1; wave <= 3; wave++)
            {
                Debug.Log($"Level {level} - Wave {wave}");
                for (int enemy = 1; enemy <= 5; enemy++)
                {
                    SpawnEnemy(Vector2.left);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(5f);
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
}
