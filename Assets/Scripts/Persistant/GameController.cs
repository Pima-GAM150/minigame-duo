using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroyThisOnLoad))]
[RequireComponent(typeof(SceneLoader))]
public class GameController : MonoBehaviour
{
    public delegate void TimerTick(float current, float max);

    public event TimerTick OnGameTimerTick;

    public event TimerTick OnTransitionTimerTick;

    public event EventHandler OnTransitionStart;

    public event EventHandler OnTransitionEnd;

    public event EventHandler OnGameStart;

    public event EventHandler OnGameEnd;

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
        TransitionTime = _transitionTime;
        GameTime = _gameTime;
        StartCoroutine(LoadNextGame());
    }

    public void Awake()
    {
        if (ActiveController == null)
        {
            ActiveController = this;
            _transitionTime = TransitionTime;
            _gameTime = GameTime;
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
        do // while WasSuccessful
        {
            WasSuccessful = false;
            var sloader = SceneLoader.ActiveLoader;
            sloader.StartTransition();
            var counter = 0f;

            // wait for transition
            OnTransitionStart?.Invoke(this, EventArgs.Empty);
            while (counter <= TransitionTime)
            {
                counter += Time.deltaTime;
                OnTransitionTimerTick?.Invoke(counter, TransitionTime);
                yield return null;
            }
            OnTransitionEnd?.Invoke(this, EventArgs.Empty);

            counter = 0f;
            sloader.ActivateNextScene();

            // wait for game timer
            OnGameStart?.Invoke(this, EventArgs.Empty);
            while (counter <= GameTime)
            {
                counter += Time.deltaTime;
                OnGameTimerTick?.Invoke(counter, GameTime);
                yield return null;
            }
            TransitionTime = Mathf.Clamp(TransitionTime, MinimumTransitionTime, TransitionTime);
            GameTime = Mathf.Clamp(GameTime, MinimumGameTime, GameTime);
            OnGameEnd?.Invoke(this, EventArgs.Empty);
        }
        while (WasSuccessful);

        // this means we failed.
    }
}