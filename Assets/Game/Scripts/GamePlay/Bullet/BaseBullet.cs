using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BaseBullet : BulletInit
{
    private void Update()
    {
        if (isMove)
        {
            if (Target != null)
            {
                if (isFollowTarget)
                {
                    if (Vector3.Distance(transform.position, Target.transform.position) > 0)
                    {
                        var distance = Target.transform.position - transform.position;
                        transform.forward = distance;
                        bulletRig.velocity = transform.forward * speed;
                    }
                }
                else
                {
                    bulletRig.velocity = transform.forward * speed;
                }
            }
            else
            {
                SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else
        {
            bulletRig.velocity = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsUsing)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                var setTarget = Target.GetComponentInParent<ITarget>();
                if (setTarget != null)
                {
                    setTarget.GetDamage(DameIncome);
                    exploreFx.gameObject.SetActive(true);
                    exploreFx.Play();
                    var time = exploreFx.main;
                    SetActive(false);
                    BulletEffect();
                    DOTween.Sequence().AppendInterval(time.duration).AppendCallback((() =>
                    {
                        gameObject.SetActive(false);
                    }));
                }
            }
            if (other.gameObject.CompareTag(Constant.Wall))
            {
                DeActive();
            }
        }
    }
    protected abstract void BulletEffect();
}
