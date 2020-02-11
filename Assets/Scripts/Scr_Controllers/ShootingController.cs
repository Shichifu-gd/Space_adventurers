using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private PoolType poolType = new PoolType();

    [SerializeField]
    private Owner RocketOwner = new Owner();

    public GameObject PreRocket;

    [SerializeField]
    private int Damage = 1;
    [SerializeField]
    private float SpeedRocket = 1;

    [SerializeField]
    private float BaseRechargeTime = 3;
    private float CurrentRechargeTime;

    [SerializeField] private Transform[] Guns = new Transform[0];

    private void Start()
    {
        CurrentRechargeTime = 0;
    }

    private void FixedUpdate()
    {
        if (CanAttack()) Attack();
        if (CurrentRechargeTime <= BaseRechargeTime) CurrentRechargeTime += Time.deltaTime;
    }

    private bool CanAttack()
    {
        if (CurrentRechargeTime >= BaseRechargeTime) return true;
        return false;
    }

    private void Attack()
    {
        foreach (var point in Guns)
        {
            GameObject rocket = PoolManager.Instance.Spawn(poolType, PreRocket, point.position, point.rotation);
            rocket.GetComponent<RocketDamage>().RocketComponents(Damage, RocketOwner);
            rocket.GetComponent<RocketMovement>().SpeedDetermination(SpeedRocket);
        }
        CurrentRechargeTime = 0;
    }
}