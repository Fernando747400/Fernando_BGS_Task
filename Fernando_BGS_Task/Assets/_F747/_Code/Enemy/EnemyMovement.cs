using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyManager))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Game Pause")]
    [Required][SerializeField] private ScriptableEventBool _gamePauseChannel;

    private EnemyManager _enemyManager;
    private NavMeshAgent _navMeshAgent;

    private Transform _target;

    private PlayerState _currentState = PlayerState.Idle;
    private float _elapsedAttackTime = 0f;

    private bool _paused = false;

    public Transform Target { get { return _target; } set { _target = value; } }
    public EnemyManager EnemyManager { set { _enemyManager = value; } }
    public NavMeshAgent NavMeshAgent { set {  _navMeshAgent = value; SetUpNavMesh(); } }

    private void OnEnable()
    {
        _gamePauseChannel.OnRaised += UpdatePause;
    }

    private void OnDisable()
    {
        _gamePauseChannel.OnRaised -= UpdatePause;
    }

    private void Update()
    {
        MoveToTarget();
        if(_currentState != PlayerState.Paused && !_paused) _elapsedAttackTime += Time.deltaTime;
    }

    public void MoveToTarget()
    {
        if(_target == null)
        {
            _enemyManager.ChangeState(PlayerState.Idle);
            return;
        }

         if(!CanPerformAction() || _paused) return;
        _navMeshAgent.SetDestination(_target.position);
        
        if (CheckArrival())
        {
            if (_elapsedAttackTime < _enemyManager.AttackRate) return;
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

    private void UpdatePause(bool pause) 
    { 
        _paused = pause;
        if (CanPerformAction()) _enemyManager.ChangeState(PlayerState.Paused);
        if (!_paused && _enemyManager.CurrentState == PlayerState.Paused) _enemyManager.ChangeState(PlayerState.Idle);

        if(_paused) _navMeshAgent.isStopped = true;
        else _navMeshAgent.isStopped = false;
    }
}
