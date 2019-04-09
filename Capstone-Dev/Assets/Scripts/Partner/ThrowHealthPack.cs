using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHealthPack : MonoBehaviour {

    private Rigidbody RBody;
    private float Timer;

    // Use this for initialization
    void Start () {
        RBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        RBody.velocity = transform.right * 60;
        Timer += Time.deltaTime;
        if (Timer > 3.0f)
        {
            Destroy(gameObject);
        }
    }
}
