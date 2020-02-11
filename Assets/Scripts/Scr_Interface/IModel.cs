using System;

public interface IModel
{
    int MaximumHealth { get; set; }
    int CurrentHealth { get; set; }

    event Action<int, int> OnHealthChanged;
    event Action Death;

    void UpdateValues(int maxHealth);
    void ResiveDemage(int damage);
}