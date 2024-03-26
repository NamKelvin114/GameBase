using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pancake;
using UnityEngine;

public class SlashBullet : BulletInit
{
    [SerializeField, Range(0, 10)] private float radiusExplosion;
    [SerializeField] private LayerMask targetLayerMask;
    [ReadOnly] public List<GameObject> listTarget;
    protected override void ReadyToPerform()
    {
        listTarget.Clear();
        exploreFx.gameObject.SetActive(true);
        exploreFx.Play();
        base.ReadyToPerform();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusExplosion);
    }
    private void Update()
    {
        bulletRig.velocity = transform.forward * speed;
        var targets = Physics.OverlapSphere(transform.position, radiusExplosion, targetLayerMask).ToList();
        var receivers = targets.Select(c => c.GetComponent<Transform>()).Distinct().ToList();
        var getReceivers = receivers.Where(g => !listTarget.Contains(g.gameObject)).Distinct().ToList();
        foreach (var checkReceiver in getReceivers)
        {
            checkReceiver.GetComponentInParent<ITarget>().GetDamage(DameIncome);
            listTarget.Add(checkReceiver.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.Wall))
        {
            SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
