using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour {

    public Rigidbody rb;
    public float maxForce = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void applyForce(Vector3 force)
    {
        rb.velocity += force * Time.deltaTime;
        //Debug.Log(rb.velocity);
        if (rb.velocity.magnitude > maxForce)
        {
            rb.velocity = rb.velocity.normalized * maxForce;
        }
    }
}
