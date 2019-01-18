using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireinthehoe : MonoBehaviour {
    public GameObject fireball;
    public GameObject fireball1;
    public GameObject fireballpos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

       
	}

    void lavaBall() {
         
        for(int i = 0; i < 3; i++)
        {

         GameObject frb=  Instantiate(fireball, fireballpos.gameObject.transform.position,Quaternion.identity);
            frb.transform.parent = null;
            frb.transform.eulerAngles = new Vector3(0, 0, 0);
        }


    }


    void fireballcharge()
    {

        GameObject frb = Instantiate(fireball1, fireballpos.gameObject.transform.position, Quaternion.identity);
        print("nmsl");

    }
}
 