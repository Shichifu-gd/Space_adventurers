using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Pool
{
    protected int Index;

    private Transform ParentForInactive;
    private Transform ParentForActive;

    private Dictionary<int, Stack<GameObject>> CachedObjects = new Dictionary<int, Stack<GameObject>>();
    private Dictionary<int, int> CachedIds = new Dictionary<int, int>();

    public Pool PopulateWith(GameObject prefab, int amount, int amountPerTick, int tickSize = 1)
    {
        var key = prefab.GetInstanceID();
        Stack<GameObject> stack;
        var stacked = CachedObjects.TryGetValue(key, out stack);
        if (stacked == false) CachedObjects.Add(key, new Stack<GameObject>());
        Observable.IntervalFrame(tickSize, FrameCountType.EndOfFrame).Where(val => amount > 0).Subscribe(_loop =>
        {
            Observable.Range(0, amountPerTick).Where(check => amount > 0).Subscribe(_pop =>
            {
                Index = amount;
                var gameObj = Populate(prefab, Vector3.zero, Quaternion.identity, ParentForInactive);
                gameObj.SetActive(false);
                CachedIds.Add(gameObj.GetInstanceID(), key);
                CachedObjects[key].Push(gameObj);
                amount--;
            });
        });
        return this;
    }

    public void SetParent(Transform parentForInactive, Transform parentForActive)
    {
        ParentForInactive = parentForInactive;
        ParentForActive = parentForActive;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        Index++;
        var key = prefab.GetInstanceID();
        Stack<GameObject> stack;
        var stacked = CachedObjects.TryGetValue(key, out stack);
        if (stacked && stack.Count > 0)
        {
            var transform = stack.Pop().transform;
            if (parent == null) transform.SetParent(ParentForActive);
            else transform.SetParent(parent);
            transform.rotation = rotation;
            transform.gameObject.SetActive(true);
            if (parent) transform.position = position;
            else transform.localPosition = position;
            var poolable = transform.GetComponent<IPool>();
            if (poolable != null) poolable.OnSpawn();
            return transform.gameObject;
        }
        if (!stacked) CachedObjects.Add(key, new Stack<GameObject>());
        var createdPrefab = Populate(prefab, position, rotation, parent);
        var mvp = createdPrefab.GetComponent<IPool>();
        if (mvp != null) mvp.SettingPreferences();
        CachedIds.Add(createdPrefab.GetInstanceID(), key);
        return createdPrefab;
    }

    public void Despawn(GameObject gameObj)
    {
        Index--;
        gameObj.SetActive(false);
        CachedObjects[CachedIds[gameObj.GetInstanceID()]].Push(gameObj);
        var poolable = gameObj.GetComponent<IPool>();
        if (poolable != null) poolable.OnDespawn();
        if (ParentForInactive != null) gameObj.transform.SetParent(ParentForInactive);
    }

    public void Dispose()
    {
        ParentForInactive = null;
        CachedObjects.Clear();
        CachedIds.Clear();
    }

    GameObject Populate(GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        var gameObj = Object.Instantiate(prefab, position, rotation, parent).transform;
        //go.name += "_" + Index;
        if (parent == null) gameObj.SetParent(ParentForActive);
        else gameObj.localPosition = position;
        return gameObj.gameObject;
    }
}