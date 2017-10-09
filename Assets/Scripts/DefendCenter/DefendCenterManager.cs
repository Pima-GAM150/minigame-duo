using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCenterManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
        GameController.ActiveController.WasSuccessful = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name.Contains("Enemy"))
        {
            GameController.ActiveController.WasSuccessful = false;
            Debug.Log("FAILED! " + GameController.ActiveController.WasSuccessful);
        }
    }
}
