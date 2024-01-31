using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Multi-Shot", menuName = "SpecialAbility/Multi-Shot")]
public class MultishotAbility : SpecialAbility
{
    [Tooltip("Number of projectiles to be fired when the multi-shot level is 1.")]
    public int numberOfProjectilesLevel1;
    [Tooltip("Number of projectiles to be fired when the multi-shot level is 2.")]
    public int numberOfProjectilesLevel2;
    [Tooltip("Key to activate multi-shot.")]
    public KeyCode key;

    private float _startAngle = 0f, _endAngle = 360f;

    public override void Activate(int level)
    {
        MultiShot(FindObjectOfType<PlayerController>());
    }

    public void MultiShot(PlayerController player)
    {
        int projectilesAmount = currentLevel * 10;

        float angleStep = (_endAngle - _startAngle) / projectilesAmount;
        float angle = _startAngle;

        for (int i = 0; i <= projectilesAmount; i++)
        {
            float projDirX = player.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float projDirY = player.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 projMoveVector = new Vector3(projDirX, projDirY, 0f);
            Vector2 projDirection = (projMoveVector - player.transform.position).normalized;

            Projectile projectile = player.CreateProjectile();
            projectile.transform.position = player.transform.position;
            projectile.transform.rotation = player.transform.rotation;
            projectile.Init(projDirection, player._projectilePool);

            angle += angleStep;
        }
    }
}
