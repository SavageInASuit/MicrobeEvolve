using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

public class BuoyancyControlScript : MonoBehaviour
{
    [SerializeField]
    public Transform waterSurface;
    [SerializeField]
    private Vector3 bouyancyForce;
    [SerializeField]
    private int waterDragForce;
    [SerializeField]
    private int airDragForce;

    private Rigidbody rb;
    ConstantForce cf;

    [SerializeField]
    bool bouyantMode;

    public void SetRigidBody(Rigidbody body){
        rb = body;
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waterSurface != null)
        {
            if (!bouyantMode && transform.position.y < waterSurface.position.y)
            {
                rb.useGravity = false;
                rb.drag = waterDragForce;
                rb.angularDrag = waterDragForce;
                cf.force = Vector3.zero;
                bouyantMode = true;
            }
            else if (bouyantMode && transform.position.y > waterSurface.position.y)
            {
                rb.useGravity = true;
                rb.drag = airDragForce;
                rb.angularDrag = airDragForce;
                cf.force = bouyancyForce;
                bouyantMode = false;
            }
        }
    }

    float GetBouyantForce()
    {
        // Need to work out the bouyancy force using the mass and 'bouyancy'
        // values of the hull + its components 

        return 0.0f;
    }
}
