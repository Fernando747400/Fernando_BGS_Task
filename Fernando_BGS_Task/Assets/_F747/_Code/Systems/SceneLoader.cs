using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Obvious.Soap;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "ShoopingSpree/SceneLoader", order = 1)]
public class SceneLoader : ScriptableObject
{
    [Header("Dependencies")]
    [Required]
    [SerializeField] private ScriptableEventBool _gamePausedChannel;

    [Header("Settings")]
    [Scene][SerializeField] private string _mainScene;
    [Scene][SerializeField] public string _storeScene;
    [Scene][SerializeField] public string _inventoryScene;

    private bool _gamePaused = false;

    public void LoadMainGame()
    {
        SceneManager.LoadScene(_mainScene);
        _gamePaused = false;
    }

    public void LoadStore()
    {
        SceneManager.LoadScene(_storeScene);
    }

    public void LoadInventory()
    {
        SceneManager.LoadScene(_inventoryScene);
    }

    public void PauseGame()
    {
        _gamePausedChannel.Raise(_gamePaused);
        _gamePaused = !_gamePaused;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
