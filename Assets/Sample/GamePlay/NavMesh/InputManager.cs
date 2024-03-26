using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private RaycastHit _hit;
    public List<int> fingerIndex;
    public int fingerOnCamera;
    private int _lastFingerIndex;
    private bool _isTouchToCamera;
    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += UpdateFinger;
        LeanTouch.OnFingerUp += FingerUp;
        LeanTouch.OnFingerDown += FingerDown;
        Observer.CameraMove += TouchToCamera;
    }
    private void TouchToCamera()
    {
        _isTouchToCamera = true;
        fingerOnCamera = _lastFingerIndex;
    }
    private void FingerDown(LeanFinger finger)
    {
        Observer.FingerDown?.Invoke();
    }
    private void UpdateFinger(LeanFinger finger)
    {
        if (finger.Index != -42)
        {
            if (!fingerIndex.Contains(finger.Index))
            {
                fingerIndex.Add(finger.Index);
                _lastFingerIndex = finger.Index;
            }
            Debug.Log(finger.Index);
            Ray ray = cam.ScreenPointToRay(finger.ScreenPosition);
            if (_isTouchToCamera)
            {
                if (finger.Index == fingerOnCamera)
                {
                    return;
                }
                ShootRaycast(ray);
            }
            else
            {
                ShootRaycast(ray);
            }

        }
    }
    void ShootRaycast(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction, out _hit, 100))
        {
            if (_hit.transform.gameObject.GetComponent<CanvasRenderer>())
            {
                Debug.Log("canvas");
            }
            else
            {
                Debug.Log("cham");
            }
        }
    }
    private void FingerUp(LeanFinger finger)
    {
        for (int i = 0; i < fingerIndex.Count; i++)
        {
            if (fingerIndex[i] == finger.Index)
            {
                if (finger.Index == fingerOnCamera && _isTouchToCamera)
                {
                    Observer.FingerUp?.Invoke();
                    _isTouchToCamera = false;
                }
                fingerIndex.RemoveAt(i);
            }
        }
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= UpdateFinger;
        LeanTouch.OnFingerUp -= FingerUp;
        LeanTouch.OnFingerDown -= FingerDown;
        Observer.CameraMove -= TouchToCamera;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
