using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private NavMeshAgent _navMeshAgent;
    [Required][SerializeField] private EnemyAnimator _enemyAnimator;
    [Required][SerializeField] private EnemyMovement _enemyMovement;
    [Required][SerializeField] private ScriptableEventNoParam _enemyDiedChannel;

    [Header("Settings SO")]
    [Header("Attack")]
    [Required][SerializeField] private FloatVariable _attackRange;
    [Required][SerializeField] private FloatVariable _attackRate;
    [Required][SerializeField] private FloatVariable _attackDamage;

    [Header("Movement")]
    [Required][SerializeField] private FloatVariable _movementSpeed;
    [Required][SerializeField] private FloatVariable _stoppingDistance;

    [Header("Health")]
    [Required][SerializeField] private FloatVariable _maxHealth;
    [Required][SerializeField] private HealthSlider _healthSlider;

    [Header("VFX")]
    [Required][SerializeField] private DeathParticles _deathParticles;

    private PlayerState _currentState;
    private float _currentHealth = 100f;

    public float AttackRange { get { return _attackRange; } }
    public float AttackRate { get { return _attackRate; } }
    public float AttackDamage { get { return _attackDamage; } }
    public float MovementSpeed { get { return _movementSpeed; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public DeathParticles DeathParticles { get { return _deathParticles; } }
    public ScriptableEventNoParam EnemyDiedChannel { get { return _enemyDiedChannel; } }

    private void Awake()
    {
        SetUpEnemy();
    }

    private void OnEnable()
    {       
        SetUpEnemy();
    }

    public void ChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
        _enemyAnimator.SetState(_currentState);
        _enemyMovement.SetState(_currentState);
    }

    public void SendAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
               MainPlayer mainPlayer = hitCollider.GetComponent<MainPlayer>();
               mainPlayer.TakeDamage(_attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthSlider.SetValues(_currentHealth, _maxHealth);
        if(_currentHealth <= 0) ChangeState(PlayerState.Dying);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private void SetUpEnemy()
    {
        _currentHealth = _maxHealth;
        _healthSlider.SetValues(_currentHealth, _maxHealth);
        _enemyAnimator.EnemyManager = this;
        _enemyMovement.EnemyManager = this;
        _enemyMovement.NavMeshAgent = _navMeshAgent;
        ChangeState(PlayerState.Idle);
    }
}
