using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroyThisOnLoad))]
[RequireComponent(typeof(SceneLoader))]
public class GameController : MonoBehaviour
{
    public delegate void TimerTick(float value);

    public event TimerTick OnGameTimerTick;

    public static GameController ActiveController;

    [Space(20)]
    public float TransitionTime = 5f;

    public float MinimumTransitionTime = 0.5f;

    [Space(20)]
    public float GameTime = 10f;

    public float MinimumGameTime = 2f;

    [Space(20)]
    public bool WasSuccessful;

    private float _transitionTime;
    private float _gameTime;

    public void StartGame()
    {
        _transitionTime = TransitionTime;
        _gameTime = GameTime;
        StartCoroutine(LoadNextGame());
    }

    public void Awake()
    {
        if (ActiveController == null)
        {
            ActiveController = this;
        }
        else
        {
            Debug.Log($"Destroying GameController on {name}.");
            Destroy(this);
            return;
        }
    }

    private IEnumerator LoadNextGame()
    {
        do
        {
            var sloader = SceneLoader.ActiveLoader;
            sloader.StartTransition();
            var counter = 0f;

            // wait for game timer
            while (counter <= GameTime)
            {
                counter += Time.deltaTime;
                OnGameTimerTick?.Invoke(counter);
                yield return null;
            }
            TransitionTime = Mathf.Clamp(TransitionTime, MinimumTransitionTime, TransitionTime);
            GameTime = Mathf.Clamp(GameTime, MinimumGameTime, GameTime);
        }
        while (WasSuccessful);

        // this means we failed.
    }
}