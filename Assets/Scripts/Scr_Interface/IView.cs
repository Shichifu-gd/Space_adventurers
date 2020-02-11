using System;

public interface IView
{
    event Action<int> OnTakeDamge;
    event Action<int> OnUpdateValues;

    void HealthAnimation(int health, int maxHealth);
}