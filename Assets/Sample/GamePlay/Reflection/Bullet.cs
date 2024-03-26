using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Bullet : MonoBehaviour
{
    public List<Vector3> paths;
    private int _step = 1;
    private bool _ismove;
    [SerializeField] private Transform center;
    [SerializeField] float radiusForForce;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Rigidbody rg;
    private float _speed;
    public void Move(float getSpeed)
    {
        _ismove = true;
        _speed = getSpeed;
    }
    private void Update()
    {
        var pos = transform.forward * _speed * 2;
        rg.velocity = pos;
        // if (_ismove && _step < paths.Count)
        // {
        //     if (Vector3.Distance(transform.position, paths[_step]) > 0.1)
        //     {
        //         var pos = paths[_step] - transform.position;
        //         transform.forward = pos;
        //         transform.position = Vector3.MoveTowards(transform.position, paths[_step], 10 * Time.deltaTime);
        //     }
        //     else
        //     {
        //         _step += 1;
        //     }
        // }
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizos(radiusForForce);
    }
    void DrawGizos(float radius)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center.position, radius);
    }
    void ShootingRaycast()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        DoPush(center.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var reflec = Vector3.Reflect((collision.contacts[0].point - transform.position).normalized, collision.contacts[0].normal);
        var contact = collision.contacts[0].point;
        var dir = reflec + contact;
        Debug.Log(reflec);
        Debug.Log(contact);
        Debug.Log(dir);
        var pos = dir - collision.contacts[0].point;
        Debug.DrawLine(collision.contacts[0].point, reflec, Color.blue);
        transform.forward = new Vector3(pos.x, transform.localPosition.y, pos.z);
    }
    void DoPush(Vector3 center)
    {
        var cols = Physics.OverlapSphere(center, radiusForForce, layerMask.value);
        var receivers = cols.Where(n => n.gameObject.GetComponentInParent<Rigidbody>())
            .Select(c => c.gameObject.GetComponent<Rigidbody>())
            .Where(r => r != null)
            .Distinct()
            .ToList();
        foreach (var receiver in receivers)
        {
            var direction = receiver.transform.position - this.transform.position;
            receiver.AddForce(direction * 500);
        }
    }
}

