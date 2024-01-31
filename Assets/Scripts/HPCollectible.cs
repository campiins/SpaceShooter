using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HPCollectible : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 25;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _amplitude = 4f;
    [SerializeField] private float _frequency = 1f;

    private float sineCenterY;
    private bool _inverted = false;

    private PlayerController _player;
    private ObjectPool<HPCollectible> _pool;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public void Init(ObjectPool<HPCollectible> pool)
    {
        _pool = pool;

        sineCenterY = transform.position.y;
        _inverted = (Random.value > 0.5f);

        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 position = transform.position;

        // Movimiento hacia la izquierda
        position.x -= _speed * Time.fixedDeltaTime;

        // Movimiento sinuoso
        float sine = Mathf.Sin(position.x * _frequency) * _amplitude;
        if (_inverted) sine *= -1;
        position.y = sineCenterY + sine;

        transform.position = position;

        if (position.x < -10f)
        {
            _pool.Release(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            _player.RestoreHealth(_healingAmount);
            _pool.Release(this);
        }
    }
}
