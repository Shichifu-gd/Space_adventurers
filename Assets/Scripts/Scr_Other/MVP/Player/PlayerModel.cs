using System;

public class PlayerModel : IModel
{
    public int MaximumHealth { get; set; }
    public int CurrentHealth { get; set; }

    public event Action<int, int> OnHealthChanged;
    public event Action Death;

    public void UpdateValues(int maxHealth)
    {
        MaximumHealth = maxHealth;
        CurrentHealth = MaximumHealth;
    }

    public void ResiveDemage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth >= 0) OnHealthChanged?.Invoke(CurrentHealth, MaximumHealth);
        else Death?.Invoke();
    }
}