using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float _destructTime = 3f;
    private float _timer;

    private void Start()
    {
        _timer = _destructTime;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
