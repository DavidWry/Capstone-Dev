using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyone : MonoBehaviour {
    public bool pig = false;
    float cs = 3;
    public GameObject portal;
    public GameObject positionaaa;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (pig)
         
            if (cs > 0)
            {
               
                cs -= 3 * Time.deltaTime;

            }
            else
            {
               
                portal.SetActive(true);
           
               


            }

        }
	}

