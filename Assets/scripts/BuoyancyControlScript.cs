using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

/* TODO: Maybe this shouldn't use ConstantForce, but actually alter the value
// of the gravity force for the RigidBody(s) under this GameObject. 
// This would mean components attached to the hull with mass would affect 
// the rotation of the body, whereas now they don't

// Altering gravity force could be combined with a constant force for bouyancy
*/

public class BuoyancyControlScript : MonoBehaviour
{
    public Transform waterSurface;
    public Vector3 bouyancyForce;
    public int waterDragForce;
    public int airDragForce;

    Rigidbody rb;
    ConstantForce cf;

    bool bouyantMode;

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
