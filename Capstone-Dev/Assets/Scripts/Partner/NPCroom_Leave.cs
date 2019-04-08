using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCroom_Leave : MonoBehaviour {

    public GameObject manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("TextManager");
    }
	
	// Update is called once per frame
	void Update () {
        manager = GameObject.Find("TextManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.transform.position = manager.GetComponent<NPCManager>().PortalPosition;
        }
    }
}
