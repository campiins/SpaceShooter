using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : Spaceship
{
    [SerializeField] private float _fireRate;
    private float _timer;

    [Header("Reward Popup")]

    [SerializeField] private int coinReward = 10;
    [SerializeField] private GameObject popupTextPrefab;
    [SerializeField] private TMP_Text popupText;

    [NonSerialized] public Vector2 movementDirection;
    private ObjectPool<Enemy> _enemyPool;

    protected override void Awake()
    {
        base.Awake();
    }
    public void Init(Vector2 direction, ObjectPool<Enemy> pool)
    {
        //IsDestroyed = false;
        Health = maxHealth;
        movementDirection = direction;
        _enemyPool = pool;
        gameObject.SetActive(true);
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
        // Mover nave
        _rigidbody.velocity = movementDirection * Speed;
        if (transform.position.x < -10) // Si sale del límite izquierdo
        {
            _enemyPool.Release(this);
        }
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
    public override void TakeDamage(int damage)
    {
        if (CanBeDamaged)
        {
            Health -= damage;
            if (Health == 0)
            {
                Destroy();
            }
        }
    }

    private void ShowPopup()
    {
        popupText.text = coinReward.ToString() + " $";
        Instantiate(popupTextPrefab, transform.position, Quaternion.identity);
        GameManager.Instance.AddCoins(coinReward);
    }

    public override void Destroy()
    {
        if (this.gameObject.activeSelf)
        {
            ShowPopup();
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
