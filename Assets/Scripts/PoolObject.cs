using UnityEngine;
using UnityEngine.Pool;

public class PoolObject : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    public int CountInactive => _pool.CountInactive;

    public int PoolMaxSize => _poolMaxSize;

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
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        FillPool(_poolCapacity);
    }

    public Cube Get()
    {
        return _pool.Get();
    }

    public void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Cube cube = _pool.Get();
            _pool.Release(cube);
        }
    }
}
