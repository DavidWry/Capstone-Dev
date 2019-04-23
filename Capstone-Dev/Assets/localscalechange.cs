using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localscalechange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.parent.localScale.x < 0)
        {
            transform.localScale = new Vector3(-25, 25, 25);
        }
        else
        {
            transform.localScale = new Vector3(25, 25, 25);
        }
    }
}
