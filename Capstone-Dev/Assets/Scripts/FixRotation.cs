using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour {

    bool hasChanged = false;
	// Use this for initialization
	void Start () {
        Debug.Log(transform.rotation);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("new"+transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!hasChanged) {
            if (transform.rotation != Quaternion.Euler(0, 0, 0)) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                // hasChanged = true;
                Debug.Log(transform.rotation);
            }
        }
    }
}
