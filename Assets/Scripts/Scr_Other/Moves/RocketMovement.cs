using UnityEngine;

public class RocketMovement : MonoBehaviour, IPool
{
    [SerializeField]
    private PoolType poolType = new PoolType();

    private float Speed;

    [SerializeField]
    private bool IsActive = true;

    private new Rigidbody rigidbody;

    [SerializeField]
    private DirectionMovement directionMovement = new DirectionMovement();

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (IsActive) StartMovementObjects();
    }

    public void Components(float speed, DirectionMovement type)
    {
        Speed = speed;
        directionMovement = type;
    }

    private void StartMovementObjects()
    {
        if (directionMovement == DirectionMovement.North) rigidbody.velocity = transform.up * Speed;
        if (directionMovement == DirectionMovement.South) rigidbody.velocity = -transform.up * Speed;
    }

    public void SpeedDetermination(float speed)
    {
        Speed = speed;
    }

    public void DeterminationDirectionMovement(DirectionMovement type)
    {
        directionMovement = type;
    }

    public PoolType GetPoolType()
    {
        return poolType;
    }

    public void SetPoolType(PoolType type)
    {
        poolType = type;
    }

    public void OnSpawn() { return; }

    public void OnDespawn() { return; }

    public void SettingPreferences() { return; }

    public void ObjectShutdown()
    {
        PoolManager.Instance.Despawn(poolType, gameObject);
    }
}