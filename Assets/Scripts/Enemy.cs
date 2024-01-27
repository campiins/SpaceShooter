using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Spaceship
{
    [SerializeField] private float _fireRate;
    private float _timer;

    protected override void Awake()
    {
        base.Awake();
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
        _rigidbody.velocity = transform.right * Speed;
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
}
