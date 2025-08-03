using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;

    private ObjectPool<Cube> _pool;
    private int _poolCapacity = 10;
    private int _poolMaxSize = 10;
    private float _offsetPosition = 10;

    private void Awake()
    {
        const string PrefabName = "CubePrefab";
        _prefab = Resources.Load<Cube>(PrefabName);

        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => ActionORelease(cube),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        float time = 0;
        float repeatRate = 0.5f;
        InvokeRepeating(nameof(GetCube), time, repeatRate);
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 offset = new Vector3(Random.Range(-_offsetPosition, _offsetPosition),
                                     Random.Range(-_offsetPosition, _offsetPosition),
                                     Random.Range(-_offsetPosition, _offsetPosition));
        offset += transform.position;

        cube.ResetState(offset, transform.rotation);
        cube.Collision += ReleaseCube;
        cube.gameObject.SetActive(true);
    }

    private void ActionORelease(Cube cube)
    {
        cube.Collision -= ReleaseCube;
        cube.gameObject.SetActive(false);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }
}