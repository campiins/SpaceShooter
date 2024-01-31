using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private ShieldAbility _shieldAbility;

    [SerializeField] private Color[] _hitColors = new Color[3];

    private int _hits = 0;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        _hits = 0;
        _spriteRenderer.color = _hitColors[_hits];
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_hits >= _shieldAbility.GetHitsAbsorbed(_shieldAbility.currentLevel))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectile"))
        {
            _hits++;
            _spriteRenderer.color = _hitColors[_hits];
        }
    }
}
