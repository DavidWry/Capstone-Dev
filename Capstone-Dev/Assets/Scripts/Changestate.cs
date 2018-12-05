using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changestate : MonoBehaviour {
    int degree = 60;
    int speed = 20;
    Vector3 origina;
   public int rotateDir = -1;
    // Use this for initialization
    void Start () {
        origina = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z);
	}
	
	// Update is called once per frame
	void Update () {
        print(this.transform.eulerAngles);
        if (rotateDir == 0)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
        else if (rotateDir == 1)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * speed);
        }
        if (this.transform.eulerAngles.z>=60&& this.transform.eulerAngles.z <= 120      )
        {
            resetRot();
        }



    }
 
       void resetRot()
    {
        this.gameObject.transform.eulerAngles = origina;
    
        gameObject.SetActive(false);


    }






}
