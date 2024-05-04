using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ShoopingSpree/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Dependencies")]
    [Required][SerializeField] private FloatVariable _baseHealth;
    [Required][SerializeField] private FloatVariable _currentHealth;
    [Required][SerializeField] private FloatVariable _maxHealth;
    [Required][SerializeField] private FloatVariable _baseSpeed;
    [Required][SerializeField] private FloatVariable _currentSpeed;
    [Required][SerializeField] private FloatVariable _baseDamage;
    [Required][SerializeField] private FloatVariable _currentDamage;

    public FloatVariable BaseHealth { get => _baseHealth; }
    public FloatVariable CurrentHealth { get => _currentHealth; }
    public FloatVariable MaxHealth { get => _maxHealth; }
    public FloatVariable BaseSpeed { get => _baseSpeed; }
    public FloatVariable CurrentSpeed { get => _currentSpeed; }
    public FloatVariable BaseDamage { get => _baseDamage; }
    public FloatVariable CurrentDamage { get => _currentDamage; }


    public void HealthUpdate(float valueToModify)
    {
        _currentHealth.Value += valueToModify;
        if(_currentHealth.Value > _maxHealth.Value)
        {
            _currentHealth.Value = _maxHealth.Value;
        }
        if(_currentHealth.Value < 0)
        {
            _currentHealth.Value = 0;
        }
    }

    public void MaxHealthUpdate(float maxHealth)
    {
        _maxHealth.Value = maxHealth;
        if(_currentHealth.Value > _maxHealth.Value)
        {
            _currentHealth.Value = _maxHealth.Value;
        }

        _maxHealth.Save();
    }

    public void SpeedUpdate(float speed)
    {
        _currentSpeed.Value = speed;
        _currentSpeed.Save();
    }

    public void DamageUpdate(float damage)
    {
        _currentDamage.Value = damage;
        _currentDamage.Save();
    }
}
