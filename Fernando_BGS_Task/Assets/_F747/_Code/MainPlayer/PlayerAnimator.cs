using NaughtyAttributes;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private MainPlayer _mainPlayer;
    [Required][SerializeField] private Animator _playerAnimator;

    [AnimatorParam("_playerAnimator")][SerializeField] private string _idleAnimation;
    [AnimatorParam("_playerAnimator")][SerializeField] private string _walkingAnimation;
    [AnimatorParam("_playerAnimator")][SerializeField] private string _attackingAnimation;
    [AnimatorParam("_playerAnimator")][SerializeField] private string _hitAnimation;
    [AnimatorParam("_playerAnimator")][SerializeField] private string _dyingAnimation;

    private PlayerState _currentState = PlayerState.Idle;

    public void SetPlayerState(PlayerState playerState)
    {
        if(_currentState == playerState) return;
        _currentState = playerState;

        switch (playerState)
        {
            case PlayerState.Idle:
                _playerAnimator.Play(_idleAnimation);
                break;
            case PlayerState.Moving:
                _playerAnimator.Play(_walkingAnimation);
                break;
            case PlayerState.Attacking:
                _playerAnimator.Play(_attackingAnimation);
                break;
                case PlayerState.Hit:
                _playerAnimator.Play(_hitAnimation);
                break;
            case PlayerState.Dying:
                _playerAnimator.Play(_dyingAnimation);
                break;
            case PlayerState.Death:
                break;
                default:
                _playerAnimator.Play(_idleAnimation);
                break;
        }
    } 
    
    public void ChangeState(PlayerState state)
    {
        _mainPlayer.ChangeState(state);
    }

    #region Methods called by animations
    public void SendAttack()
    {
        _mainPlayer.SendAttack();
    }

    #endregion Methods called by animations
}
