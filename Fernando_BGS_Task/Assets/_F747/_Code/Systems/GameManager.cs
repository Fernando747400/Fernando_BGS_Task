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

    [Header("Rounds Settings")]
    [Required][SerializeField] private ScriptableEventInt _starSpawningChannel;
    [Required][SerializeField] private ScriptableEventNoParam _finishedSpawningChannel;
    [Required][SerializeField] private IntVariable _currentRound;
    [Required][SerializeField] private IntVariable _enemiesAlive;
    [CurveRange(0, 1, 100, 50, EColor.Green)] // min.x, min.y, max.x, max.y
    [SerializeField] private AnimationCurve _enemiesByRound;

    [CurveRange(0, 10, 100, 120, EColor.Green)] // min.x, min.y, max.x, max.y
    [SerializeField] private AnimationCurve _timeBetweenRounds;

    [Header("Economy")]
    [Required][SerializeField] private IntVariable _playerMoney;

    [Header("Game Pause")]
    [Required][SerializeField] private ScriptableEventBool _gamePauseChannel;

    private bool _lastEnemyDied = false;
    private bool _lastEnemySpawned = false;
    private bool _readyForNextRound = false;
    private bool _paused = false;

    private float _elapsedTime = 0;

    private void OnEnable()
    {
        _enemyDiedChannel.OnRaised += EnemyDied;
        _playerDiedChannel.OnRaised += PlayerDied;
        _gamePauseChannel.OnRaised += UpdatePause;
        _finishedSpawningChannel.OnRaised += LastEnemySpawned;
        _gamePauseChannel.Raise(true);
    }

    private void OnDisable()
    {
        _enemyDiedChannel.OnRaised -= EnemyDied;
        _playerDiedChannel.OnRaised -= PlayerDied;
        _gamePauseChannel.OnRaised -= UpdatePause;
        _finishedSpawningChannel.OnRaised -= LastEnemySpawned;
    }

    private void Update()
    {
        TryNextRound();
    }

    [Button]
    public void StartGame()
    {
        _starSpawningChannel.Raise(Mathf.RoundToInt(_enemiesByRound.Evaluate(_currentRound.Value)));
        _currentRound.Value++;
        _lastEnemySpawned = false;
        _lastEnemyDied = false;
    }

    private void EnemyDied()
    {
        _playerMoney.Value += _enemyDeathValue;
        _enemiesAlive.Value--;
    }

    private void PlayerDied()
    {
        _playerMoney.Value = _playerDeathValue;
    }

    private void UpdatePause(bool pause)
    {
        _paused = pause;
    }

    private void TryNextRound()
    {
        _lastEnemyDied = (_enemiesAlive.Value == 0);
        _readyForNextRound = (_lastEnemyDied && _lastEnemySpawned);
        if (_paused || !_readyForNextRound) return;
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < _timeBetweenRounds.Evaluate(_currentRound)) return;

        _elapsedTime = 0;
        StartGame();
    }

    private void LastEnemySpawned()
    {
        _lastEnemySpawned = true;
    }
}
