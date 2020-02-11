using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<int, Pool> Pools = new Dictionary<int, Pool>();

    public Pool PopulateWith(PoolType id, GameObject prefab, int amount, int amountPerTick, int tickSize = 1)
    {
        var gameObj = Pools[(int)id].PopulateWith(prefab, amount, amountPerTick, tickSize);
        return gameObj;
    }

    public Pool AddPool(PoolType id, bool reparent = true)
    {
        Pool pool;
        if (Pools.TryGetValue((int)id, out pool) == false)
        {
            pool = new Pool();
            Pools.Add((int)id, pool);
            if (reparent)
            {
                var poolsGameObj = GameObject.Find("[POOLS]") ?? new GameObject("[POOLS]");
                var parentForInactive = new GameObject("Pool: " + id);
                parentForInactive.transform.SetParent(poolsGameObj.transform);
                var slotsGameObj = GameObject.Find("[Slot]") ?? new GameObject("[Slot]");
                var parentForActive = new GameObject("Active: " + id);
                parentForActive.transform.SetParent(slotsGameObj.transform);
                pool.SetParent(parentForInactive.transform, parentForActive.transform);
            }
        }
        return pool;
    }

    public GameObject Spawn(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        return Pools[(int)id].Spawn(prefab, position, rotation, parent);
    }

    public T Spawn<T>(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null) where T : class
    {
        var value = Pools[(int)id].Spawn(prefab, position, rotation, parent);
        return value.GetComponent<T>();
    }

    public void Despawn(PoolType id, GameObject obj)
    {
        Pools[(int)id].Despawn(obj);
    }

    public void Dispose()
    {
        foreach (var poolsValue in Pools.Values) poolsValue.Dispose();
        Pools.Clear();
    }
}