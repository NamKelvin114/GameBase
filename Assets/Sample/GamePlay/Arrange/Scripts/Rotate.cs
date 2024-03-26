using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void OnMouseDrag()
    {
        Debug.Log(Input.GetAxis("Mouse X"));
        Debug.Log(Input.GetAxis("Mouse Y"));
        float rotatex = Input.GetAxis("Mouse X");
        float rotatey = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.down,rotatex);
        transform.Rotate(Vector3.right, rotatey);
    }
  
}
