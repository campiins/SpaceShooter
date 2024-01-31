using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boundaries")]
public class Boundaries : ScriptableObject
{
    public Vector2 min = new Vector2(-8.4f, -4.5f);
    public Vector2 max = new Vector2(8.4f, 4.5f);
}
