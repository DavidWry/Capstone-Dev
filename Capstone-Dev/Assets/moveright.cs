using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveright : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += new Vector3(1, 0, 0);
	}
}
