using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private MainPlayer _mainPlayer;
    [Required][SerializeField] private GameObject _playerSprite;
    [Required][SerializeField] private HealthSlider _attackSlider;

    [Header("Game Pause")]
    [Required][SerializeField] private ScriptableEventBool _gamePauseChannel;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private PlayerInputAction _playerInputAction;
    private InputAction _moveAction;

    private PlayerState _currentState;

    private float _elpasedAttackTime = 0;
    private bool _paused = false;

    public MainPlayer MainPlayer { set { _mainPlayer = value; } }
    public PlayerInputAction PlayerInputAction { get { return _playerInputAction; } }

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.MainPlayer.Enable();
    }

    private void OnEnable()
    {
        _playerCamera = _mainPlayer.PlayerCamera;
        _characterController = _mainPlayer.CharacterController;
        _moveAction = _playerInputAction.MainPlayer.Move;
        _playerInputAction.MainPlayer.Attack.started += DoAttack;
        _gamePauseChannel.OnRaised += UpdatePause;
    }

    private void OnDisable()
    {
        _playerInputAction.MainPlayer.Attack.started -= DoAttack;
        _playerInputAction.MainPlayer.Disable();
        _gamePauseChannel.OnRaised -= UpdatePause;
    }

    private void Update()
    {
        Move();
        if(_currentState != PlayerState.Paused && !_paused) _elpasedAttackTime += Time.deltaTime;
        _attackSlider.SetValues(_elpasedAttackTime, _mainPlayer.AttackSpeed);
    }

    public void SetPlayerState(PlayerState state)
    {
        if (_currentState == state) return;
        _currentState = state;
    }

    private void Move()
    {
        if (!CanPerformAction()) return;
        Vector2 movementInput = _moveAction.ReadValue<Vector2>();

        if (movementInput == Vector2.zero) _mainPlayer.ChangeState(PlayerState.Idle);
        else _mainPlayer.ChangeState(PlayerState.Moving);

        ChangeDirection(movementInput);
        Vector3 cameraDirection = GetCameraDirection();

        Vector3 moveDirection = cameraDirection * movementInput.y + _playerCamera.transform.right * movementInput.x;
        moveDirection.y = 0f;

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        _characterController.SimpleMove(_mainPlayer.MoveSpeed * moveDirection);

        LookForward(moveDirection);
    }

    private void DoAttack(InputAction.CallbackContext context)
    {
        if(_elpasedAttackTime < _mainPlayer.AttackSpeed) return;
        if (!CanPerformAction()) return;
        _mainPlayer.ChangeState(PlayerState.Attacking);
        _elpasedAttackTime = 0f;
    }

    private void LookForward(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _mainPlayer.MoveSpeed * Time.deltaTime);
    }

    private void ChangeDirection(Vector2 inputVector)
    {
        Vector3 localScale = _playerSprite.transform.localScale;
        if (Mathf.Sign(inputVector.x) > 0)
        {
            _playerSprite.transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y , localScale.z);
        } else
        {
            _playerSprite.transform.localScale = new Vector3(Mathf.Abs(localScale.x) * -1, localScale.y , localScale.z);
        }
    }

    private Vector3 GetCameraDirection()
    {
        if(_playerCamera == null) _playerCamera = _mainPlayer.PlayerCamera;
        return Vector3.Scale(_playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
    }

    private bool CanPerformAction()
    {
        return (_currentState != PlayerState.Attacking && _currentState != PlayerState.Dying && _currentState != PlayerState.Death && _currentState != PlayerState.Paused);
    }

    private void UpdatePause(bool paused)
    {
        _paused = paused;
        if (CanPerformAction()) _mainPlayer.ChangeState(PlayerState.Paused);
        if(!_paused && _mainPlayer.CurrentState == PlayerState.Paused) _mainPlayer.ChangeState(PlayerState.Idle);
    }
    
}
