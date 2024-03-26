using System.Collections;
using System.Collections.Generic;
using Pancake;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void ShowPopupSkinShop()
    {
        PopupManager.Instance.ShowPopup<PopupSkinShop>();
    }
    public void ShowPopupTask()
    {
        PopupManager.Instance.ShowPopup<PopupTask>();
    }
    public void StartGame()
    {
        Load.DoTransitionLoading(Constant.GamePlayScene);
    }
    public void ShowPopupLucky()
    {
        PopupManager.Instance.ShowPopup<PopupLucky>();
    }

}
