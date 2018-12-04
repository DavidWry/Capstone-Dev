using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class RotateFollow : MonoBehaviour {

    private Shoot playershot;

	// Use this for initialization
	void Start () {
        playershot = transform.parent.GetComponent<Shoot>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = playershot.Left.rotation;
        transform.position = playershot.Center.position;
	}
}
