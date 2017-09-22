using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    public float rotationTime = 2f;

    private float rotationTimeLeft = 0f;
    private Quaternion target = Quaternion.Euler(0f, 0f, 0f);

    // Update is called once per frame
    void Update () {
        rotationTimeLeft += Time.deltaTime;

        if(rotationTimeLeft > rotationTime)
        {
            float zTilt = Random.Range(-7.0f, 7.0f);
            target = Quaternion.Euler(0f, 0f, zTilt);
            rotationTimeLeft = 0f;
            
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
    }
}
