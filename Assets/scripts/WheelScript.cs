using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]

public class WheelScript : MonoBehaviour
{
    [SerializeField]
    private float torque;

    private WheelCollider wheel;

    // TODO: Possibly get torque from the chromosome itself...

    private void Start()
    {
        wheel = GetComponent<WheelCollider>();
        wheel.motorTorque = torque;
    }
}
