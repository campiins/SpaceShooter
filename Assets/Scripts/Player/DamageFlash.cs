using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]

    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private void Awake()
    {
        _spriteRenderers = GetComponents<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];
        // Asignar los materiales de spriteRenderers a materials
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallDamageFlash()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(DamageFlasher());
        }
    }

    private IEnumerator DamageFlasher()
    {
        // Asignar color
        SetFlashColor();
        float elapsedTime = 0f;

        while (elapsedTime < _flashTime)
        {
            // Iterar elapsedTime
            elapsedTime += Time.deltaTime;
            // Interpolar la cantidad de flash
            float currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        // Asignar cantidad de flash
        for(int i = 0;i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
