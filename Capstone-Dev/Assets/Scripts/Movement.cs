using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float WalkSpeed = 1f;

    private float yRotate;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // float value = Input.GetAxis("Left Y");
        //Debug.Log(value);
        if (Mathf.Abs(Input.GetAxis("Left X")) <= 1 && Mathf.Abs(Input.GetAxis("Left Y")) <= 1){
            transform.Translate(Input.GetAxis("Left X") * 0.05f * WalkSpeed, Input.GetAxis("Left Y") * 0.05f * WalkSpeed, 0);

            if (Input.GetAxis("Left X") > 0){
                yRotate = 0;
                transform.rotation=Quaternion.Euler(0,yRotate,0);
            }
            else {
                yRotate = 180;
                transform.rotation = Quaternion.Euler(0, yRotate, 0);
            }
        }

       


    }
}
