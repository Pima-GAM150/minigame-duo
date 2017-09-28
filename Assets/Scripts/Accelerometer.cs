using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accelerometer : MonoBehaviour {

    public Rigidbody rb;
    public float maxMagnitude = 5f;
    public float speed = 1f;
    public Text text;

	// Use this for initialization
	void Start () {
        if (rb == null) { rb = gameObject.GetComponent<Rigidbody>(); }
        
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        text.text = (Input.acceleration.x).ToString();
        Vector3 dir = Vector3.zero;

        if(Mathf.Abs(Input.acceleration.x) > 0.1)
        {
            dir.x = Input.acceleration.x;
            //dir.y = Input.acceleration.y;
            if (dir.sqrMagnitude > maxMagnitude)
                dir.Normalize();

            dir *= Time.deltaTime * speed;
        }
        rb.AddForce(dir, ForceMode.Impulse);
        //transform.position += dir;
        //transform.Translate(dir * speed);
    }
}
