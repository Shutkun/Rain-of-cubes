using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Color _currentColor;
    private Coroutine _lifeCoroutine;

    public event Action<Cube> LifeIsEnd;

    public bool IsColorChange { get; private set; } = false;

    private void OnEnable()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            _currentColor = renderer.material.color;
        }
    }

    private void OnDisable()
    {
        if (_lifeCoroutine != null)
        {
            StopCoroutine(_lifeCoroutine);
        }
    }

    public void ColorChange() =>
        IsColorChange = true;

    private void ResetTheParametrs()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = _currentColor;
            IsColorChange = false;
            this.GetComponent<Animation>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Platform platform))
        {
            _lifeCoroutine = StartCoroutine(CounterOfLife());
            this.GetComponent<Animation>().enabled = false;
        }
    }

    private IEnumerator CounterOfLife()
    {
        float minLifeSecond = 2;
        float maxLifeSecond = 5;

        WaitForSeconds _waitForSeconds = new WaitForSeconds(UnityEngine.Random.Range(minLifeSecond, maxLifeSecond));

        yield return _waitForSeconds;

        ResetTheParametrs();
        LifeIsEnd?.Invoke(this);
    }
}
