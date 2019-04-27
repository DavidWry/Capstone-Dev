using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasflip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.parent.localScale.x == 4)
        {
            transform.localScale = new Vector3(-0.078f,transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(0.078f, transform.localScale.y, transform.localScale.z);
        }
    }
}
