using Lean.Pool;
using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventNoParam _itemPickedChannel;

    [Header("Settings")]
    [SerializeField] private bool _poolable = false;
    [HideIf("_poolable")]
    [SerializeField] private bool _deactivable = false;
    [HideIf("_deactivable")]
    [SerializeField] private bool _destroyable = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _itemPickedChannel.Raise();
            if(_poolable)
            {
                LeanPool.Despawn(gameObject);
            }
            else if (_deactivable)
            {
                gameObject.SetActive(false);
            } else if (_destroyable)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            _itemPickedChannel.Raise();
            if(_poolable)
            {
                LeanPool.Despawn(gameObject);
            }
            else if (_deactivable)
            {
                gameObject.SetActive(false);
            } else if (_destroyable)
            {
                Destroy(gameObject);
            }
        }
    }
}
