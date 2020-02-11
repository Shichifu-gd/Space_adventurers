using UnityEngine;
using System;

public class PlayerView : MonoBehaviour, IView, IPool
{
    private UIActive PanelGameEnd = new UIActive();
    public ScriptableObjectsSettingsPlayer settingsPlayer;

    [SerializeField]
    private PoolType poolType;

    private float Speed = 15;
    private float Z_Tilt = 0.4f;
    private float X_Tilt = 0.6f;
    private float MoveHorizontal;
    private float MoveVertical;

    private Boundary boundary = new Boundary();
    private BoundarySettings boundarySettings = new BoundarySettings();

    private new Rigidbody rigidbody;

    public event Action<int> OnTakeDamge;
    public event Action<int> OnUpdateValues;
    public event Action<float> OnUpdateLife;

    private void Awake()
    {
        PanelGameEnd = GameObject.FindGameObjectWithTag("GameEnd").GetComponent<UIActive>();
        PanelGameEnd.OffUI();
        rigidbody = GetComponent<Rigidbody>();
        SettingPreferences();
    }

    private void Start()
    {
        boundarySettings.SetBoundary(boundary, Resolution.R_16_9);
        Speed = settingsPlayer.Speed;
    }

    void FixedUpdate()
    {
        MoveHorizontal = Input.GetAxis("Horizontal");
        MoveVertical = Input.GetAxis("Vertical");
        Move();
        Tilts();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(MoveHorizontal, 0.0f, MoveVertical);
        rigidbody.velocity = movement * Speed;
        rigidbody.position = new Vector3
        (
            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
        );
    }

    private void Tilts()
    {
        if (MoveHorizontal > 0 || MoveHorizontal < 0) rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -X_Tilt);
        if (MoveVertical > 0 || MoveVertical < 0) rigidbody.rotation = Quaternion.Euler(rigidbody.velocity.z * -Z_Tilt, 0.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RocketDamage>())
        {
            if (other.GetComponent<RocketDamage>().owner == Owner.Enemy)
            {
                var damage = other.GetComponentInParent<RocketDamage>().Damage;
                var gameObj = other.GetComponentInParent<IPool>();
                if (gameObj != null)
                {
                    gameObj.ObjectShutdown();
                    OnTakeDamge(damage);
                }
            }
        }
        if (other.GetComponent<EnemyView>()) OnTakeDamge.Invoke(10);
    }

    public void HealthAnimation(int health, int MaxHealth)
    {
        float num = health;
        OnUpdateLife.Invoke(num / MaxHealth);
    }

    public void OnDespawn()
    {
        OnUpdateValues.Invoke(settingsPlayer.MaxHealth);
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
        PlayerModel playerModel = new PlayerModel();
        PlayerPresenter playerController = new PlayerPresenter(this, playerModel);
        OnUpdateValues.Invoke(settingsPlayer.MaxHealth);
    }

    public void ObjectShutdown()
    {
        PanelGameEnd.OnUI();
        PoolManager.Instance.Despawn(poolType, gameObject);
    }
}