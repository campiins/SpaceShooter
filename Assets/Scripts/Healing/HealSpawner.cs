using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class HealSpawner : MonoBehaviour
{
    [SerializeField] private HPCollectible _hPCollectible;
    [SerializeField] private float spawnInterval = 30.0f;

    private ObjectPool<HPCollectible> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<HPCollectible>(CreateCollectible, null, OnReturnedToPool, defaultCapacity: 20);
    }

    private void Start()
    {
        StartCoroutine(SpawnCollectibles());
    }

    private IEnumerator SpawnCollectibles()
    {
        while (GameManager.Instance.currentLevel <= 0)
        {
            yield return null;
        }

        while (!GameManager.Instance.isGameOver)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCollectible();
        }
    }

    private void SpawnCollectible()
    {
        Vector3 spawnPosition = new(transform.position.x, Random.Range(-2, 2), transform.position.z);
        HPCollectible collectible = _pool.Get();
        collectible.transform.position = spawnPosition;
        collectible.Init(_pool);
    }

    private HPCollectible CreateCollectible()
    {
        HPCollectible collectible = Instantiate(_hPCollectible);
        return collectible;
    }

    private void OnReturnedToPool(HPCollectible collectible)
    {
        collectible.gameObject.SetActive(false);
    }
}
