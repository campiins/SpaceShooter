using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public List<SpecialAbility> abilities = new List<SpecialAbility>();
    
    [SerializeField] private AbilitiesUI _abilitiesUI;


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

                // Actualizar la interfaz de usuario (UI)
                _abilitiesUI?.UpdateUI(ability);
            }
        }
    }
}
