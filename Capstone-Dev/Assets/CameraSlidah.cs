using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlidah : MonoBehaviour {
    Vector3 original;
    public bool offset=false;
	// Use this for initialization
	void Start () {
        original = gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

       Vector3 bs= new Vector3(Input.GetAxis("Right X"), Input.GetAxis("Right Y"),0);

        

        if (Mathf.Abs(Input.GetAxis("Right X")) < 0.1f && Mathf.Abs(Input.GetAxis("Right Y")) < 0.1f)
        {
            offset = false;


        }
        else {
            offset = true;
        
            if (gameObject.transform.localPosition.x< 50&& Input.GetAxis("Right X")>0.1f)
            {
                gameObject.transform.localPosition += new Vector3(1, 0, 0)* 150*Time.deltaTime;

            }
            else if(gameObject.transform.localPosition.x > -50 && Input.GetAxis("Right X") <- 0.1f)
            {
                gameObject.transform.localPosition += new Vector3(-1, 0, 0) * 150 * Time.deltaTime;

            }
            if (gameObject.transform.localPosition.y< 50 && Input.GetAxis("Right Y") > 0.1f)
            {
                 
                gameObject.transform.localPosition += new Vector3(0, 1, 0) * 150 * Time.deltaTime;

            }

            else if(gameObject.transform.localPosition.y > -50 && Input.GetAxis("Right Y") < -0.1f)
            {

                gameObject.transform.localPosition += new Vector3(0, -1, 0) * 150 * Time.deltaTime;
            }  


        }


   




    }
}
