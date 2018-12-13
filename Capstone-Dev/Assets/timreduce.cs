using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timreduce : MonoBehaviour {
    private float timere = 1.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timere -= Time.deltaTime;
        if (timere < 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
	}
}
