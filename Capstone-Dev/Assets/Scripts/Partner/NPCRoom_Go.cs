using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRoom_Go : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("BButton"))
            {
                other.gameObject.transform.position = new Vector3(315, -475, 0);
            }
        }
    }
}
