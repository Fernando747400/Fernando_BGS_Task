using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private NavMeshAgent _navMeshAgent;
    [Required][SerializeField] private EnemyAnimator _enemyAnimator;
    [Required][SerializeField] private EnemyMovement _enemyMovement;

    [Header("Settings")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackRate = 5f;

    private PlayerState _currentState;

    public float AttackRate { get { return _attackRate; } }

    public void ChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
        Debug.Log("Enemy state " + _currentState);

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
                Debug.Log("Player hit");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
