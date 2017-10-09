using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linecast : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 endPosition;

    public Text sp;
    public Text ep;

    private Swipe swipe;

	// Use this for initialization
	void Start () {
        swipe = GetComponent<Swipe>();
	}
	
	// Update is called once per frame
	void Update () {
        
        sp.text = "StartPosition: " + startPosition;
        ep.text = "EndPosition: " + endPosition;
        //if(startPosition != Vector3.zero && endPosition != Vector3.zero)
        if (!swipe.swiping)
        {
            startPosition = new Vector3(Camera.main.ScreenToWorldPoint(swipe.startPosition).x, Camera.main.ScreenToWorldPoint(swipe.startPosition).y, 0f);
            endPosition = new Vector3(Camera.main.ScreenToWorldPoint(swipe.endPosition).x, Camera.main.ScreenToWorldPoint(swipe.endPosition).y, 0f);

            RaycastHit hit;
            if (Physics.Linecast(startPosition, endPosition, out hit))
            {
                //Debug.Log(hit.collider.name);
                if (hit.collider.name.Contains("Enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
                startPosition = Vector3.zero;
                endPosition = Vector3.zero;
            }
        }
        
	}
}
