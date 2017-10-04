using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    public float rotationTime = 2f;
    public float tilt = 7f; //needs to be positive

    private float rotationTimeLeft = 0f;
    private Quaternion target = Quaternion.Euler(0f, 0f, 0f);

    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
        
        rotationTimeLeft += Time.deltaTime;

        if(rotationTimeLeft > rotationTime)
        {
            float zTilt = Random.Range(-tilt, tilt);
            target = Quaternion.Euler(0f, 0f, zTilt);
            rotationTimeLeft = 0f;
            
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
    }
}
