using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider>().material.dynamicFriction = 0;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider>().material.dynamicFriction = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider>().material.dynamicFriction = 1;
        }
    }


}
