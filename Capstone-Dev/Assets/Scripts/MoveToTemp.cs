using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTemp : MonoBehaviour {

    public Transform Player;

	// Use this for initialization
	void Start () {
        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Rigidbody>().AddForce(-transform.right * 2);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, 1);
    }
}
