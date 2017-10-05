using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameController.ActiveController.WasSuccessful = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.name == "GameOver")
        {
            GameController.ActiveController.WasSuccessful = false;
        }
    }
}
