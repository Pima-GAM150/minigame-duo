using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour {

    public List<GameObject> prefabs;
    public float timeBetweenSpawn = 2f;
    public int totalObjects = 3;

    private float timer = 0f;
    private int rmdPref;
    private float width;
    private float height;


	// Use this for initialization
	void Start () {
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        InstaniateObjects();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(width + ", " + height);
	}

    public void InstaniateObjects()
    {
        Vector3 rmdPosition = new Vector3(Random.Range(-width, width), height*2, 0f);

        GameObject newObject = (GameObject) Instantiate(prefabs[0], rmdPosition, Quaternion.identity);

        //Changing the drag determines how fast the object will fall.
        newObject.GetComponent<Rigidbody>().drag = Random.Range(1.5f, 2.5f);

    }
}
