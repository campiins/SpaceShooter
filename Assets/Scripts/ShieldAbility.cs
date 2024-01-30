using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "SpecialAbility/Shield")]

public class ShieldAbility : SpecialAbility
{
    [Tooltip("Number of hits absorbed before being damageable again when the shield level is 1.")]
    public int numberOfHitsAbsorbedLevel1;
    [Tooltip("Number of hits absorbed before being damageable again when the shield level is 2.")]
    public int numberOfHitsAbsorbedLevel2;
    [Tooltip("Key to activate shield.")]
    public KeyCode key;

    public override void Activate(int level)
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        player.ActivateShield();

        Debug.Log("Shield level " + level + " activated");
    }

    public int GetHitsAbsorbed(int level)
    {
        if (level == 1)
        {
            return numberOfHitsAbsorbedLevel1;
        }
        else if (level == 2)
        {
            return numberOfHitsAbsorbedLevel2;
        }
        else
        {
            return -1;
        }
    }
}
