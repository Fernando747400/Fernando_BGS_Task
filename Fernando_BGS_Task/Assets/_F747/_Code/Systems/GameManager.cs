using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dependencies SO")]
    [Required][SerializeField] private ScriptableEventNoParam _enemyDiedChannel;
    [Required][SerializeField] private ScriptableEventNoParam _playerDiedChannel;

    [Header("Setting Dependencies")]
    [Required][SerializeField] private IntVariable _enemyDeathValue;
    [Required][SerializeField] private IntVariable _playerDeathValue;

    [Required][SerializeField] private IntVariable _playerMoney;


    private void OnEnable()
    {
        _enemyDiedChannel.OnRaised += EnemyDied;
        _playerDiedChannel.OnRaised += PlayerDied;
    }

    private void OnDisable()
    {
        _enemyDiedChannel.OnRaised -= EnemyDied;
        _playerDiedChannel.OnRaised -= PlayerDied;
    }

    private void EnemyDied()
    {
        _playerMoney.Value += _enemyDeathValue;
    }

    private void PlayerDied()
    {
        _playerMoney.Value = _playerDeathValue;
    }
}
