using UnityEngine;

public class ColorRandom : MonoBehaviour
{
    public void ChangeColor(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Cube>(out Cube cube))
        {
            if (cube.IsColorChange == false)
            {
                cube.ColorChange();

                if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.material.color = Random.ColorHSV();
                }
            }
        }
        else
        {
            Debug.Log("Component Cube not found.");
        }
    }
}
