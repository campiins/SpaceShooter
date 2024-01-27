using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            other.GetComponent<Projectile>().Destroy();
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Destroy();
        }
    }
}
