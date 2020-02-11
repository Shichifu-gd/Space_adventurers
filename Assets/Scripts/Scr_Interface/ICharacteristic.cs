public interface ICharacteristic
{
    int Health { get; set; }

    void TakeDamage(int damage);
    void Destroy();
}