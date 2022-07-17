using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Vector3 rotate = new Vector3();
    

    private void FixedUpdate()
    {
        transform.eulerAngles += rotate;
    }
}
