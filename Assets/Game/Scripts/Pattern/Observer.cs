using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Observer
{
    #region GamePLay
    public static Action FingerUp;
    public static Action FingerDown;
    public static Action CameraMove;
    public static Action CameraShake;
    public static Action<float, bool> UpdatePlayerHealth;
    public static Action<float, bool> UpdatePlayerEnergy;
    public static Action CheckArea;
    public static Action CheckWinLevel;
    #endregion
    #region Loading
    public static Action<string> DoTransition;
    #endregion
    #region Task
    public static Action<SubTask> Claim;
    #endregion
    #region Resource
    public static Action<int, Vector3> IncreaseCoin;
    public static Action<bool> TestAction;
    public static Predicate<string> TestPredicate;
    #endregion
}
