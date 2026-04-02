using UnityEngine;
using UnityEngine.Pool;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    public int CountInactive => _pool.CountInactive;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => cube.gameObject.SetActive(true),
            actionOnRelease: (cube) => ReleaseCube(cube),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        FillPool(_poolCapacity);
    }

    public Cube Get()
    {
        if (_pool.CountAll < _poolMaxSize)
        {
            return _pool.Get();
        }
        else
        {

            return null;
        }
    }

    public void ReleaseCube(Cube cube)
    {

        if (cube == null)
        {
            Debug.Log("Cube - null!");
            return;
        }

        cube.gameObject.SetActive(false);
        _pool.Release(cube);
        Debug.Log("Release cube" );
    }

    private void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log("cube to pool");
            Cube cube = _pool.Get();
            _pool.Release(cube);
        }
    }
}
