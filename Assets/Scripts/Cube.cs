using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private ColorRandom _colorRandom;
    
    private bool _isColorChange = false;
    private Color _currentColor;
    private Vector3 _currentVelocity;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Quaternion _currentRotation;
    private Coroutine _lifeCoroutine;

    public event Action<Cube> LifeIsEnd;


    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _currentRotation = transform.rotation;

        if (_rigidbody != null)
        {
            _currentVelocity = _rigidbody.velocity;
        }
        else
        {
            Debug.LogWarning("Rigidbody not found");
        }

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
        if (other.TryGetComponent(out Platform platform))
        {
            _lifeCoroutine = StartCoroutine(CountingDownTimeOfLife());
            
            if (_isColorChange == false)
            {
                _colorRandom.ChangeColor(_renderer);
                _isColorChange = true;
            }
        }
    }

    private void ResetParametrs()
    {
        _isColorChange = false;
        _renderer.material.color = _currentColor;
        _rigidbody.velocity = _currentVelocity;
        transform.rotation = _currentRotation;
    }

    private IEnumerator CountingDownTimeOfLife()
    {
        float minLifeSecond = 2;
        float maxLifeSecond = 5;

        WaitForSeconds _waitForSeconds = new WaitForSeconds(Random.Range(minLifeSecond, maxLifeSecond));

        yield return _waitForSeconds;

        ResetParametrs();
        LifeIsEnd?.Invoke(this);
    }
}
