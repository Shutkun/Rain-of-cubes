using UnityEngine;

public class ColorRandom : MonoBehaviour
{
    public void ChangeColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }
}
