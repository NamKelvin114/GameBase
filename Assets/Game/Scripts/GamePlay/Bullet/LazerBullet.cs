using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LazerBullet : BulletInit
{
    private Transform center;
    private bool _isRun;
    private float _timeDamage = 0.5f;
    private float _currentTime;
    protected override void ReadyToPerform()
    {
        exploreFx.gameObject.SetActive(true);
        exploreFx.Play();
        _isRun = true;
        base.ReadyToPerform();
    }
    public void Init(Transform pos)
    {
        _currentTime = _timeDamage;
        center = pos;
        transform.position = center.position;
    }
    private void Update()
    {
        if (_isRun)
        {
            aliveTime -= Time.deltaTime;
            if (aliveTime <= 0)
            {
                DeActive();
                _isRun = false;
            }
            else
            {
                if (Target != null)
                {
                    transform.position = center.position;
                    var dir = Target.transform.position - center.position;
                    transform.forward = dir;
                }
                else
                {
                    DeActive();
                    _isRun = false;
                }

            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                var setTarget = other.GetComponent<ITarget>();
                if (setTarget != null)
                {
                    setTarget.GetDamage(DameIncome);
                    exploreFx.gameObject.SetActive(true);
                    exploreFx.Play();
                    _currentTime = _timeDamage;
                    // var time = exploreFx.main;
                    // SetActive(false);
                    // DOTween.Sequence().AppendInterval(time.duration).AppendCallback((() =>
                    // {
                    //     gameObject.SetActive(false);
                    // }));
                }
            }
        }
    }
}
