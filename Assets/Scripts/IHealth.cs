public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    float TakeDamage(float damage);
}