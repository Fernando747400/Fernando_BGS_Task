using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private Transform _target;

    public Transform Target { get { return _target; } set { _target = value; } }


    private void FixedUpdate()
    {
        MoveToTarget();
    }

    public void MoveToTarget()
    {
        if(_target == null) return;

        _navMeshAgent.SetDestination(_target.position);
    }
}
