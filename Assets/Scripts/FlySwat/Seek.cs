using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 seek(Vector2 target, float weight)
    {
        Vector2 desired = target - (Vector2)this.transform.position;
        return desired *= weight;
    }
}
