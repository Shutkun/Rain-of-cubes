using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Color _currentColor;
    private Coroutine _lifeCoroutine;
    private Renderer _renderer;

    public event Action<Cube> LifeIsEnd;

    public bool IsColorChange { get; private set; } = false;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();

        if (_renderer != null)
        {
            _currentColor = _renderer.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer not found");
        }
    }

    private void OnDisable()
    {
        if (_lifeCoroutine != null)
        {
            StopCoroutine(_lifeCoroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ColorRandom colorRandom))
        {
            _lifeCoroutine = StartCoroutine(CountingDownTimeOfLife());
            colorRandom.ChangeColor(gameObject);
        }
    }

    private void ResetParametrs()
    {
        _renderer.material.color = _currentColor;
        IsColorChange = false;
    }

    private IEnumerator CountingDownTimeOfLife()
    {
        float minLifeSecond = 2;
        float maxLifeSecond = 5;

        WaitForSeconds _waitForSeconds = new WaitForSeconds(UnityEngine.Random.Range(minLifeSecond, maxLifeSecond));

        yield return _waitForSeconds;

        ResetParametrs();
        LifeIsEnd?.Invoke(this);
    }

    public void ColorChange() =>
        IsColorChange = true;
}
