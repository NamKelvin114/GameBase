using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Material selectMat;
    [SerializeField] private Material disSelectMat;
    [SerializeField] private MeshRenderer itemMesh;
    private Transform _previousPosi;
    private Transform _updatePosi;
    [SerializeField] private Vector3 currentPosi;
    [SerializeField] EItemState setEItemState;
    public ETypeItem eTypeItem;
    public bool isHang { get; set; }
    // Start is called before the first frame update
    private void OnEnable()
    {
        Observer.FingerUp += Checkposi;
    }
    private void OnDisable()
    {
        Observer.FingerUp -= Checkposi;
    }
    void Start()
    {
        if (eTypeItem == ETypeItem.Hang)
        {
            isHang = true;
        }
        else
        {
            isHang = false;
        }
        currentPosi = transform.position;
    }
    public void SetSelect(EItemState eItemState)
    {
        switch (eItemState)
        {
            case EItemState.selected:
                SetMatSelect(selectMat, eItemState);
                break;
            case EItemState.unselected:
                SetMatSelect(disSelectMat, eItemState);
                break;
        }
    }
    public void RotateFlex(ETypeItem eTypeItem)
    {
        if (eTypeItem == ETypeItem.Hang)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isHang = true;
        }
        else if (eTypeItem == ETypeItem.Lie)
        {
            transform.eulerAngles = new Vector3(90, 0, 0);
            isHang = false;
        }
    }
    void Checkposi()
    {
        if (setEItemState != EItemState.unselected)
        {
            currentPosi = transform.position;
        }
        else
        {
            transform.DOMove(currentPosi, 0.2f).OnComplete((() =>
            {
                SetMatSelect(selectMat, EItemState.selected);
            }));
        }
    }
    void SetMatSelect(Material setMat, EItemState eItemState)
    {
        itemMesh.material = setMat;
        setEItemState = eItemState;
    }
    public void Move(Vector3 movePosi)
    {
        Vector3 endPosi;
        if (eTypeItem == ETypeItem.HangandLie)
        {
            if (isHang)
            {
                endPosi = new Vector3(movePosi.x, movePosi.y, movePosi.z - (transform.lossyScale.z / 2));
            }
            else
            {
                endPosi = new Vector3(movePosi.x, movePosi.y + (transform.lossyScale.z / 2), movePosi.z);
            }
        }
        else
        {
            if (isHang)
            {
                endPosi = new Vector3(movePosi.x, movePosi.y, movePosi.z - (transform.lossyScale.z / 2));
            }
            else
            {
                endPosi = new Vector3(movePosi.x, movePosi.y + (transform.lossyScale.y / 2), movePosi.z);
            }
        }
        transform.DOMove(endPosi, 0.2f);
    }
}
public enum EItemState
{
    selected,
    unselected,
}
public enum ETypeItem
{
    Hang,
    Lie,
    HangandLie,
    NoWhere,
}
