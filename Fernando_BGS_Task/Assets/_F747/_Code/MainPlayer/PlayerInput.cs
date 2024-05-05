using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Camera _playerCamera;
    [Required][SerializeField] private CharacterController _characterController;
    [Required][SerializeField] private GameObject _playerSprite;


    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 2f;

    private PlayerInputAction _playerInputAction;
    private InputAction _moveAction;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        _moveAction = _playerInputAction.MainPlayer.Move;
        _playerInputAction.MainPlayer.Enable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementInput = _moveAction.ReadValue<Vector2>();
        ChangeDirection(movementInput);
        Vector3 cameraDirection = GetCameraDirection();

        Vector3 moveDirection = cameraDirection * movementInput.y + _playerCamera.transform.right * movementInput.x;
        moveDirection.y = 0f;

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        _characterController.SimpleMove(_moveSpeed * moveDirection);

        LookForward(moveDirection);
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
        return Vector3.Scale(_playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
    }

    
}
