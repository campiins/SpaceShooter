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
    private bool _isDead = false;

    [Header("Projectile Settings")]

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private GameObject _firePoints;
    [SerializeField] protected float _fireRate = 1f;

    private List<Transform> _firePointsList = new List<Transform>();
    private ObjectPool<Projectile> _projectilePool;
    protected float _timer;

    [Header("Reward Popup")]

    public int scoreReward = 100;
    public int moneyReward = 10;
    [SerializeField] protected GameObject popupTextPrefab;
    [SerializeField] protected TMP_Text popupText;

    [Header("Sound")]
    [SerializeField] private EnemySounds _sounds;

    protected Vector2 _movementDirection;
    private ObjectPool<Enemy> enemyPool;
    private DeathCause deathCause;

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
        _isDead = false;
        Health = maxHealth;
        _movementDirection = direction;
        enemyPool = pool;
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
            enemyPool.Release(this);
        }
    }

    protected virtual void Fire()
    {
        _timer += Time.deltaTime;
        if (Health > 0 && _timer > _fireRate)
        {
            AudioManager.Instance.PlayAudioClip(_sounds.fireProjectile);
            SpawnProjectile(transform.right.normalized);
            _timer = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            deathCause = DeathCause.playerProjectile;
            CallDestroyAnimation();
            ShowMoneyPopup();
        }
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

    private void ShowMoneyPopup()
    {
        popupText.text = moneyReward.ToString() + " $";
        popupTextPrefab.SetActive(false);
        Instantiate(popupTextPrefab, transform.position, Quaternion.identity).SetActive(true);
    }

    private void CallDestroyAnimation()
    {
        _isDead = true;
        if (_isDead) // Esta comprobación se hace para evitar que el sonido y la animación se reproduzcan más de una vez si varios proyectiles impactan a la vez
        {
            AudioManager.Instance.PlayAudioClip(_sounds.deathExplosion);
            _animator.SetBool("isDead", true);
        } 
    }

    public void Destroy()
    {
        if (this.gameObject.activeSelf)
        {
            _animator.SetBool("isMoving", false);

            enemyPool.Release(this);
        }
    }

    public DeathCause GetDeathCause()
    {
        return deathCause;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.CanBeDamaged && !this._isDead)
            {
                player.Destroy();
            }
            deathCause = DeathCause.player;
            CallDestroyAnimation();
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            deathCause = DeathCause.player;
            CallDestroyAnimation();
        }
    }
}

public enum DeathCause
{
    playerProjectile,
    player,
    other
}
