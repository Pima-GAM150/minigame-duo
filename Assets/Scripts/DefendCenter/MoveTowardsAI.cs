using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsAI : MonoBehaviour {

    public Vector3 target;
    public ApplyForce applyForce;
    public Seek seek;
    public float weight = 1f;

    private GameObject home;

	// Use this for initialization
	void Start () {
        home = GameObject.Find("Home");
        target = home.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        target = home.transform.position;
        applyForce.maxForce = weight;
        applyForce.applyForce(seek.seek(target, weight));
	}
}
