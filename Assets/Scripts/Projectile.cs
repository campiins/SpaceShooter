using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifetime;
    [NonSerialized] public Vector2 movementDirection;
    private ObjectPool<Projectile> _pool;
    private float _timer;

    public void Init(Vector2 direction, ObjectPool<Projectile> pool)
    {
        movementDirection = direction;
        _pool = pool;
        gameObject.SetActive(true);
        _timer = 0;
    }

    private void Update()
    {
        Movement();

        _timer += Time.deltaTime;
        if (_timer >= _lifetime)
        {
            Destroy();
        }
    }

    private void Movement()
    {
        transform.Translate(_speed * Time.deltaTime * movementDirection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("PlayerProjectile") && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(_damage);
        }
        else if (this.CompareTag("EnemyProjectile") && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage(_damage);
        }

        Destroy();
    }

    public void Destroy()
    {
        if (this.gameObject.activeSelf)
            _pool.Release(this);
    }
}
