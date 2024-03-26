using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotLucky : MonoBehaviour
{
    [SerializeField] private Image icon;
    public SlotLuckyData data;
    public void Init(SlotLuckyData getSlotLuckyData)
    {
        data = getSlotLuckyData;
        icon.sprite = data.icon;
    }
}
