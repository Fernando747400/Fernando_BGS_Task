using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopKeeper : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private PlayerInput _playerInput;
    [Required][SerializeField] private GameObject _interactPrompObject;
    [Required][SerializeField] private GameObject _enemiesNearbyPrompObject;

    public SceneLoader SceneLoader;

    [SerializeField] private float _interactRange = 2f;
    [SerializeField] private float _enemiesNearbyRange = 10f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _enemyLayer;

    private bool _playerNearby = false;
    private bool _enemiesNearby = false;
    private InputAction _interactAction;

    private void Start()
    {
        _playerInput.PlayerInputAction.MainPlayer.Interact.performed += OpenStore;
    }

    private void OnEnable()
    {
        _interactPrompObject.SetActive(false);
        _enemiesNearbyPrompObject.SetActive(false);
    }

    private void OnDisable()
    {
        _playerInput.PlayerInputAction.MainPlayer.Interact.started -= OpenStore;
    }

    private void Update()
    {
        _playerNearby = CheckNearby(_interactRange, _playerLayer);
        _enemiesNearby = CheckNearby(_enemiesNearbyRange, _enemyLayer);
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
        if(_playerNearby)
        {
            if (!_enemiesNearby)
            {
                _interactPrompObject.SetActive(true);
                _enemiesNearbyPrompObject.SetActive(false);
            }
            else
            {
                _interactPrompObject.SetActive(false);
                _enemiesNearbyPrompObject.SetActive(true);
            }
           
        }
        else
        {
            _interactPrompObject.SetActive(false);
            _enemiesNearbyPrompObject.SetActive(false);
        }
    }

    private void OpenStore(InputAction.CallbackContext context)
    {
        if(_playerNearby && !_enemiesNearby)
        {
            Debug.Log("Open Store");
           SceneLoader.LoadStore(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _interactRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _enemiesNearbyRange);
    }
}
