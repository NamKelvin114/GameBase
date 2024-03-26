using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public abstract class BaseEnemy : BaseAttack, ITarget, ISlow
{
    [SerializeField] private Image healthFillBar;
    [SerializeField] protected float moveSpeed;
    [SerializeField, Range(1, 100)] protected float rangeAttack;
    [SerializeField] protected SkillGroup skillGroup;
    [SerializeField] protected List<GemItem> gemItems;
    protected GameObject playerTarget;
    [SerializeField] protected Rigidbody enemyRigid;
    protected float maxHp;
    protected float maxMoveSpeed;
    protected float maxShield;
    protected Sequence getslowSequence;
    private bool _isStart;
    public void Init(EnemyStats enemyStats)
    {
        _isStart = true;
        HP = enemyStats.MaxHP;
        Shield = enemyStats.MaxShield;
        moveSpeed = enemyStats.moveSpeed;
        maxHp = HP;
        maxShield = Shield;
        maxMoveSpeed = moveSpeed;
        healthFillBar.fillAmount = maxHp / HP;
        IsDead = false;
        StartFinding();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }
    public void StartFinding()
    {
        var listPlayer = GameObject.FindGameObjectsWithTag(Constant.Player).ToList();
        playerTarget = listPlayer.Find(p => p.GetComponent<MainPlayer>());
        target = playerTarget;
        center = transform;
    }
    private void Update()
    {
        if (!IsDead && _isStart)
        {
            if (playerTarget != null)
            {
                var dir = playerTarget.transform.position - transform.position;
                var getpos = new Vector3(dir.x, transform.position.y, dir.z);
                if (Vector3.Distance(transform.position, playerTarget.transform.position) > rangeAttack)
                {
                    enemyRigid.velocity = getpos * moveSpeed;
                }
                else
                {
                    enemyRigid.velocity = Vector3.zero;
                    if (currentSkill==null)
                    {
                        InitToAttack(skillGroup.skillDatas[0].skill);
                    }
                    CalCoolDown();
                }
                transform.forward = getpos;
            }
        }
    }
    public float HP
    {
        get;
        set;
    }
    public float Energy
    {
        get;
        set;
    }
    public float Shield
    {
        get;
        set;
    }
    public bool IsDead
    {
        get;
        set;
    }
    public void GetDamage(float damage)
    {
        if (!IsDead && _isStart)
        {
            HP = Mathf.Clamp(HP -= damage, 0, maxHp);
            healthFillBar.fillAmount = HP / maxHp;
            DameIncomeAction();
            if (HP <= 0)
            {
                Die();
                var randomItem = Pancake.Random.Range(0, gemItems.Count);
                Instantiate(gemItems[randomItem], transform.position, Quaternion.identity);
                Observer.CheckArea?.Invoke();
                IsDead = true;
            }
        }
    }
    public abstract void DameIncomeAction();
    public abstract void Die();
    public void GetSlow(float percentage, float timeToslow)
    {
        moveSpeed = maxMoveSpeed;
        DOTween.Kill(getslowSequence);
        getslowSequence = null;
        moveSpeed -= moveSpeed * (percentage / 100);
        getslowSequence = DOTween.Sequence();
        getslowSequence.AppendInterval(timeToslow).AppendCallback((() =>
        {
            moveSpeed = maxMoveSpeed;
            Debug.Log(moveSpeed);
        }));
    }
}
