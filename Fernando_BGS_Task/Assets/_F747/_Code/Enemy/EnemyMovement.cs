using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyManager))]
public class EnemyMovement : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private Transform _target;

    private PlayerState _currentState = PlayerState.Idle;
    private float _elapsedAttackTime = 0f;

    public Transform Target { set { _target = value; } }
    public EnemyManager EnemyManager { set { _enemyManager = value; } }
    public NavMeshAgent NavMeshAgent { set {  _navMeshAgent = value; SetUpNavMesh(); } }


    private void Update()
    {
        MoveToTarget();
        if(_currentState != PlayerState.Paused) _elapsedAttackTime += Time.deltaTime;
    }

    public void MoveToTarget()
    {
        if(_target == null)
        {
            _enemyManager.ChangeState(PlayerState.Idle);
            return;
        }

        _navMeshAgent.SetDestination(_target.position);
        
        if (CheckArrival())
        {
            if (_elapsedAttackTime < _enemyManager.AttackRate) return;
            if(!CanPerformAction()) return;
            _enemyManager.ChangeState(PlayerState.Attacking);
            _elapsedAttackTime = 0f;
        }
        else
        {
            _enemyManager.ChangeState(PlayerState.Moving);
        }    
    }

    public void SetState(PlayerState state)
    {
        if (_currentState == state) return;
        _currentState = state;
    }

    private bool CheckArrival()
    {
        float distance = _navMeshAgent.remainingDistance;
        if (distance != Mathf.Infinity && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetUpNavMesh()
    {
        _navMeshAgent.speed = _enemyManager.MovementSpeed;
        _navMeshAgent.stoppingDistance = _enemyManager.StoppingDistance;
    }

    private bool CanPerformAction()
    {
        return (_currentState != PlayerState.Attacking && _currentState != PlayerState.Dying && _currentState != PlayerState.Death && _currentState != PlayerState.Paused);
    }
}
