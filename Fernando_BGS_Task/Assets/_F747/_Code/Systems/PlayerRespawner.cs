using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private GameObject _playerObject;
    [Required][SerializeField] private ScriptableEventNoParam _playerDiedChannel;

    [Header("Settings")]
    [SerializeField] private Transform[] _respawnPoints;
    [SerializeField] private float _seekRadius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private void OnEnable()
    {
        _playerDiedChannel.OnRaised += RespawnPlayer;
    }

    private void OnDisable()
    {
        _playerDiedChannel.OnRaised -= RespawnPlayer;
    }

    private void RespawnPlayer()
    {
        _playerObject.transform.position = GetSpawnPoint().position;
        MainPlayer mainPlayer = _playerObject.GetComponent<MainPlayer>();
        mainPlayer.CurrentHealth.Value = mainPlayer.MaxHealth;
    }

    private Transform GetSpawnPoint()
    {
        Transform selectedSpawnPoint = _respawnPoints[0];
        int enemiesOnSelected = int.MaxValue;
        for (int i = 0; i < _respawnPoints.Length; i++)
        {
            int enemiesOnCurrent = 0;
            Collider[] colliders = Physics.OverlapSphere(_respawnPoints[i].position, _seekRadius, _enemyLayer);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    enemiesOnCurrent++;
                }
            }
            if (enemiesOnCurrent < enemiesOnSelected)
            {
                selectedSpawnPoint = _respawnPoints[i];
                enemiesOnSelected = enemiesOnCurrent;
            }
        }
        return selectedSpawnPoint;
    }
}
