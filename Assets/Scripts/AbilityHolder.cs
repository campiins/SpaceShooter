using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public List<SpecialAbility> abilities = new List<SpecialAbility>();

    private void Update()
    {
        foreach (SpecialAbility ability in abilities)
        {
            // Activar habilidades
            if (ability is MultishotAbility multishot && Input.GetKeyDown(multishot.key) && ability.cooldownTimer <= 0)
            {
                multishot.Activate(multishot.currentLevel);
                multishot.cooldownTimer = multishot.cooldownTime;
            }
            if (ability is ShieldAbility shield && Input.GetKeyDown(shield.key) && ability.cooldownTimer <= 0)
            {
                shield.Activate(shield.currentLevel);
                shield.cooldownTimer = shield.cooldownTime;
            }

            // Actualizar cooldowns
            if (ability.cooldownTimer > 0)
            {
                ability.cooldownTimer -= Time.deltaTime;
            }
        }
    }

    private void TryActivateAbility<T>() where T : SpecialAbility
    {
        SpecialAbility ability = abilities.Find(sa => sa.GetType() == typeof(T));

        if (ability != null && ability.cooldownTimer <= 0)
        {
            ability.Activate(ability.currentLevel);
            ability.cooldownTimer = ability.cooldownTime;
        }
        else
        {
            Debug.LogWarning($"Ability {typeof(T)} not found or on cooldown.");
        }
    }
}
