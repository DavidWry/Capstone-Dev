using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour {

    public GameObject front;
    public GameObject back;

    private float speedBack;
    private float speedFront;
    // Use this for initialization
    void Start ()
    {
        speedBack = 0.2f;
        speedFront = 1.5f;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //move in respective directions
        front.transform.position -= new Vector3(0, Time.deltaTime * speedFront, 0);
        back.transform.position += new Vector3(0, Time.deltaTime * speedBack, 0);






        //if reached the end

        if (front.transform.position.y <= 398)
        {
            front.transform.position += new Vector3(0, 40, 0);
        }

        if (back.transform.position.y >= 438)
        {
            back.transform.position -= new Vector3(0, 40, 0);
        }
    }
}
