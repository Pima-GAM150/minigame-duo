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

    public void StartClicked()
    {
        GameController.ActiveController.StartGame();
    }
}