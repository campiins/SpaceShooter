using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Special Abilty")]
public class SpecialAbility : ScriptableObject
{
    public string abilityName;
    public float cooldownTime;
    public int currentLevel = 0;
    public int priceLevel1;
    public int priceLevel2;
    [NonSerialized] public float cooldownTimer;

    public virtual void Activate(int level) { }

    public virtual float GetCooldown(int level)
    {
        return cooldownTime;
    }

    public virtual int GetPrice(int level)
    {
        if (level == 1)
        {
            return priceLevel1;
        }
        else if (level == 2)
        {
            return priceLevel2;
        }
        else
        {
            Debug.LogError("Level not valid for ability " + abilityName);
            return -1;
        }
    }

    public virtual int GetMaxLevel()
    {
        return 2;
    }
}
