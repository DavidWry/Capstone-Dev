﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {

    public GameObject PopCanvas;
    public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WithPopUp")
        {
            if (other.gameObject.GetComponent<PopUp>())
            {
                PopCanvas.SetActive(true);
                text.text = other.gameObject.GetComponent<PopUp>().context;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WithPopUp")
        {
            PopCanvas.SetActive(false);
        }
    }
}