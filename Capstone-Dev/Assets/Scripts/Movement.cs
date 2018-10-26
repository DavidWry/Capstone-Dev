using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float WalkSpeed = 3.0f;
    public bool IsFaceRight = true;

    private float yRotate;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // float value = Input.GetAxis("Left Y");
        //Debug.Log(value);
        if (Mathf.Abs(Input.GetAxis("Left X")) <= 1 && Mathf.Abs(Input.GetAxis("Left Y")) <= 1){
            if (Input.GetAxis("Left X") > 0.03)
            {
                IsFaceRight = true; 
                yRotate = 0;
                transform.rotation = Quaternion.Euler(0, yRotate, 0);
                transform.Translate(Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
            }
            else if (Input.GetAxis("Left X") < -0.03) {
                IsFaceRight = false;
                yRotate = 180;
                transform.rotation = Quaternion.Euler(0, yRotate, 0);
                transform.Translate(-Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, yRotate, 0);
                transform.Translate(-Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
            }
        }

       


    }
}
