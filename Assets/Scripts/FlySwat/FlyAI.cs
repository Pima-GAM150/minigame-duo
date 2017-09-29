using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAI : MonoBehaviour {

    public ApplyForce af;
    public Seek seek;
    public float changeTargetTimer = 2f;

    private Vector2 target;
    private float timer = 0f;
    private float height;
    private float width;

	// Use this for initialization
	void Start () {
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        target = new Vector2(Random.Range(-width/2, width/2), Random.Range(-height/2, height/2));
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        af.applyForce(seek.seek(target, 1));
        if(timer > changeTargetTimer)
        {
            timer = 0;
            target = new Vector2(Random.Range(-15, 15), Random.Range(-15, 15));
        }
	}
}
