using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private ShieldAbility _shieldAbility;

    private int hits = 0;

    public void Init()
    {
        hits = 0;
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (hits >= _shieldAbility.GetHitsAbsorbed(_shieldAbility.currentLevel))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectile"))
        {
            hits++;
        }
    }
}
