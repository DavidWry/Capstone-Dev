using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipBack : MonoBehaviour {

    public GameObject text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent.eulerAngles.y > 0)
        {
            text.transform.localScale = new Vector3(-0.01f , 0.01f, 0.05f);
        }
        else
        {
            text.transform.localScale = new Vector3(0.01f, 0.01f, 0.05f);
        }
	}
}
