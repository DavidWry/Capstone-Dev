using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour {

    public GameObject front_1;
    public GameObject front_2;
    public GameObject back_1;
    public GameObject back_2;

    private float speedBack;
    private float speedFront;
    // Use this for initialization
    void Start ()
    {
        speedBack = 0.2f;
        speedFront = 1.5f;


        //back_1.transform.position.y += 2;
        //back_2.transform.position.y += 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        back_1.transform.position += new Vector3(0, Time.deltaTime * speedBack, 0);
        back_2.transform.position += new Vector3(0, Time.deltaTime * speedBack, 0);

        front_1.transform.position -= new Vector3(0, Time.deltaTime * speedFront, 0);
        front_2.transform.position -= new Vector3(0, Time.deltaTime * speedFront, 0);
    }
}
