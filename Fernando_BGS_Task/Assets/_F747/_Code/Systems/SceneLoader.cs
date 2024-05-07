using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Obvious.Soap;
using System.Collections;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "ShoopingSpree/SceneLoader", order = 1)]
public class SceneLoader : ScriptableObject
{
    [Header("Dependencies")]
    [Required]
    [SerializeField] private ScriptableEventBool _gamePausedChannel;

    [Header("Settings")]
    [Scene][SerializeField] private string _mainScene;
    [Scene][SerializeField] private string _storeScene;
    [Scene][SerializeField] private string _inventoryScene;

    private bool _gamePaused = false;

    public void LoadMainGame()
    {
        SceneManager.LoadScene(_mainScene);
        _gamePaused = false;
    }

    public void LoadStore(MonoBehaviour caller)
    {
       caller.StartCoroutine(LoadSceneAsync(_storeScene));
    }

    public void LoadInventory()
    {
        SceneManager.LoadScene(_inventoryScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_inventoryScene));
    }

    public void PauseGame()
    {
        _gamePausedChannel.Raise(_gamePaused);
        _gamePaused = !_gamePaused;
    }

    private IEnumerator LoadSceneAsync(string Scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Scene);

        while (!asyncLoad.isDone)
        { 
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scene));
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
