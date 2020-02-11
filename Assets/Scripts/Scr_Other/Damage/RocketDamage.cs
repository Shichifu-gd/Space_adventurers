using UnityEngine;

public enum Owner
{
    Player,
    Enemy,
    None,
}

public class RocketDamage : MonoBehaviour
{
    public Owner owner { get; set; } = Owner.None;

    public int Damage { get; set; }

    public void RocketComponents(int damage, Owner type)
    {
        Damage = damage;
        owner = type;
    }
}