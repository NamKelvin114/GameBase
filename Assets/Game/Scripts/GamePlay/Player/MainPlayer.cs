using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pancake;
using UnityEditor;
using UnityEngine;

public class MainPlayer : MonoBehaviour, ITarget
{
    [SerializeField] private float distance;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Rigidbody playerRig;
    [SerializeField] private Material normalStage;
    [SerializeField] private Material getDameStage;
    [SerializeField] private MeshRenderer playerMesh;
    [SerializeField, Range(1, 10)] private float speed;
    [SerializeField] private List<SubPlayer> subPlayers;
    [ReadOnly] [SerializeField] private List<SubPlayer> bodies;
    [SerializeField] private SubPlayer sub;
    [SerializeField] private Transform subPlayerSpwan;
    [SerializeField] private float hp;
    [SerializeField] private float shield;
    [SerializeField] private float energy;
    private Guid _gui;
    private Sequence _stateMatSequence;
    private float cooldown;
    private float _currentAngle;
    private float _maxHp;
    private float _maxEnergy;
    public float Energy
    {
        get => energy;
        set => energy = value;
    }
    public float HP
    {
        get => hp;
        set => hp = value;
    }
    public float Shield
    {
        get => shield;
        set => shield = value;
    }
    public void GetDamage(float damage)
    {
        DOTween.Kill(_stateMatSequence);
        _stateMatSequence = null;
        HP = Mathf.Clamp(HP -= damage, 0, _maxHp);
        Observer.UpdatePlayerHealth?.Invoke(damage, false);
        _stateMatSequence = DOTween.Sequence();
        SetStateMat(true);
        Debug.Log(Time.deltaTime);
        _stateMatSequence.AppendInterval(0.3f).AppendCallback((() =>
        {
            Debug.Log(Time.deltaTime);
            SetStateMat(false);
        }));
        Debug.Log(Time.deltaTime);
        if (HP <= 0)
        {
            IsDead = true;
        }
    }
    public void Heal(float value)
    {
        HP = Mathf.Clamp(HP + value, 0, _maxHp);
        Observer.UpdatePlayerHealth?.Invoke(value, true);
    }
    public void IncreaseEnergy(float value)
    {
        Energy = Mathf.Clamp(Energy + value, 0, _maxEnergy);
        Observer.UpdatePlayerEnergy?.Invoke(value, true);
    }
    public bool IsDead
    {
        get;
        set;
    }
    void SetStateMat(bool isGetDame)
    {
        if (isGetDame)
        {
            playerMesh.material = getDameStage;
        }
        else
        {
            playerMesh.material = normalStage;
        }
    }
    private void Start()
    {
        SetStateMat(false);
        _maxEnergy = energy;
        _maxHp = hp;
        IsDead = false;
        sub.ClearPlayerInfor();
        bodies.Add(this.sub);
    }
    private void FixedUpdate()
    {
        if (!IsDead)
        {
            Vector3 movedirec = transform.forward * speed;
            movedirec.y = playerRig.velocity.y;
            playerRig.velocity = movedirec;
            if (joystick.Direction != Vector2.zero)
            {
                _currentAngle = Mathf.Atan2(joystick.Direction.x, joystick.Direction.y) * Mathf.Rad2Deg;
            }
            transform.rotation = Quaternion.AngleAxis(_currentAngle, transform.up);
            if (bodies.Count > 1)
            {
                for (int i = 1; i < bodies.Count; i++)
                {
                    var prevSub = bodies[i - 1];
                    bodies[i].transform.position = prevSub.playerInfors[0].currentPos;
                    bodies[i].transform.eulerAngles = prevSub.playerInfors[0].rotation;
                    prevSub.playerInfors.RemoveAt(0);
                }
            }
            UpdateBody();
        }
    }
    void UpdateBody()
    {
        if (subPlayers.Count > 0)
        {
            cooldown += Time.deltaTime;
            if (cooldown >= distance)
            {
                var prevSub = bodies[bodies.Count - 1];
                var temp = Instantiate(subPlayers[0], prevSub.playerInfors[0].currentPos, Quaternion.Euler(prevSub.playerInfors[0].rotation));
                temp.transform.parent = subPlayerSpwan;
                bodies.Add(temp);
                subPlayers.RemoveAt(0);
                cooldown = 0;
                temp.ClearPlayerInfor();
            }
        }
    }

}
