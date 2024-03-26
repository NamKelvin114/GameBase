using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchManager : MonoBehaviour
{
    private bool _isFingerDown;
    private bool _isFingerUp;
    private bool _isFingerDrag;
    [SerializeField] private GameObject trail;
    Camera _cam;
    private void OnEnable()
    {
        _cam=Camera.main;
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
           
            trail.gameObject.SetActive(true);
            var pos = finger.GetWorldPosition(-_cam.transform.position.z, _cam);
            trail.transform.position = pos;
        }
    }

    void OnFingerUp(Lean.Touch.LeanFinger finger)
    {
        _isFingerDrag = false;

    }

    void OnFingleDown(Lean.Touch.LeanFinger finger)
    {
        if (!finger.IsOverGui)
        {
            trail.gameObject.SetActive(false);
            _isFingerDrag = true;
        }
    }
}
