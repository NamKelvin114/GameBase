using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    #region Settings
    public static bool IsMuteSoundBG
    {
        get => GetBool(Constant.IsMuteSoundBg);
        set => SetBool(Constant.IsMuteSoundBg, value);
    }
    public static bool IsMuteSoundOneShot
    {
        get => GetBool(Constant.IsMuteSoundOneShot);
        set => SetBool(Constant.IsMuteSoundOneShot, value);
    }
    #endregion
    #region LoadCondition
    public static string SceneToLoad;
    public static bool IsTransforLoadingScene = false;
    #endregion
    #region GamePlay
    public static int CurrentLevel
    {
        get => GetInt(Constant.Level, 1);
        set => SetInt(Constant.Level, value);
    }
    #endregion
    #region Task
    public static string ListCurrentTask
    {
        get => GetString(Constant.ListCurrentTask, "");
        set => SetString(Constant.ListCurrentTask, value);
    }
    public static void SetTaskProcess(ETypeTask eTypeTask, int process)
    {
        SetInt(eTypeTask.ToString(), process);
    }
    public static int GetTaskProcess(ETypeTask eTypeTask)
    {
        return GetInt(eTypeTask.ToString(), 0);
    }
    public static int MaxCurrentTask
    {
        get => GetInt(Constant.MaxCurrentTask, 0);
        set => SetInt(Constant.MaxCurrentTask, value);
    }
    #endregion
    #region Resource
    public static int CurrentCoint
    {
        get => GetInt(Constant.Money, 0);
        set => SetInt(Constant.Money, value);
    }
    public static void IncreaseMoney(int valueToIncrease, Vector3 position)
    {
        var getPopupMoney = PopupManager.Instance.GetPopup<PopupMoney>();
        if (getPopupMoney.isActiveAndEnabled)
        {
            Observer.IncreaseCoin?.Invoke(valueToIncrease, position);
        }
        else
        {
            CurrentCoint += valueToIncrease;
        }
    }
    #endregion

    #region CustomValue
    private static bool GetBool(string key, bool defaultValue = false) =>
        PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) > 0;

    private static void SetBool(string id, bool value) => PlayerPrefs.SetInt(id, value ? 1 : 0);

    private static int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    private static void SetInt(string id, int value) => PlayerPrefs.SetInt(id, value);

    private static string GetString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
    private static void SetString(string id, string value) => PlayerPrefs.SetString(id, value);
    #endregion
}
