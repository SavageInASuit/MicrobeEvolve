using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour {

    [SerializeField] private float boostForce = 300f;

    Rigidbody body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        body.AddForce(transform.forward * boostForce * Time.deltaTime); // * Input.GetAxis("Vertical"));
	}

    public void SetBoostForce(float force){
        boostForce = force;
    }
}
