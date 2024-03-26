using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private NavMeshAgent playerNav;
    private RaycastHit hit;
    private bool _isMove;
    private Vector3 _destination;
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
    }
    private void OnFingerDown(LeanFinger finger)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))
        {
            _isMove = true;
            _destination = hit.point;
        }
    }
    private void Update()
    {
        if (_isMove)
        {
            playerNav.SetDestination(_destination);
        }
    }
}
