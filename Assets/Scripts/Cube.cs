using UnityEngine;

public class Cube : MonoBehaviour, IRelease
{
    private Material _currentMaterial;
    private Color _currentColor;

    public bool IsColorChange { get; private set; } = false;

    public void Start()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            _currentMaterial = renderer.material;
            _currentColor = renderer.material.color;
        }
    }

    public void ColorChange() =>
        IsColorChange = true;

    public void CallBack(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material = _currentMaterial;
            renderer.material.color = _currentColor;
            IsColorChange = false;
        }
    }
}
