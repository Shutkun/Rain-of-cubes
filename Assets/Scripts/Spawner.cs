using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    private float _timeOfWaiting = 0.7f;
    private Coroutine _counter;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => cube.gameObject.SetActive(true),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) =>
            {
                Destroy(cube.gameObject);
            },
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        _counter = StartCoroutine(WaitOfAppeal());
    }

    private void OnDisable()
    {
        StopCoroutine(_counter);
    }

    private void Spawn()
    {
        for (int i = 0; i < _poolMaxSize; i++)
        {
            Cube cube = _pool.Get();

            cube.LifeIsEnd += ActionOnRelease;
            cube.gameObject.transform.position = GetRadomPosition();
        }
    }

    private Vector3 GetRadomPosition()
    {
        Vector3 origin = _startPoint.transform.position;
        Vector3 range = _startPoint.transform.localScale / 2f;
        Vector3 randomRange = new Vector3(
            Random.Range(-range.x, range.x),
            Random.Range(-range.y, range.y),
            Random.Range(-range.z, range.z)
        );
        Vector3 randomCoordinate = origin + randomRange;

        return randomCoordinate;
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.LifeIsEnd -= ActionOnRelease;
        _pool.Release(cube);
    }

    private IEnumerator WaitOfAppeal()
    {
        WaitForSeconds _waitForSeconds = new WaitForSeconds(_timeOfWaiting);

        while (true)
        {
            Spawn();
            yield return _waitForSeconds;
        }
    }
}
