using UnityEngine;

public class Decor : MonoBehaviour, IPool
{
    public PoolType poolType;

    public void ObjectShutdown()
    {
        PoolManager.Instance.Despawn(poolType, gameObject);
    }

    public void OnDespawn() { return; }
    public void OnSpawn() { return; }
    public void SettingPreferences() { return; }
}