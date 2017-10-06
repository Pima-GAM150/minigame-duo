﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDoor : MonoBehaviour
{
    public enum DoorState
    {
        Open,
        Closed
    }
    public enum DoorDirection
    {
        Left,
        Right
    }
    public DoorState State;
    public DoorDirection Direction;

    [Range(0, 1)]
    public float ClosedByPercentage = 0.1f;
    [Range(0, 1)]
    public float OpenAtPercentage = 0.9f;

    private RectTransform _rectTransform => transform as RectTransform;

    private Vector2 _dir = Vector2.left;
    private Vector2 _closedPos;
    private Vector2 _openPos;

    public void Start()
    {
        var gc = GameController.ActiveController;
        _closedPos = _rectTransform.anchoredPosition;
        switch (Direction)
        {
            case DoorDirection.Left:
                _dir = Vector2.left;
                break;

            case DoorDirection.Right:
                _dir = Vector2.right;
                break;
        }
        State = DoorState.Open;

        // set door to open as the default state.
        _openPos = Vector2.zero;
        _rectTransform.anchoredPosition = _openPos;
        gc.OnTransitionTimerTick += Event_TransitionTimerTick;
    }

    private void Event_TransitionTimerTick(float current, float max)
    {
        var percent = current / max;

        if (percent <= ClosedByPercentage)
        {
            percent /= ClosedByPercentage;
            LerpDoor(_closedPos, percent);
        }
        else if (percent >= OpenAtPercentage)
        {
            percent -= OpenAtPercentage;

            LerpDoor(_openPos, percent / (1 - OpenAtPercentage));
        }
    }

    private void LerpDoor(Vector2 targetPos, float t)
    {
        var pos = Vector2.Lerp(_rectTransform.anchoredPosition, targetPos, t);
        _rectTransform.anchoredPosition = pos;
    }
}