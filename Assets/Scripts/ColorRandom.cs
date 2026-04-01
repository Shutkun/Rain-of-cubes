using UnityEngine;

public class ColorRandom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ChangeColor(other.gameObject);
    }

    private void ChangeColor(GameObject gameObject)
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
    }
}
