using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    private bool _isFingerDown;
    private bool _isFingerUp;
    private bool _isFingerDrag;
    public Camera cam;
    private Item _select;
    private  GameObject selectedItem;
    private void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerDown += OnFingleDown;
        Lean.Touch.LeanTouch.OnFingerUpdate += OnFingerDrag;
        Lean.Touch.LeanTouch.OnFingerUp += OnFingerUp;
    }
    private void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerDown -= OnFingleDown;
        Lean.Touch.LeanTouch.OnFingerUpdate -= OnFingerDrag;
        Lean.Touch.LeanTouch.OnFingerUp -= OnFingerUp;
    }

    void OnFingerDrag(Lean.Touch.LeanFinger finger)
    {
        
        if (_isFingerDrag)
        {
            var ray = finger.GetRay(cam);
            var hit = default(RaycastHit);
            var pos = new Vector3(ray.direction.x, ray.direction.y + 0.1f, ray.direction.z);
            Debug.DrawRay(ray.origin, pos * 10, Color.yellow);
            if (Physics.Raycast(ray.origin, pos, out hit, float.PositiveInfinity))
            {
                if (hit.collider.gameObject.CompareTag(Constant.Floor))
                {
                    CheckSlot(hit.collider.gameObject.GetComponent<ItemSlot>(), ETypeItem.Lie);
                    MoveItem(hit.collider.gameObject.transform);
                }
                if (hit.collider.gameObject.CompareTag(Constant.Zone))
                {
                    var h = hit.point;
                    var getPoint = new Vector3(h.x, h.y, h.z);
                    _select.Move(getPoint);
                    _select.SetSelect(EItemState.unselected);
                }
                if (hit.collider.gameObject.CompareTag(Constant.Wall))
                {

                    CheckSlot(hit.collider.gameObject.GetComponent<ItemSlot>(), ETypeItem.Hang);
                    MoveItem(hit.collider.gameObject.transform);
                }
            }
        }
    }
    void CheckSlot(ItemSlot Obj, ETypeItem eTypeItem)
    {
        var parent = Obj.GetComponentInParent<Surface>();
        var maxLength = Convert.ToInt32(Mathf.Ceil(_select.transform.lossyScale.x / Obj.transform.lossyScale.x));
        var maxHeight = Convert.ToInt32(Mathf.Ceil(_select.transform.lossyScale.y / Obj.transform.lossyScale.z));
        if (parent == null) return;
        if (parent.SlotCheck(Obj, maxHeight, maxLength))
        {
            if (_select.eTypeItem == ETypeItem.HangandLie)
            {
                _select.SetSelect(EItemState.selected);
                switch (eTypeItem)
                {
                    case ETypeItem.Hang:
                        _select.RotateFlex(ETypeItem.Hang);
                        break;
                    case ETypeItem.Lie:
                        _select.RotateFlex(ETypeItem.Lie);
                        break;
                }
            }
            else
            {
                if (eTypeItem == ETypeItem.Hang)
                {
                    _select.isHang = true;
                    SetSelectItem(eTypeItem);
                }
                if (eTypeItem == ETypeItem.Lie)
                {
                    _select.isHang = false;
                    SetSelectItem(eTypeItem);
                }
            }
        }
        else
        {
            Debug.Log("cant");
            _select.SetSelect(EItemState.unselected);
        }
    }
    void SetSelectItem(ETypeItem eTypeItem)
    {
        if (_select.eTypeItem == eTypeItem)
        {
            _select.SetSelect(EItemState.selected);
        }
        else
        {
            _select.SetSelect(EItemState.unselected);
        }
    }
    void MoveItem(Transform hit)
    {
        var getPoint = hit;
        _select.Move(getPoint.position);
    }
    void OnFingerUp(Lean.Touch.LeanFinger finger)
    {
        _isFingerDrag = false;
        _select = null;
        Observer.FingerUp?.Invoke();
    }

    void OnFingleDown(Lean.Touch.LeanFinger finger)
    {
        if (!finger.IsOverGui)
        {
            _isFingerDown = true;
            var ray = finger.GetRay(cam);
            var hit = default(RaycastHit);
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (hit.collider.gameObject.CompareTag(Constant.Item))
                {
                    _isFingerDrag = true;
                    _select = hit.collider.gameObject.GetComponentInParent<Item>();
                    _select.transform.position = new Vector3(_select.transform.position.x, _select.transform.position.y + 0.1f, _select.transform.position.z - 0.1f);
                }
                else if (hit.collider.gameObject.CompareTag("RotateItem"))
                {
                    _isFingerDrag = true;
                    selectedItem = hit.collider.gameObject;
                }
            }
        }
    }
}
