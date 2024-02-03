using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public void Destroy()
    {
        DeathCause cause = _enemy.GetDeathCause();

        if (cause == DeathCause.playerProjectile)
        {
            HandleEnemyDeathByProjectile();
        }
        else
        {
            HandleEnemyDeath();
        }
    }

    private void HandleEnemyDeathByProjectile()
    {
        GameManager.Instance.enemyKills++;
        GameManager.Instance.AddScore(_enemy.scoreReward);
        GameManager.Instance.AddMoney(_enemy.moneyReward);
        _enemy.Destroy();
    }

    private void HandleEnemyDeath()
    {
        GameManager.Instance.enemyKills++;
        // No añadir puntuación ni dinero si no ha muerto por un proyectil
        // No mostrar popup de dinero obtenido
        _enemy.Destroy();
    }
}
