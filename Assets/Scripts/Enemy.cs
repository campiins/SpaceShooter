using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private string _name;
    public int maxHealth;
    private int _health;
    [SerializeField] protected float _speed;

    [Header("Projectile Settings")]

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private GameObject _firePoints;
    [SerializeField] protected float _fireRate = 1f;

    private List<Transform> _firePointsList = new List<Transform>();
    private ObjectPool<Projectile> _projectilePool;
    protected float _timer;

    [Header("Reward Popup")]

    [SerializeField] protected int scoreReward = 100;
    [SerializeField] protected int moneyReward = 10;
    [SerializeField] protected GameObject popupTextPrefab;
    [SerializeField] protected TMP_Text popupText;

    protected Vector2 _movementDirection;
    private ObjectPool<Enemy> _enemyPool;

    private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;

    public int Health
    {
        get { return _health; }
        set { _health = value < 0 ? 0 : value; }
    }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        Health = maxHealth;

        _projectilePool = new ObjectPool<Projectile>(CreateProjectile, null, OnReturnedToPool, defaultCapacity: 20);
        foreach (Transform childTransform in _firePoints.GetComponentsInChildren<Transform>())
        {
            if (childTransform != _firePoints.transform)
                _firePointsList.Add(childTransform);
        }
    }
    public void Init(Vector2 direction, ObjectPool<Enemy> pool)
    {
        Health = maxHealth;
        _movementDirection = direction;
        _enemyPool = pool;
        gameObject.SetActive(true);
        _timer = _fireRate - 0.33f;
        // Aplicar animación de movimiento
        _animator.SetBool("isMoving", true);
    }

    private void Update()
    {
        if (transform.position.x > -8.5f) Fire();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        // Mover nave
        _rigidbody.velocity = _movementDirection * _speed;

        if (transform.position.x < -9f) // Si sale del límite izquierdo
        {
            _enemyPool.Release(this);
        }
    }

    protected virtual void Fire()
    {
        _timer += Time.deltaTime;
        if (Health > 0 && _timer > _fireRate)
        {
            SpawnProjectile(transform.right.normalized);
            _timer = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            Destroy();
        }
    }

    private void ShowPopup()
    {
        popupText.text = moneyReward.ToString() + " $";
        popupTextPrefab.SetActive(false);
        Instantiate(popupTextPrefab, transform.position, Quaternion.identity).SetActive(true);
    }

    protected void SpawnProjectile(Vector3 movementDirection)
    {
        foreach (Transform firePointTransform in _firePointsList)
        {
            Vector3 spawnPosition = firePointTransform.position;
            Projectile projectile = _projectilePool.Get();
            projectile.transform.position = spawnPosition;
            projectile.Init(movementDirection, _projectilePool);
        }
    }

    public Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(_projectilePrefab);
        return projectile;
    }

    private void OnReturnedToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    public void Destroy()
    {
        if (this.gameObject.activeSelf)
        {
            _animator.SetBool("isMoving", false);

            ShowPopup();

            GameManager.Instance.enemyKills++;
            GameManager.Instance.AddScore(scoreReward);
            GameManager.Instance.AddMoney(moneyReward);

            _enemyPool.Release(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.CanBeDamaged)
            {
                player.Destroy();
            }
            _enemyPool.Release(this);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            _enemyPool.Release(this);
        }
    }
}
