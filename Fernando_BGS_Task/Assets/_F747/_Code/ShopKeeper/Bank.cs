using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bank : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private PlayerInput _playerInput;
    [Required][SerializeField] private GameObject _interactPrompObject;

    [Required][SerializeField] private IntVariable _playerMoney;
    [Required][SerializeField] private IntVariable _depositedInBank;
    [Required][SerializeField] private IntVariable _amountToWin;

    [Required][SerializeField] private ScriptableEventNoParam _winGameChannel;

    [SerializeField] private float _interactRange = 2f;
    [SerializeField] private LayerMask _playerLayer;

    private bool _playerNearby = false;
    private InputAction _interactAction;

    private void OnEnable()
    {
        _interactPrompObject.SetActive(false);
    }

    private void Start()
    {
        _playerInput.PlayerInputAction.MainPlayer.Interact.performed += DepositBank;
    }

    private void Update()
    {
        _playerNearby = CheckNearby(_interactRange, _playerLayer);
        UpdatePrompts();
    }

    private bool CheckNearby(float range, LayerMask layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, range, layerMask);

        if (colliders.Length > 0)
        {
            return true;
        }

        return false;
    }

    private void UpdatePrompts()
    {
        if (_playerNearby) _interactPrompObject.SetActive(true);
        else _interactPrompObject.SetActive(false);
    }

    private void DepositBank(InputAction.CallbackContext context)
    {
        if (_playerNearby)
        {
            _depositedInBank.Value += _playerMoney.Value;
            _playerMoney.Value = 0;
            if(_depositedInBank.Value >= _amountToWin.Value)
            {
                _winGameChannel.Raise();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _interactRange);
    }
}
