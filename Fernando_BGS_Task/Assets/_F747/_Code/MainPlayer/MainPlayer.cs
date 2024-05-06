using NaughtyAttributes;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Camera _playerCamera;
    [Required][SerializeField] private CharacterController _characterController;
    [Required][SerializeField] private PlayerAnimator _playerAnimator;
    [Required][SerializeField] private PlayerInput _playerInput;

    [Header("Settings")]
    [SerializeField] private float _attackRadius = 5f;
    

    private PlayerState _currentState;

    public PlayerState CurrentState { get { return _currentState; }}
    public Camera PlayerCamera { get { return _playerCamera; }}
    public CharacterController CharacterController { get { return _characterController; }}
    public float AttackRadius { get { return _attackRadius; }}

    private void Update()
    {
        
    }

    public void ChangeState(PlayerState newState)
    {
        if(_currentState == newState) return;
        _currentState = newState;

        _playerAnimator.SetPlayerState(_currentState);
        _playerInput.SetPlayerState(_currentState);
    }

    public void SendAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Enemy"))
            {
                //hitCollider.GetComponent<Enemy>().TakeDamage(10);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        //Health -= damage;
        //if(Health <= 0) ChangeState(PlayerState.Dying);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);
    }
}
