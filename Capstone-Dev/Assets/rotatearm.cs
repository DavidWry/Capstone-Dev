using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatearm : MonoBehaviour {
   
    public GameObject player1;
    public GameObject thisParent;
	// Use this for initialization
	void Start () {
        player1 = GameObject.FindGameObjectWithTag("Player");
        thisParent = gameObject.transform.parent.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        
    }


    void LateUpdate() {

        if(player1)
        {
        ///shooting angle calculations
        var targetPos = player1.transform.position;
        var thisPos = thisParent.transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
    
       
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        if (player1.transform.position.x > thisParent.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 290));
          
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 240 - angle));
            
        }

        }

    }
}
