﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateprojectile : MonoBehaviour {
    Vector3 rotateVector;
    public GameObject bianda;
    int rotateFreq = 5;
    int upMin = 60;
    int upMax = 100;
	// Use this for initialization
	void Start () {
        rotateVector = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));
        rotateVector = rotateVector.normalized * rotateFreq;
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(upMin*3, upMax*3));
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * Random.Range(upMin*40, upMax* 40));
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * Random.Range(-upMax*40, upMax*40 ));
    }
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Rotate(rotateVector);
        if (this.gameObject.transform.localPosition.z < 0)
        {
            GameObject lava = Instantiate(bianda, this.transform.position,Quaternion.Euler(90.0f,0,0));
            lava.transform.parent = null;
            Destroy(this.gameObject.transform.parent.gameObject);
            
        }





	}
}