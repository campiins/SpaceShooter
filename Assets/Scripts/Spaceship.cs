using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spaceship : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private float _speed;

    [Header("Projectile Settings")]

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private GameObject _firePoints;

    private List<Transform> _firePointsList = new List<Transform>();
    private ObjectPool<Projectile> _pool;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Health
    {
        get { return _health; }
        set { _health = value < 0 ? 0 : value; }
    }

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    protected Rigidbody2D _rigidbody;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _pool = new ObjectPool<Projectile>(CreateProjectile, null, OnReturnedToPool, defaultCapacity: 20);
        foreach (Transform childTransform in _firePoints.GetComponentsInChildren<Transform>())
        {
            if (childTransform != _firePoints.transform)
                _firePointsList.Add(childTransform);
        }
    }

    protected virtual void Movement() { }

    protected virtual void Fire() { }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            Destroy();
        }
    }

    protected void SpawnProjectile(Vector3 movementDirection)
    {
        foreach (Transform firePointTransform in _firePointsList)
        {
            Vector3 spawnPosition = firePointTransform.position;
            Projectile projectile = _pool.Get();
            projectile.transform.position = spawnPosition;
            projectile.Init(movementDirection, _pool);
        }
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(_projectilePrefab);
        return projectile;
    }

    private void OnReturnedToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}
