#pragma warning disable RECS0062 // Warns when a culture-aware 'LastIndexOf' call is used by default.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroyThisOnLoad))]
[RequireComponent(typeof(GameController))]
public class SceneLoader : MonoBehaviour
{
    public delegate void AsyncSceneReadyHandler(AsyncOperation transition, AsyncOperation nextScene);

    public event AsyncSceneReadyHandler OnSceneReady;

    public static SceneLoader ActiveLoader;

    public List<string> SceneNamesToIgnore;
    public string TransitionSceneName;
    public string NextSceneName;

    private AsyncOperation _transitionScene;
    private AsyncOperation _inactiveNextScene;

    private Scene _lastScene;
    private bool _waitingToPopScene;
    private List<string> _scenesInBuild = new List<string>();

    /// <summary>
    /// Activates the next Scene.
    /// </summary>
    public void ActivateNextScene()
    {
        _inactiveNextScene.allowSceneActivation = true;
    }

    /// <summary>
    /// Starts the transition to the next Scene.
    /// </summary>
    public void StartTransition()
    {
        if (!_waitingToPopScene)
        {
            Debug.LogError("TransitionNextScene() called before next scene was loaded! (_waitingToPopScene = false)");
            return;
        }

        _waitingToPopScene = false;

        _transitionScene.allowSceneActivation = true;
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
        SceneManager.sceneUnloaded += Event_SceneUnloaded;
        SceneManager.sceneLoaded += Event_SceneLoaded;
        OnSceneReady += Event_SceneReady;
        StartCoroutine(PrepareNextScene());
    }

    private void Event_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    /// <summary>
    /// loads the next scene and the coresponding transition scene in the background.
    /// </summary>
    public IEnumerator PrepareNextScene()
    {
        NextSceneName = GetNextScene();

        var transitionAsync = SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
        transitionAsync.allowSceneActivation = false;
        do
        {
            yield return null;
        } while (transitionAsync.progress < .89f); // magical number where scenes aren't fully loaded until they're active.
        Debug.Log($"Completed loading of: {TransitionSceneName}");
        var nextSceneAsync = SceneManager.LoadSceneAsync(NextSceneName, LoadSceneMode.Additive);
        nextSceneAsync.allowSceneActivation = false;
        do
        {
            yield return null;
        } while (nextSceneAsync.progress < .89f);
        Debug.Log($"Completed loading of: {NextSceneName}");

        // unity should get their shit together why is there even an "on scene loaded" event if its 100% identical to on scene active?
        OnSceneReady?.Invoke(transitionAsync, nextSceneAsync);
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
        var scene = _scenesInBuild[UnityEngine.Random.Range(0, _scenesInBuild.Count - 1)];

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

    private void Event_SceneReady(AsyncOperation transition, AsyncOperation nextScene)
    {
        _waitingToPopScene = true;
        Debug.Log($"Inactive scenes ready to pop.");
        _transitionScene = transition;
        _inactiveNextScene = nextScene;
    }

    private void Event_ActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        Debug.Log($"Switching to next active scene: {newScene.name} and unloading {oldScene.name}");

        // this is stupid
        foreach (var obj in oldScene.GetRootGameObjects())
        {
            Destroy(obj);
        }

        // all this does is remove the empty scene after an additional scenes have been activated.
        SceneManager.UnloadSceneAsync(oldScene);
    }
}