using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private MeshCollider _meshCollider;
    private Vector2 _sizeCam;
    private Vector3 _sizeMap;
    private Vector3 _currentPosi;
    private Vector3 _prevPosi;
    private bool _isMoveCam;
    private void Start()
    {
        _sizeMap = _meshCollider.bounds.size;
        _sizeCam = gameObject.GetComponent<Camera>().sensorSize;
        _currentPosi = transform.position;
        _prevPosi = _currentPosi;
    }
    private void OnEnable()
    {
        Observer.FingerUp += FingerUp;
        Observer.FingerDown += FingerDown;
    }
    private void FingerDown()
    {
       
    }
    private void OnDisable()
    {
        Observer.FingerUp -= FingerUp;
        Observer.FingerDown -= FingerDown;
    }
    void FingerUp()
    {
        _isMoveCam = false;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + joystick.Horizontal * Time.deltaTime * 8, -_sizeMap.z / 2 + _sizeCam.y, _sizeMap.z / 2 - _sizeCam.y), transform.position.y,
            Mathf.Clamp(transform.position.z + joystick.Vertical * Time.deltaTime * 8, -_sizeMap.x / 2 + _sizeCam.x, _sizeMap.x / 2 - _sizeCam.x));
        _currentPosi = transform.position;
        if (_prevPosi != _currentPosi)
        {
            _prevPosi = _currentPosi;
            if (!_isMoveCam)
            {
                Observer.CameraMove?.Invoke();
            }
            _isMoveCam = true;
        }
    }
}
