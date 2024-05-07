using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventNoParam _PotionPickedChannel;
    [Required][SerializeField] private IntVariable _playerPotions;
    [Required][SerializeField] private GameObject _potion;

    [Header("Game Pause")]
    [Required][SerializeField] private ScriptableEventBool _gamePauseChannel;

    [Header("Settings")]
    [MinMaxSlider(30,300)]
    [SerializeField] private Vector2 _spawnRate = new Vector2(30f,300f);

    private float _elapsedTime = 0;
    private float _spawnTime = 0;

    private bool _paused = false;

    private void OnEnable()
    {
        _gamePauseChannel.OnRaised += UpdatePause;
        _PotionPickedChannel.OnRaised += PickUpPotion;
    }

    private void OnDisable()
    {
        _gamePauseChannel.OnRaised -= UpdatePause;
        _PotionPickedChannel.OnRaised -= PickUpPotion;
    }

    private void Update()
    {
        if (_paused) return;
        if(_potion.activeInHierarchy) return;
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= _spawnTime)
        {
            _potion.SetActive(true);
            GetSpawnTime();
            _elapsedTime = 0;
        } 
    }

    private void GetSpawnTime()
    {
        _spawnTime = Random.Range(_spawnRate.x, _spawnRate.y);
    }

    private void UpdatePause(bool pause)
    {
        _paused = pause;
    }

    private void PickUpPotion()
    {
        _playerPotions.Value++;
    }
}
