using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Entity entityPrefab;
    IObjectPool<Entity> pool;

    private void Awake()
    {
        pool = new ObjectPool<Entity>(
            createFunc: CreateEntity,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyEntity);
    }

    public async UniTask SpawnEntityAsync(Vector3 position)
    {
        var entity = pool.Get();
        entity.transform.position = position;
        await entity.PlaySpawnAnimationAsync();
        await entity.MoveAwayAsync(5f, 1.5f);

        await UniTask.Delay(2000);
        await entity.PlayDeathAnimationAsync();
    }

    Entity CreateEntity()
    {
        var entity = Instantiate(entityPrefab, transform);
        entity.Initialize(pool);
        return entity;
    }
    
    void OnGet(Entity entity) => entity.gameObject.SetActive(true);

    void OnRelease(Entity entity)
    {
        entity.Interrupt();
        entity.gameObject.SetActive(false);
    }

    void OnDestroyEntity(Entity entity)
    {
        if (entity != null)
        {
            entity.Interrupt();
            Destroy(entity.gameObject);
        }
    }
}
