using System;
using System.Collections;
using System.Collections.Generic;
using Pancake;
using UnityEngine;

[Serializable]
public struct PlayerInfor
{
    public Vector3 currentPos;
    public Vector3 rotation;
}

public class SubPlayer : MonoBehaviour
{
    [ReadOnly] public List<PlayerInfor> playerInfors;
    private void FixedUpdate()
    {
        UpdatePLayerInfor();
    }
    void UpdatePLayerInfor()
    {
        PlayerInfor temp = new PlayerInfor();
        temp.currentPos = transform.position;
        temp.rotation = transform.eulerAngles;
        playerInfors.Add(temp);
    }
    public void ClearPlayerInfor()
    {
        playerInfors.Clear();
        UpdatePLayerInfor();
    }
}
