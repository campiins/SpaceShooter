using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent OnPlayerDeath;

    public void Destroy()
    {
        OnPlayerDeath.Invoke();

        GameObject playerParent = this.transform.parent.gameObject;
        if (playerParent.activeSelf)
        {
            Destroy(playerParent);
        }
    }
}
