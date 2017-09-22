using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accelerometer : MonoBehaviour {

    public Rigidbody rb;
    public float speed = 1;
    public Text text;

	// Use this for initialization
	void Start () {
        if (rb == null) { rb = gameObject.GetComponent<Rigidbody>(); }
        
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        text.text = (Input.acceleration.x).ToString() + ", " + gameObject.transform.position;
        Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        //dir.y = Input.acceleration.y;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        //rb.AddForce(dir, ForceMode.Acceleration);
        transform.position += dir;
        //transform.Translate(dir * speed);
    }
}
