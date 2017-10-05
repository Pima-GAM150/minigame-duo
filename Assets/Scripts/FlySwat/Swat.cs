using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swat : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                Debug.Log("Clicking!");
                if (Physics.Raycast(ray, out hit))
                {

                    Debug.Log(hit.collider.name);
                    if (hit.collider.name.Contains("Fly"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition).origin + ", " + Camera.main.ScreenPointToRay(Input.mousePosition).direction);
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Clicking!");
            if (Physics.Raycast(ray, out hit))
            {

                Debug.Log(hit.collider.name);
                if (hit.collider.name.Contains("Fly"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
