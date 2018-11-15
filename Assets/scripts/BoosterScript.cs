using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour {

    public float boostForce = 300f;

    Rigidbody body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        body.AddForce(transform.forward * boostForce * Time.deltaTime); // * Input.GetAxis("Vertical"));
	}
}
