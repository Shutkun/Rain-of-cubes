using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private PoolObject _pool;
    [SerializeField] private Cube _cube;

    private void OnEnable()
    {
        _cube.LifeIsEnd += ActionOnRelease;
    }

    private void OnDisable()
    {
        _cube.LifeIsEnd -= ActionOnRelease;
    }

    private void Update()
    {
        ActionOnGet();
    }

    private void ActionOnGet()
    {
        Vector3 origin = _startPoint.transform.position;
        Vector3 range = _startPoint.transform.localScale / 2f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                          Random.Range(-range.y, range.y),
                                          Random.Range(-range.z, range.z));
        Vector3 randomCoordinate = origin + randomRange;

        if (_pool.CountInactive > 0)
        {
            _pool.Get().gameObject.transform.position = randomCoordinate;
        }
    }

    private void ActionOnRelease(Cube cube)
    {
        Debug.Log("Cube return to pool");
        _pool.ReleaseCube(cube);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
