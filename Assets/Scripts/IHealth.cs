public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    void TakeDamage(float damage);
}