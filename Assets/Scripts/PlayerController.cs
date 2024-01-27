using System;
using UnityEngine;
using UnityEngine.Events;

public enum SpecialAbility
{
    none,
    multipleProjectiles,
    shield
}

public class PlayerController : Spaceship
{
    [SerializeField] private float _fireRate;
    public SpecialAbility currentSpecialAbility = SpecialAbility.none;
    private int _projectilesAmount = 10;
    private float _startAngle = 180f, _endAngle = 0f;
    private float _timer;

    [Header("Movement Boundaries")]

    [SerializeField] private Boundaries _bounds;

    private HealthBar healthBar;
    public UnityEvent OnPlayerDeath;

    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<HealthBar>();
        _timer = _fireRate;
    }

    private void Update()
    {
        Fire();

        if (Input.GetKeyDown(KeyCode.F) && currentSpecialAbility == SpecialAbility.multipleProjectiles)
        {
            FireMultipleProjectiles();
        }
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

    private void FireMultipleProjectiles()
    {
        float angleStep = (_endAngle - _startAngle) / _projectilesAmount;
        float angle = _startAngle;

        for (int i = 0; i <= _projectilesAmount; i++)
        {
            float projDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float projDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 projMoveVector = new Vector3(projDirX, projDirY, 0f);
            Vector2 projDirection = (projMoveVector - transform.position).normalized;

            Projectile projectile = CreateProjectile();
            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            projectile.Init(projDirection, _projectilePool);

            angle += angleStep;
        }

        currentSpecialAbility = SpecialAbility.none;
    }

    public override void TakeDamage(int damage)
    {
        if (CanBeDamaged)
        {
            Health -= damage;
            if (healthBar != null) healthBar.UpdateHealthBar();
            if (Health == 0)
            {
                Destroy();
            }
        }
    }

    public override void Destroy()
    {
        OnPlayerDeath.Invoke();
        base.Destroy();
    }
}

[System.Serializable]
public class Boundaries
{
    public Vector2 min; // minX = -8.4f; minY = -4.5f;
    public Vector2 max; // maxX =  8.4f; maxY =  4.5f;
}
