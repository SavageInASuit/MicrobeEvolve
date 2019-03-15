using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(BoosterScript))]

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
    private ConstantForce cf;
    private BoosterScript[] boosters;

    [SerializeField]
    private bool bouyantMode;

    public void SetRigidBody(Rigidbody body){
        rb = body;
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();

        boosters = GetComponentsInChildren<BoosterScript>();

        foreach(BoosterScript booster in boosters)
            booster.SetBoostForce(booster.BoostForce/2);
    }

    // Update is called once per frame
    void Update()
    {
        if (waterSurface != null)
        {
            if (!bouyantMode && transform.position.y < waterSurface.position.y)
            {
                //rb.useGravity = false;
                rb.drag = waterDragForce;
                rb.angularDrag = waterDragForce;
                cf.force = bouyancyForce;

                foreach (BoosterScript booster in boosters)
                    booster.SetBoostForce(booster.BoostForce*2);

                bouyantMode = true;
            }
            else if (bouyantMode && transform.position.y > waterSurface.position.y)
            {
                //rb.useGravity = true;
                rb.drag = airDragForce;
                rb.angularDrag = airDragForce;
                cf.force = Vector3.zero;

                foreach (BoosterScript booster in boosters)
                    booster.SetBoostForce(booster.BoostForce/2);

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
