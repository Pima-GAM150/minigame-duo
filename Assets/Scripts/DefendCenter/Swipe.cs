using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour {

    public Vector3 startPosition { get; set; }
    public Vector3 endPosition { get; set; }
    public bool swiping = false;

    public Text sp;
    public Text ep;

	// Use this for initialization
	void Start () {
        swiping = false;
	}
	
	// Update is called once per frame
	void Update () {

        //Only worrying about single touches on the screen.  If they do multiple should ignore it.
#if UNITY_ANDROID
        if (Input.touches.Length > 0)
        {

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startPosition = Input.touches[0].position;
                swiping = true;
                //sp.text = "StartPosition: " + startPosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                endPosition = Input.touches[0].position;
                swiping = false;
                //ep.text = "EndPosition: " + endPosition;
            }
        }/* else
        {
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
        }*/
#endif

#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            swiping = true;
            //sp.text = "StartPosition: " + startPosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;
            swiping = false;
            //ep.text = "EndPosition: " + endPosition;
        }
        
#endif


    }

}
