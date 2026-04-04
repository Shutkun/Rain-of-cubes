using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private PoolObject _pool;

    private Coroutine _counter;

    private void Start()
    {
        _counter = StartCoroutine(CounterOfAppeal());
    }

    private IEnumerator ActionOnGet()
    {
        for (int i = 0; i < _pool.PoolMaxSize; i++)
        {
            Vector3 origin = _startPoint.transform.position;
            Vector3 range = _startPoint.transform.localScale / 2f;
            Vector3 randomRange = new Vector3(
                Random.Range(-range.x, range.x),
                Random.Range(-range.y, range.y),
                Random.Range(-range.z, range.z)
            );
            Vector3 randomCoordinate = origin + randomRange;

            Cube cube = _pool.Get();

            if (cube != null)
            {
                cube.LifeIsEnd += ActionOnRelease;
                cube.gameObject.transform.position = randomCoordinate;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield break;
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.LifeIsEnd -= ActionOnRelease;
        _pool.ReleaseCube(cube);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    private IEnumerator CounterOfAppeal()
    {
        while (true)
        {
            yield return StartCoroutine(ActionOnGet());
            yield return new WaitForSeconds(2f);
        }
    }
}
