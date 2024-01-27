using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _width;
    private Vector3 _intialPosition;

    private void Start()
    {
        _intialPosition = transform.position;
        _width = GetComponent<SpriteRenderer>().size.y; // Cogemos el Y porque el sprite original es vertical
    }

    private void Update()
    {
        // Resto: cuanto me queda de recorrido par alcanzar un nuevo ciclo.
        float remainder = (_speed * Time.time) % _width;

        transform.position = _intialPosition + remainder * Vector3.left;
    }
}
