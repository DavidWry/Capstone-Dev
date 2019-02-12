using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManu : MonoBehaviour {

    public GameObject Menu;
    GameObject Player;
    InterfaceAnimManager manager;

	// Use this for initialization
	void Start () {
		if (Menu)
        {
            manager = Menu.GetComponent<InterfaceAnimManager>();
        }
        Player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Menu)
            {
                if (Input.GetButtonDown("AButton"))
                {
                    Player.GetComponent<Movement_New>().enabled = false;
                    manager.startAppear(true);
                }
                else if (Input.GetButtonDown("BButton"))
                {
                    Player.GetComponent<Movement_New>().enabled = true;
                    manager.startDisappear(true);
                }
            }
        }
    }
}
