using NaughtyAttributes;
using Obvious.Soap;
using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] ScriptableEventNoParam _gameWinChannel;
    [Required][SerializeField] ScriptableEventInt _gameStartedChannel;
    [Required][SerializeField] TextMeshProUGUI _finalClock;
    [Required][SerializeField] GameObject _finalCanvas;
    [Required][SerializeField] GameObject _mainCanvas;

    private bool _started = false;
    private float _elapsedTime = 0;

    private void OnEnable()
    {
        _gameWinChannel.OnRaised += StopTimer;
        _gameStartedChannel.OnRaised += StartTimer;
    }


    private void OnDisable()
    {
        _gameWinChannel.OnRaised -= StopTimer;
        _gameStartedChannel.OnRaised -= StartTimer;
    }

    private void Update()
    {
        if(_started)_elapsedTime += Time.deltaTime;
        UpdateClock();
    }

    private void UpdateClock()
    {
       _finalClock.text = TimeSpan.FromSeconds(_elapsedTime).ToString(@"mm\:ss");
    }

    private void StartTimer(int noUse)
    {
      if(!_started) _started = true;
    }
    private void StopTimer()
    {
      _started = false;
        _finalCanvas.SetActive(true);
        _mainCanvas.SetActive(false);
    }
}
