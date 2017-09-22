#pragma warning disable RECS0062 // Warns when a culture-aware 'LastIndexOf' call is used by default.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void StartTrannsition()
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

    /// <summary>
    /// loads the next scene and the coresponding transition scene in the background.
    /// </summary>
    private void LoadNextScene()
    {
        var cachedScene = GetNextScene();

        var async = SceneManager.LoadSceneAsync(_transitionScene.name, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        async = SceneManager.LoadSceneAsync(cachedScene.name, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
    }

    /// <summary>
    /// returns a random scene to use next.
    /// </summary>
    /// <returns>Scene</returns>
    private Scene GetNextScene()
    {
        var scount = SceneManager.sceneCountInBuildSettings;

        if (scount <= 1 || scount <= SceneNamesToIgnore.Count)
        {
            throw new UnassignedReferenceException("Not enough Scenes in build settings!");
        }
        var scene = SceneManager.GetSceneByBuildIndex(Random.Range(0, scount - 1));

        if (SceneNamesToIgnore.Contains(scene.name) || _lastScene == scene)
            return GetNextScene();
        else return scene;
    }

    /// <summary>
    /// Default Unity Method
    /// </summary>
    public void Start()
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

        DontDestroyOnLoad(gameObject);

        GetValidSceneNames();

        if (!SceneNamesToIgnore.Any())
        {
            Debug.LogError("SceneNamesToIgnore is empty!");
            return;
        }

        _transitionScene = SceneManager.GetSceneByName(TransitionSceneName);
        if (!_transitionScene.IsValid())
        {
            Debug.LogError("_transitionScene is not valid!");
            return;
        }

        SceneManager.activeSceneChanged += Event_ActiveSceneChanged;
        SceneManager.sceneLoaded += Event_SceneLoaded;
        SceneManager.sceneUnloaded += Event_SceneUnloaded;
        GetNextScene();
    }

    private void GetValidSceneNames()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            int lastSlash = scenePath.LastIndexOf("/");
            _scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }
    }

    private void Event_SceneUnloaded(Scene scene)
    {
        _lastScene = scene;
    }

    private void Event_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
        SceneManager.UnloadSceneAsync(oldScene);
        LoadNextScene();
    }
}