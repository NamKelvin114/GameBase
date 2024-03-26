using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInit : MonoBehaviour, IBullet
{
    [SerializeField] protected EBulletType eBulletType;
    [SerializeField] protected float speed;
    [SerializeField] protected ParticleSystem exploreFx;
    [SerializeField] protected GameObject model;
    [SerializeField] protected BoxCollider collider;
    [SerializeField] protected bool isFollowTarget;
    protected float aliveTime;
    protected Rigidbody bulletRig;
    protected string targetTag;
    protected bool isMove;
    private void Start()
    {
        bulletRig = gameObject.GetComponent<Rigidbody>();
    }
    public EBulletType BulletType
    {
        get => eBulletType;
        set => eBulletType = value;
    }
    public GameObject Target
    {
        get;
        set;
    }
    public float DameIncome
    {
        get;
        set;
    }
    public SpecialStats SpecialStats
    {
        get;
        set;
    }
    public bool IsUsing
    {
        get;
        set;
    }
    public void HitTarget(ref GameObject target, float damageIncome, string targetTag, SpecialStats getSpecialStats, float getTimeAlive)
    {
        aliveTime = getTimeAlive;
        SpecialStats = getSpecialStats;
        this.targetTag = targetTag;
        Target = target;
        DameIncome = damageIncome;
        SetActive(true);
        exploreFx.Stop();
        exploreFx.gameObject.SetActive(false);
        var distance = Target.transform.position - transform.position;
        transform.forward = distance;
        ReadyToPerform();
    }
    protected virtual void ReadyToPerform()
    {
        
    }
    protected void SetActive(bool isActive)
    {
        isMove = isActive;
        collider.enabled = isActive;
        model.gameObject.SetActive(isActive);
        IsUsing = isActive;
    }
    public void DeActive()
    {
        SetActive(false);
        gameObject.SetActive(false);
    }
}
