using Lean.Pool;
using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private GameObject _enemyPrefab;
    [Required][SerializeField] private MainPlayer _mainPlayer;
    [Required][SerializeField] private IntVariable _enemiesAlive;

    [Header("Dependencies Channels SO")]
    [Required][SerializeField] private ScriptableEventInt _starSpawningChannel;
    [Required][SerializeField] private ScriptableEventNoParam _finishedSpawningChannel;

    [Header("Settings SO")]
    [Required][SerializeField] private FloatVariable _spawnCadence;

    [Header("Settings")]
    [SerializeField] private bool _spawnAwayFromPlayer = false;

    [Header("Spawn Array")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Game Pause")]
    [Required][SerializeField] private ScriptableEventBool _gamePauseChannel;

    private int _enemiesToSpawn = 0;

    private float _elpasedTime = 0;
    private bool _spawning = false;

    private bool _paused = false;

    public bool Spawning { get { return _spawning; } }

    private void OnEnable()
    {
        _starSpawningChannel.OnRaised += SpawnEnemy;
        _gamePauseChannel.OnRaised += UpdatePause;
    }

    private void OnDisable()
    {
        _starSpawningChannel.OnRaised -= SpawnEnemy;
        _gamePauseChannel.OnRaised -= UpdatePause;
    }

    private void Update()
    {
        if(!_paused) _elpasedTime += Time.deltaTime;
        HandleSpawn();
    }

    public void SpawnEnemy(int numberToSpawn)
    {
        _enemiesToSpawn += numberToSpawn;
        _spawning = true;
    }

    public void SpawnEnemy(int numberToSpawn, float cadence = 10)
    {
        _enemiesToSpawn += numberToSpawn;
        _spawnCadence.Value = cadence;
        _spawning = true;
    }

    private void HandleSpawn()
    {
        if (_elpasedTime < _spawnCadence) return;

        if (_enemiesToSpawn > 0)
        {
            _elpasedTime = 0;
            _enemiesToSpawn--;
            _enemiesAlive.Value = _enemiesAlive.Value + 1;
            Spawn();
        }
        else
        {
            if (_spawning) _finishedSpawningChannel.Raise();
            _spawning = false;
        }
    }

    private void Spawn()
    {
        Transform spawnPoint;
        if (_spawnAwayFromPlayer) spawnPoint = GetSpawnAwayFromPlayer();
        else spawnPoint = GetSpawn();

        GameObject enemy = LeanPool.Spawn(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.Target = _mainPlayer.transform;
    }

    private Transform GetSpawn()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }

    private Transform GetSpawnAwayFromPlayer()
    {
        Transform spawnPoint;
        Transform playerTransform = _mainPlayer.transform;

        spawnPoint = _spawnPoints[0];
        foreach (var point in _spawnPoints)
        {
            if (Vector3.Distance(point.position, playerTransform.position) > Vector3.Distance(spawnPoint.position, playerTransform.position))
            {
                spawnPoint = point;
            }
        }

        return spawnPoint;
    }

    private void UpdatePause(bool paused)
    {
        _paused = paused;
    }
}
