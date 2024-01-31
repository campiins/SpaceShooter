using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetEnemy : Enemy
{
    private PlayerController _player;

    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<PlayerController>();
    }

    protected override void Fire()
    {
        _timer += Time.deltaTime;
        if (Health > 0 && _timer > _fireRate)
        {
            if (_player != null)
            {
                Vector2 playerDirection = (_player.transform.position - transform.position).normalized;
                SpawnProjectile(playerDirection);
            }
            _timer = 0;
        }
    }
}
