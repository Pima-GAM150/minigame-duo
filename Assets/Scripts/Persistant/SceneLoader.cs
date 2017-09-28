#pragma warning disable RECS0062 // Warns when a culture-aware 'LastIndexOf' call is used by default.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroyThisOnLoad))]
[RequireComponent(typeof(GameController))]
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader ActiveLoader;

    public List<string> SceneNamesToIgnore;
    public string TransitionSceneName;

    private Scene _transitionScene;

    private Scene _inactiveNextScene;
    private Scene _lastScene;
    private bool _waitingToPopScene;
    private List<string> _scenesInBuild = new List<string>();

    /// <summary>
    /// Activates the next Scene.
    /// </summary>
    public void ActivateNextScene()
    {
        SceneManager.SetActiveScene(_inactiveNextScene);
    }

    /// <summary>
    /// Starts the transition to the next Scene.
    /// </summary>
    public void StartTransition()
    {
        if (SceneManager.GetActiveScene() == _inactiveNextScene)
        {
            Debug.LogWarning($"StartTransition() called while _inactiveNextScene is the active scene! This probably isn't intentional!");
        }

        if (SceneManager.GetActiveScene() == _transitionScene)
        {
            Debug.LogError("TransitionNextScene() called while TransitionScene already active!");
            return;
        }

        if (!_waitingToPopScene)
        {
            Debug.LogError("TransitionNextScene() called before next scene was loaded! (_waitingToPopScene = false)");
            return;
        }

        _waitingToPopScene = false;

        SceneManager.SetActiveScene(_transitionScene);
    }

    public void Awake()
    {
        if (ActiveLoader == null)
        {
            ActiveLoader = this;
        }
        else
        {
            Debug.Log($"Destroying SceneLoader on {name}.");
            Destroy(this);
            return;
        }
    }

    /// <summary>
    /// Default Unity Method
    /// </summary>
    public void Start()
    {
        GetValidSceneNames();

        if (!SceneNamesToIgnore.Any())
        {
            Debug.LogError("SceneNamesToIgnore is empty!");
            return;
        }

        SceneManager.activeSceneChanged += Event_ActiveSceneChanged;
        SceneManager.sceneLoaded += Event_SceneLoaded;
        SceneManager.sceneUnloaded += Event_SceneUnloaded;
        GetNextScene();
    }

    /// <summary>
    /// loads the next scene and the coresponding transition scene in the background.
    /// </summary>
    private void LoadNextScene()
    {
        var nextSceneName = GetNextScene();

        var async = SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        async = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
    }

    /// <summary>
    /// returns a random scene name to use next.
    /// </summary>
    /// <returns>string</returns>
    private string GetNextScene()
    {
        var scount = SceneManager.sceneCountInBuildSettings;

        if (scount <= 1 || scount <= SceneNamesToIgnore.Count)
        {
            throw new UnassignedReferenceException("Not enough Scenes in build settings!");
        }
        var scene = _scenesInBuild[Random.Range(0, _scenesInBuild.Count - 1)];

        if (_lastScene.name == scene)
            return GetNextScene();

        return scene;
    }

    private void GetValidSceneNames()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);

            var sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);

            if (SceneNamesToIgnore.Contains(sceneName))
                continue;

            _scenesInBuild.Add(sceneName);
        }
    }

    private void Event_SceneUnloaded(Scene scene)
    {
        Debug.Log($"Unloaded scene: {scene.name}");
        _lastScene = scene;
    }

    private void Event_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Completed Scene Load on {scene.name}");
        if (mode == LoadSceneMode.Single)
        {
            Debug.Log("Scene loaded using LoadSceneMode.single");
            return;
        }
        if (scene != _transitionScene)
        {
            _waitingToPopScene = true;
            _inactiveNextScene = scene;
        }
        else _transitionScene = scene;
    }

    private void Event_ActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log($"Switching to next active scene: {newScene.name}");
        SceneManager.UnloadSceneAsync(oldScene);
        LoadNextScene();
    }
}