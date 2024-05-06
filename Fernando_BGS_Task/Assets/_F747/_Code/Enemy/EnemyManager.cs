using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private NavMeshAgent _navMeshAgent;
}
