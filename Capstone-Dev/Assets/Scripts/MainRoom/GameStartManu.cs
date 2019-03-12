using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStartManu : MonoBehaviour {

    public GameObject Menu;
    GameObject Player;
    InterfaceAnimManager manager;
    bool isOff = true;

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
                if (Input.GetButtonDown("AButton") && isOff)
                {
                    Player.GetComponent<Movement_New>().enabled = false;
                    Menu.SetActive(true);
                    isOff = false;
                }
                else if (Input.GetButtonDown("BButton"))
                {
                    Player.GetComponent<Movement_New>().enabled = true;
                    Menu.SetActive(false);
                    isOff = true;
                }
            }
        }
    }
}
