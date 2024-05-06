using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    [Header("Dependencies")]
    [Required][SerializeField] private EnemyManager _enemyManager;
    [Required][SerializeField] private Animator _enemyAnimator;

    [AnimatorParam("_enemyAnimator")][SerializeField] private string _idleAnimation;
    [AnimatorParam("_enemyAnimator")][SerializeField] private string _walkingAnimation;
    [AnimatorParam("_enemyAnimator")][SerializeField] private string _attackingAnimation;
    [AnimatorParam("_enemyAnimator")][SerializeField] private string _hitAnimation;
    [AnimatorParam("_enemyAnimator")][SerializeField] private string _dyingAnimation;

    private PlayerState _currentState;

    public void SetState(PlayerState playerState)
    {
        if (_currentState == playerState) return;
        _currentState = playerState;
        Debug.Log("Enemy state in animator" + _currentState);
        switch (_currentState)
        {
            case PlayerState.Idle:
                _enemyAnimator.Play(_idleAnimation);
                break;
            case PlayerState.Moving:
                _enemyAnimator.Play(_walkingAnimation);
                break;
            case PlayerState.Attacking:
                Debug.Log("Calling attack");
                _enemyAnimator.Play(_attackingAnimation);
                break;
            case PlayerState.Hit:
                _enemyAnimator.Play(_hitAnimation);
                break;
            case PlayerState.Dying:
                _enemyAnimator.Play(_dyingAnimation);
                break;
            case PlayerState.Death:
                break;
            default:
                _enemyAnimator.Play(_idleAnimation);
                break;
        }
    }

    public void ChangeState(PlayerState state)
    {
        _enemyManager.ChangeState(state);
    }

    #region Methods called by animations
    public void SendAttack()
    {
        _enemyManager.SendAttack();
    }

    public void FinishAttack()
    {
        _enemyManager.ChangeState(PlayerState.Idle);
    }

    #endregion Methods called by animations
}
