using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Camera _playerCamera;
    [Required][SerializeField] private CharacterController _characterController;
    [Required][SerializeField] private PlayerAnimator _playerAnimator;
    [Required][SerializeField] private PlayerInput _playerInput;
    [Required][SerializeField] private ScriptableEventNoParam _playerDiedChannel;

    [Header("Player Health")]
    [Required][SerializeField] private FloatVariable _playerCurrentHealth;

    [Header("Player Movement")]
    [Required][SerializeField] private FloatVariable _playerSpeedCurrent;
    [Required][SerializeField] private FloatVariable _playerRotationSpeedCurrent;

    [Header("Attack")]
    [Required][SerializeField] private FloatVariable _attackRadiusCurrent;
    [Required][SerializeField] private FloatVariable _attackDamageCurrent;

    [Header("Final Curves")]
    [CurveRange(50, 40, 115, 100, EColor.Green)] // min.x, min.y, max.x, max.y
    [SerializeField] private AnimationCurve _attackDamageCurve;

    [CurveRange(50, 2, 140, 5, EColor.Orange)]
    [SerializeField] private AnimationCurve _attackSpeedCurve;

    [CurveRange(50, 2, 140, 5, EColor.Red)]
    [SerializeField] private AnimationCurve _moveSpeedCurve;
    

    private PlayerState _currentState;
     
    public PlayerState CurrentState { get { return _currentState; }}
    public Camera PlayerCamera { get { return _playerCamera; }}
    public CharacterController CharacterController { get { return _characterController; }}
    public float AttackRadius { get { return _attackRadiusCurrent; }}
    public float AttackDamage { get { return EvaluateCurve(_attackDamageCurve, _attackDamageCurrent); }}
    public float AttackSpeed { get { return EvaluateCurve(_attackSpeedCurve, _playerSpeedCurrent); }}
    public float MoveSpeed { get { return EvaluateCurve(_moveSpeedCurve, _playerSpeedCurrent); }}


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
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _attackRadiusCurrent);

        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Enemy"))
            {
                EnemyManager enemyManager = hitCollider.GetComponent<EnemyManager>();
                enemyManager.TakeDamage(AttackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _playerCurrentHealth.Value -= damage;
        if(_playerCurrentHealth <= 0) ChangeState(PlayerState.Dying);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _attackRadiusCurrent);
    }

    private float EvaluateCurve(AnimationCurve curve, float value)
    {
        return curve.Evaluate(value);
    }
}
