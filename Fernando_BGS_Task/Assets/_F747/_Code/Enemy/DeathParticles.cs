using NaughtyAttributes;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private ParticleSystem _particleSystem;

    private Transform _parent;

    private void OnEnable()
    {
        _parent = this.transform.parent;
        this.transform.localPosition = Vector3.zero;
    }

    public void PlayParticles()
    {
        this.transform.parent = null;
        _particleSystem.Play();
        Invoke("Despawn", _particleSystem.main.duration);
    }

    private void Despawn()
    {
        _particleSystem.Stop();
        this.transform.parent = _parent;      
    }
}
