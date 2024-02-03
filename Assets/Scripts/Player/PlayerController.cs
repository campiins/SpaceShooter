using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [Header("General Settings")]

    public int maxHealth;
    [SerializeField] private float _speed;
    [NonSerialized] public bool canMove = true;
    
    private int _health;
    [NonSerialized] public bool _canBeDamaged = true;

    [Header("Projectile Settings")]

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private GameObject _firePoints;
    [SerializeField] private float _fireRate;

    private List<Transform> _firePointsList = new List<Transform>();
    public ObjectPool<Projectile> _projectilePool;
    private float _timer;

    [Header("Special Abilities")]

    [SerializeField] private Shield _shield;

    [Header("Movement Boundaries")]

    [SerializeField] private Boundaries _bounds;

    [Header("Sound")]

    public PlayerSounds sounds;

    private HealthBar _healthBar;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    public int Health
    {
        get { return _health; }
        set { _health = value < 0 ? 0 : value; }
    }

    public bool CanBeDamaged
    {
        get { return _canBeDamaged; }
        set { _canBeDamaged = value; }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _healthBar = FindObjectOfType<HealthBar>();

        Health = maxHealth;
        _timer = _fireRate;

        _projectilePool = new ObjectPool<Projectile>(CreateProjectile, null, OnReturnedToPool, defaultCapacity: 20);
        foreach (Transform childTransform in _firePoints.GetComponentsInChildren<Transform>())
        {
            if (childTransform != _firePoints.transform)
                _firePointsList.Add(childTransform);
        }
    }

    private void Update()
    {
        if (canMove)
            Fire();
    }

    private void FixedUpdate()
    {
        if (canMove)
            Movement();
    }

    private void Movement()
    {
        // Inputs
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Mover nave
        _rigidbody.velocity = input * _speed;

        // Delimitar movimiento
        ConstrainPlayerMovement();

        // Aplicar animación de movimiento
        _animator.SetBool("isMoving", input != Vector2.zero);
    }

    private void ConstrainPlayerMovement()
    {
        float xClamped = Mathf.Clamp(_rigidbody.position.x, _bounds.min.x, _bounds.max.x);
        float yClamped = Mathf.Clamp(_rigidbody.position.y, _bounds.min.y, _bounds.max.y);
        _rigidbody.position = new Vector2(xClamped, yClamped);
    }

    private void Fire()
    {
        _timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && _timer > _fireRate)
        {
            AudioManager.Instance.PlayAudioClip(sounds.fireProjectile);
            SpawnProjectile(transform.right);
            _timer = 0;
        }
    }

    public void ActivateShield()
    {
        if (!_shield.gameObject.activeSelf) 
        {
            AudioManager.Instance.PlayAudioClip(sounds.activateShield);
            _shield.Init();
        }
    }

    public void DeactivateShield()
    {
        AudioManager.Instance.PlayAudioClip(sounds.deactivateShield);
        _shield.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (CanBeDamaged)
        {
            Health -= damage;
            if (_healthBar != null) _healthBar.UpdateHealthBar();
            if (Health == 0)
            {
                Destroy();
            }
        }
    }

    public void RestoreHealth(int health)
    {
        if (Health < maxHealth)
        {
            Health += health;
            Mathf.Clamp(Health, 0, maxHealth);
            _healthBar.UpdateHealthBar();
        }
    }

    private void SpawnProjectile(Vector3 movementDirection)
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
        AudioManager.Instance.PlayAudioClip(sounds.deathExplosion);
        _animator.SetBool("isDead", true);
    }
}
