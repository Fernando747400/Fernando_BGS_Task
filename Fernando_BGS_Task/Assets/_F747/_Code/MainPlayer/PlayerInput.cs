using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private GameObject _playerSprite;
  
    private MainPlayer _mainPlayer;
    private float _moveSpeed = 5f;
    private float _rotationSpeed = 2f;

    private Camera _playerCamera;
    private CharacterController _characterController;

    private PlayerInputAction _playerInputAction;
    private InputAction _moveAction;

    private PlayerState _currentState;

    public MainPlayer MainPlayer { set { _mainPlayer = value; } }
    public float MoveSpeed { set {_moveSpeed = value; } }
    public float RotationSpeed { set { _rotationSpeed = value; } }

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        _playerCamera = _mainPlayer.PlayerCamera;
        _characterController = _mainPlayer.CharacterController;
        _moveAction = _playerInputAction.MainPlayer.Move;
        _playerInputAction.MainPlayer.Attack.started += DoAttack;
        _playerInputAction.MainPlayer.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction.MainPlayer.Attack.started -= DoAttack;
        _playerInputAction.MainPlayer.Disable();
    }

    private void Update()
    {
        Move();
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

        _characterController.SimpleMove(_moveSpeed * moveDirection);

        LookForward(moveDirection);
    }

    private void DoAttack(InputAction.CallbackContext context)
    {
        if (!CanPerformAction()) return;
        _mainPlayer.ChangeState(PlayerState.Attacking);
    }

    private void LookForward(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
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

    
}
