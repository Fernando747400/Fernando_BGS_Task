using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Obvious.Soap;
using System.Collections;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "ShoopingSpree/SceneLoader", order = 1)]
public class SceneLoader : ScriptableObject
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventBool _gamePausedChannel;
    [Required][SerializeField] private ScriptableEventNoParam _mainSceneUnloadedChannel;
    [Required][SerializeField] private ScriptableEventNoParam _mainSceneLoadedChannel;

    [Header("Settings")]
    [Scene][SerializeField] private string _mainScene;
    [Scene][SerializeField] private string _storeScene;
    [Scene][SerializeField] private string _inventoryScene;

    public void LoadMainGame()
    {     _gamePausedChannel.Raise(false);
    }

    public void LoadStore(MonoBehaviour caller)
    {
       caller.StartCoroutine(LoadSceneAsync(_storeScene));
    }

    public void LoadInventory(MonoBehaviour caller)
    {
        caller.StartCoroutine(LoadSceneAsync(_inventoryScene));
    }

    public void UnloadStore(MonoBehaviour caller)
    {
        caller.StartCoroutine(UnloadSceneAsync(_storeScene));
    }

    public void UnloadInventory(MonoBehaviour caller)
    {
        caller.StartCoroutine(UnloadSceneAsync(_inventoryScene));
    }

    private IEnumerator LoadSceneAsync(string Scene)
    {
        _gamePausedChannel.Raise(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        { 
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scene));
        _mainSceneUnloadedChannel.Raise();
    }

    private IEnumerator UnloadSceneAsync(string scene)
    {
        _mainSceneLoadedChannel.Raise();
        _gamePausedChannel.Raise(false);

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_mainScene));

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
