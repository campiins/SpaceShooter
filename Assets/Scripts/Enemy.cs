using System;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : Spaceship
{
    [SerializeField] private float _fireRate;
    private float _timer;

    [NonSerialized] public Vector2 movementDirection;
    private ObjectPool<Enemy> _pool;

    protected override void Awake()
    {
        base.Awake();
    }
    public void Init(Vector2 direction, ObjectPool<Enemy> pool)
    {
        movementDirection = direction;
        _pool = pool;
        gameObject.SetActive(true);
    }


    private void Update()
    {
        Fire();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    protected override void Movement()
    {
        // Mover nave
        _rigidbody.velocity = movementDirection * Speed;
    }

    protected override void Fire()
    {
        _timer += Time.deltaTime;
        if (Health > 0 && _timer > _fireRate)
        {
            SpawnProjectile(transform.right);
            _timer = 0;
        }
    }

    public override void Destroy()
    {
        _pool.Release(this);
    }
}
