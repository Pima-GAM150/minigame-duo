using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBallManager : MonoBehaviour {

    public bool gameOver = false;
    public int caughtObjects = 0;
    public GameObject sceneManager;

    private int totalObjects = 0;
    private bool caughtWrongObject = false;

	// Use this for initialization
	void Start () {
        caughtObjects = 0;
        totalObjects = sceneManager.GetComponent<CreateObjects>().totalObjects;
    }
	
	// Update is called once per frame
	void Update () {
        CalculateCaughtPercentage(caughtObjects, totalObjects);

    }

    public void CalculateCaughtPercentage(int caughtObjects, int totalObjects)
    {
        if((float)caughtObjects/ (float)totalObjects > 0.5f && !caughtWrongObject)
        {
            gameOver = false;
        } else
        {
            gameOver = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.name.Contains("Red"))
        {
            gameOver = true;
            caughtWrongObject = true;
        }
        if(collider.name.Contains("Blue"))
        {
            caughtObjects++;
        }
    }

}
