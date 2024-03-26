using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private RaycastHit _raycastHit;
    public int numberofRays = 1;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Bullet bullet;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Transform direc;
    private float _maxPower;
    private bool _isTouch;
    private bool _isSpawn;
    private int _index = 0;
    private string a;
    private void OnEnable()
    { 
        Debug.Log(a);
        LeanTouch.OnFingerUpdate += UpdateFinger;
        LeanTouch.OnFingerUp += FingerUp;
        LeanTouch.OnFingerDown += FingerDown;
    }
    

private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= UpdateFinger;
        LeanTouch.OnFingerUp -= FingerUp;
        LeanTouch.OnFingerDown -= FingerDown;
    }
    private void FingerDown(LeanFinger finger)
    {
        _isTouch = true;
        _isSpawn = true;
    }
    private void FingerUp(LeanFinger finger)
    {

        _isTouch = false;
        Spawn();
    }
    void Spawn()
    {
        if (_isSpawn)
        {
            _isSpawn = false;
            var spawnBullet = Instantiate(bullet, transform);
            spawnBullet.transform.parent = transform.root;
            // for (int i = 0; i < lineRenderer.positionCount; i++)
            // {
            //     spawnBullet.paths.Add(lineRenderer.GetPosition(i));
            // }
            spawnBullet.Move(_maxPower);
        }
    }
    private void UpdateFinger(LeanFinger finger)
    {
        if (_isTouch)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = new Vector3(mousePos.x, transform.position.y, mousePos.z) - transform.position;
            _maxPower = Mathf.Clamp(pos.z, -4, 4);
            direc.localPosition = new Vector3(direc.localPosition.x, direc.localPosition.y, Mathf.Abs(_maxPower));
            lineRenderer.SetPosition(0, transform.position);
            transform.forward = pos;
            lineRenderer.SetPosition(1, direc.position);
        }
    }

    void Update()
    {
        Shooting(transform.position, transform.forward);
    }
    void Shooting(Vector3 start, Vector3 from)
    {
        //Use Reflection:---------------------------------------------------------------
        // numberofRays = 1;
        // lineRenderer.positionCount = numberofRays;
        // lineRenderer.SetPosition(0, start);
        // for (int i = 0; i < numberofRays; i++)
        // {
        //     var ray = new Ray(start, from);
        //     if (Physics.Raycast(ray, out _raycastHit, 100, _mask))
        //     {
        //         numberofRays += 1;
        //         lineRenderer.positionCount += 1;
        //         lineRenderer.SetPosition(i + 1, _raycastHit.point);
        //         Debug.DrawLine(start, _raycastHit.point, Color.blue);
        //         start = _raycastHit.point;
        //         from = Vector3.Reflect(start, _raycastHit.normal);
        //     }
        //     else
        //     {
        //         lineRenderer.positionCount += 1;
        //         lineRenderer.SetPosition(i + 1, from * 100);
        //         Debug.DrawRay(start, from * 100, Color.blue);
        //     }
        // }
    }
}

