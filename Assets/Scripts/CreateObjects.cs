using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float timeBetweenSpawn = 2f;
    public int totalObjects = 3;

    private float timer = 0f;
    private int rmdPref;
    private float width;
    private float height;
    private int totalSpawned = 0;
    private int catchObject = 0;

    public void InstaniateObjects()
    {
        Material newMaterial = new Material(Shader.Find("Diffuse"));

        Vector3 rmdPosition = new Vector3(Random.Range(-width, width), height * 2, 0f);

        int rmdInt = Random.Range(0, prefabs.Count);
        GameObject newObject = (GameObject)Instantiate(prefabs[rmdInt], rmdPosition, Quaternion.identity);

        if (rmdInt == catchObject)
        {
            newMaterial.color = Color.blue;
            newObject.name = "Blue";
        }
        else
        {
            newMaterial.color = Color.red;
            newObject.name = "Red";
        }

        //Changing the drag determines how fast the object will fall.
        newObject.GetComponent<Rigidbody>().drag = Random.Range(1.5f, 2.5f);
        newObject.GetComponent<MeshRenderer>().material = newMaterial;
    }

    // Use this for initialization
    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        catchObject = Random.Range(0, prefabs.Count);

        InstaniateObjects();
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(width + ", " + height);
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawn & totalSpawned < totalObjects)
        {
            InstaniateObjects();
            timer = 0;
            totalSpawned++;
        }
    }
}