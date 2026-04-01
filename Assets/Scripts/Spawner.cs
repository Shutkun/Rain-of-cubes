using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private float _repeatRate = 0.02f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 4;

    private ObjectPool<GameObject> _pool;
    private Coroutine _releaseCoroutine;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void ActionOnGet(GameObject obj)
    {
        Vector3 origin = _startPoint.transform.position;
        Vector3 range = _startPoint.transform.localScale / 2f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                          Random.Range(-range.y, range.y),
                                          Random.Range(-range.z, range.z));
        Vector3 randomCoordinate = origin + randomRange;

        obj.transform.position = randomCoordinate;
        obj.SetActive(true);
    }

    private void GetCube()
    {
        if (_pool.CountAll <= _poolMaxSize)
        {
            _pool.Get();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _releaseCoroutine = StartCoroutine(ReleaseAfterDelay(other));
    }

    private IEnumerator ReleaseAfterDelay(Collider other)
    {
        yield return new WaitForSeconds(2f);

        if (other.gameObject.TryGetComponent<Cube>(out Cube cube))
        {
            cube.CallBack(other.gameObject);
            _pool.Release(other.gameObject);
        }
    }

    private void Destroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
