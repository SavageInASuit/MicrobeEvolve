using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour {
    [SerializeField]
    private float boostForce = InstanceData.BoosterForce;

    Rigidbody body;

    public float BoostForce
    {
        get
        {
            return boostForce;
        }
    }

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        body.AddForce(transform.forward * BoostForce * Time.deltaTime);
	}

    public void SetBoostForce(float force){
        boostForce = force;
    }
}
