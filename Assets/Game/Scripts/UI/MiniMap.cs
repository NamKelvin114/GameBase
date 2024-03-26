using System;
using System.Collections;
using System.Collections.Generic;
using Pancake;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Camera cameraMiniMap;
    private GameObject _target;
    [ReadOnly] public bool isOpen;
    private Vector3 _openCamPos;
    private float _miniCamPos;
    private Vector3 _miniMapPos;
    private Vector3 _openMapPos;
    private float _widthMiniMap;
    private float _heightMiniMap;
    private Vector2 _anchorMin;
    private RectTransform _rectTransform;
    public void OnOpenMiniMap()
    {
        isOpen = !isOpen;
        SetOpenMiniMap();
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _target = GameObject.FindGameObjectWithTag(Constant.Player);
        _openCamPos = cameraMiniMap.transform.position;
        _miniCamPos = _openCamPos.y / 5;
        _miniMapPos = _rectTransform.anchoredPosition3D;
        //Debug.Log(_miniMapPos);
        _widthMiniMap = _rectTransform.rect.width;
        _anchorMin = _rectTransform.anchorMin;
        _heightMiniMap = _rectTransform.rect.height;
    }
    void SetOpenMiniMap()
    {
        if (isOpen)
        {
            GameManager.Instance.PauseGame(true);
            Debug.Log("vao");
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.localPosition = Vector3.zero;
            _rectTransform.sizeDelta = Vector2.zero;
        }
        else
        {
            GameManager.Instance.PauseGame(false);
            Debug.Log("ra");
            _rectTransform.anchoredPosition3D = _miniMapPos;
            _rectTransform.sizeDelta = new Vector2(_widthMiniMap, _heightMiniMap);
            _rectTransform.anchorMin = _anchorMin;
        }
    }
    private void LateUpdate()
    {
        if (!isOpen)
        {
            cameraMiniMap.transform.position = new Vector3(_target.transform.position.x, _miniCamPos, _target.transform.position.z);
        }
        else
        {
            cameraMiniMap.transform.position = _openCamPos;
        }
    }

}
