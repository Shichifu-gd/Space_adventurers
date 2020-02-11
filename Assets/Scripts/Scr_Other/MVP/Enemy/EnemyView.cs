using UnityEngine;
using System;

public class EnemyView : MonoBehaviour, IView, IPool
{
    public ScriptableObjectsSettingsEnemy settingsEnemy;

    [SerializeField] private PoolType poolType;

    public event Action<int> OnTakeDamge;
    public event Action<int> OnUpdateValues;
    public event Action<int> Reward;

    private void Awake()
    {
        SettingPreferences();
    }

    public void HealthAnimation(int health, int maxHealth) { return; }

    public void ObjectShutdown()
    {
        PoolManager.Instance.Despawn(poolType, gameObject);
    }

    public void OnDespawn()
    {
        var random = UnityEngine.Random.Range(0, 15);
        Reward(random);
        OnUpdateValues.Invoke(settingsEnemy.MaxHealth);
    }

    public void OnSpawn() { return; }

    public PoolType GetPoolType()
    {
        return poolType;
    }

    public void SetPoolType(PoolType type)
    {
        poolType = type;
    }

    public void SettingPreferences()
    {
        EnemyModel model = new EnemyModel();
        EnemyPresenter presenter = new EnemyPresenter(this, model);
        OnUpdateValues.Invoke(settingsEnemy.MaxHealth);
        PlayerController playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        Reward += playerController.SetMoney;
        // Reward += playerController.SetScore;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RocketDamage>())
        {
            if (other.GetComponent<RocketDamage>().owner == Owner.Player)
            {
                var damage = other.GetComponentInParent<RocketDamage>().Damage;
                var gameObj = other.GetComponentInParent<IPool>();
                if (gameObj != null) OnTakeDamge(damage);
            }
        }
        if (other.GetComponent<PlayerView>()) ObjectShutdown();
    }
}