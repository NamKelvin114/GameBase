using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    [SerializeField] protected bool isUseAnimation;
    [ShowIf("isUseAnimation")] [SerializeField] private TypeAnimation typeAnimation;
    [ShowIf("isUseAnimation")]
    [ShowIf("typeAnimation", TypeAnimation.Move)] [SerializeField] private TypeAnimMove typeAnimMove;
    public Canvas CanvasPopup => GetComponent<Canvas>();
    public RectTransform rectTransform => CanvasPopup.GetComponent<RectTransform>();
    int _offset = 200;
    public bool isShowing { get; set; }
    public  void Show()
    {
        if (!isShowing)
        {
            CanvasPopup.overrideSorting = true;
            BeforeShow();
            Active(true);
            if (isUseAnimation)
            {
                switch (typeAnimation)
                {
                    case TypeAnimation.Move:
                        isShowing = true;
                        switch (typeAnimMove)
                        {
                            case TypeAnimMove.Down:
                                MoveDownAnimation();
                                break;
                            case TypeAnimMove.Up:
                                MoveUpAnimation();
                                break;
                            case TypeAnimMove.Right:
                                MoveRightAnimation();
                                break;
                            case TypeAnimMove.Left:
                                MoveLeftAnimation();
                                break;
                        }
                        break;
                    case TypeAnimation.Scale:
                        isShowing = true;
                        DoScaleOpen();
                        break;
                }
            }
            ShowContent();
        }
    }
    void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
        rectTransform.localPosition = Vector3.zero;
    }
    protected virtual void BeforeShow()
    {
        
    }
    protected virtual void ShowContent()
    {

    }
    protected virtual void BeforeHide()
    {

    }
    public virtual void Hide()
    {
        Debug.Log("Base");
        if (!isShowing)
        {
            BeforeHide();
            if (isUseAnimation)
            {
                switch (typeAnimation)
                {
                    case TypeAnimation.Move:
                        isShowing = true;
                        switch (typeAnimMove)
                        {
                            case TypeAnimMove.Down:
                                rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - (rectTransform.rect.height + _offset), rectTransform.localPosition.z), 1)
                                    .OnComplete((() =>
                                    {
                                        isShowing = false;
                                        Close();
                                    }));
                                break;
                            case TypeAnimMove.Up:
                                rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y + (rectTransform.rect.height + _offset), rectTransform.localPosition.z), 1)
                                    .OnComplete((() =>
                                    {
                                        isShowing = false;
                                        Close();
                                    }));
                                ;
                                break;
                            case TypeAnimMove.Right:
                                rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x + (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z), 1)
                                    .OnComplete((() =>
                                    {
                                        isShowing = false;
                                        Close();
                                    }));
                                ;
                                break;
                            case TypeAnimMove.Left:
                                rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x - (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z), 1)
                                    .OnComplete((() =>
                                    {
                                        isShowing = false;
                                        Close();
                                    }));
                                ;
                                break;
                        }
                        break;
                    case TypeAnimation.Scale:
                        Close();
                        break;
                }
            }
            else
            {
                Close();
            }
        }
    }
    void Close()
    {
        BeforeHide();
        gameObject.SetActive(false);
    }
    void DoScaleOpen()
    {
        var scaleDes = rectTransform.localScale;
        var scaleStart = scaleDes * 0.1f;
        rectTransform.localScale = scaleStart;
        rectTransform.DOScale(scaleDes, 1).OnComplete((() =>
        {
            isShowing = false;
        }));
    }
    void MoveDownAnimation()
    {
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, -(rectTransform.rect.height + _offset), rectTransform.localPosition.z);
        rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y + (rectTransform.rect.height + _offset), rectTransform.localPosition.z), 1)
            .OnComplete((() =>
            {
                isShowing = false;
            }));
    }
    void MoveUpAnimation()
    {
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.rect.height + _offset, rectTransform.localPosition.z);
        rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - (rectTransform.rect.height + _offset), rectTransform.localPosition.z), 1)
            .OnComplete((() =>
            {
                isShowing = false;
            }));
        ;
    }
    void MoveLeftAnimation()
    {
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z);
        rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x + (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z), 1)
            .OnComplete((() =>
            {
                isShowing = false;
            }));
        ;
    }
    void MoveRightAnimation()
    {
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z);
        rectTransform.DOLocalMove(new Vector3(rectTransform.localPosition.x - (rectTransform.rect.width + _offset), rectTransform.localPosition.y, rectTransform.localPosition.z), 1)
            .OnComplete((() =>
            {
                isShowing = false;
            }));
        ;
    }
}
public enum TypeAnimation
{
    Move,
    Scale,
}
public enum TypeAnimMove
{
    Up,
    Down,
    Left,
    Right,
}
