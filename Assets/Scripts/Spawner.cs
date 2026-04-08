using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private PoolObject _pool;

    private float _timeOfWaiting = 2f;
    private float _timeOfDelay = 0.1f;
    private Coroutine _counter;
    private Coroutine _delay;

    private void Start()
    {
        _counter = StartCoroutine(CounterOfAppeal());
    }

    private void OnDisable()
    {
        StopCoroutine( _counter);
        StopCoroutine( _delay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    private IEnumerator ActionOnGet()
    {
        WaitForSeconds _waitForSeconds = new WaitForSeconds(_timeOfDelay);

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
                yield return _waitForSeconds;
            }
            else
            {
                yield break;
            }
        }
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.LifeIsEnd -= ActionOnRelease;
        _pool.ReleaseCube(cube);
    }

    private IEnumerator CounterOfAppeal()
    {
        WaitForSeconds _waitForSeconds = new WaitForSeconds(_timeOfWaiting);

        while (true)
        {
            yield return _delay = StartCoroutine(ActionOnGet());
            yield return _waitForSeconds;
        }
    }
}
