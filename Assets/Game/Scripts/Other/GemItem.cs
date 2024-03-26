using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : MonoBehaviour
{
    [SerializeField] private ETypeGemItem eTypeGemItem;
    [SerializeField] private float value;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.Player))
        {
            var getPlayer = other.gameObject.GetComponentInParent<MainPlayer>();
            if (!getPlayer.IsDead)
            {
                switch (eTypeGemItem)
                {
                    case ETypeGemItem.HealthGem:
                        getPlayer.Heal(value);
                        break;
                    case ETypeGemItem.EnergyGem:
                        getPlayer.IncreaseEnergy(value);
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}
public enum ETypeGemItem
{
    HealthGem,
    EnergyGem,
}
