using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pancake;
using UnityEngine;
using UnityEngine.UI;

public class HealthBoard : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image energyFill;
    [ReadOnly] [SerializeField] float maxHealth;
    [ReadOnly] [SerializeField] float maxEnergy;
    private float _currentHealth;
    private float _currentEnergy;
    private ITarget _target;
    private void Awake()
    {
        var getTarget = GameObject.FindGameObjectsWithTag(Constant.Player).ToList();
        _target = getTarget.Find(c => c.GetComponent<MainPlayer>()).GetComponent<ITarget>();
        maxHealth = _target.HP;
        maxEnergy = _target.Energy;
        _currentHealth = maxHealth;
        energyFill.fillAmount = _currentEnergy / maxEnergy;
        Debug.Log(_currentEnergy/maxEnergy);
        healthFill.fillAmount = _currentHealth / maxHealth;
    }
    private void OnEnable()
    {
        Observer.UpdatePlayerHealth += UpdateHealth;
        Observer.UpdatePlayerEnergy += UpdateEnergy;
    }
    private void OnDisable()
    {
        Observer.UpdatePlayerHealth -= UpdateHealth;
        Observer.UpdatePlayerEnergy -= UpdateEnergy;
    }
    void UpdateHealth(float value, bool isIncrease)
    {
        var trueValue = isIncrease ? value : value * -1;
        _currentHealth = Mathf.Clamp(_currentHealth + trueValue, 0, maxHealth);
        healthFill.fillAmount = _currentHealth / maxHealth;
    }
    void UpdateEnergy(float value, bool isIncrease)
    {
        var trueValue = isIncrease ? value : value * -1;
        _currentEnergy = Mathf.Clamp(_currentEnergy + trueValue, 0, maxEnergy);
        energyFill.fillAmount = _currentEnergy / maxEnergy;
    }
}
