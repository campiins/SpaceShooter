using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Pool;

public class PlayerController : Spaceship
{
    [SerializeField] private float _fireRate;
    private float _timer;

    [Header("Movement Boundaries")]

    [SerializeField] private Boundaries _bounds;

    protected override void Awake()
    {
        base.Awake();
        _timer = _fireRate;
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
        // Inputs
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Mover nave
        _rigidbody.velocity = input * Speed;

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

    protected override void Fire()
    {
        _timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && _timer > _fireRate)
        {
            SpawnProjectile(transform.right);
            _timer = 0;
        }
    }
}

[System.Serializable]
public class Boundaries
{
    public Vector2 min; // minX = -8.4f; minY = -4.5f;
    public Vector2 max; // maxX =  8.4f; maxY =  4.5f;
}
