using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkin : MonoBehaviour
{
    [SerializeField] private Area currentArea;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.Player))
        {
            var getPlayer = other.gameObject.GetComponentInParent<MainPlayer>();
            if (getPlayer != null)
            {
                currentArea.StartEntry();
                gameObject.SetActive(false);
            }
        }
    }
}
