using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu ActiveMenu;
    public RectTransform LeftTransitionDoor;
    public RectTransform RightTransitionDoor;

    public void Awake()
    {
        if (ActiveMenu == null)
        {
            ActiveMenu = this;
        }
        else
            Destroy(this);
    }

    public void Start()
    {
        var gc = GameController.ActiveController;
        gc.OnTransitionStart += (s, e) => { StartCoroutine(CloseDoors()); };
        gc.OnTransitionEnd += (s, e) => { StartCoroutine(OpenDoors()); };
    }

    public IEnumerator CloseDoors()
    {
        yield return null;
    }

    public IEnumerator OpenDoors()
    {
        yield return null;
    }

    public void StartClicked()
    {
        GameController.ActiveController.StartGame();
    }
}