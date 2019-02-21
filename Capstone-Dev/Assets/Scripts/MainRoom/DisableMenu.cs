using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenu : MonoBehaviour {

    public GameObject manu;

	// Use this for initialization
	void Start () {
        if (NextScene.nowName == "MainRoom")
        {
            manu.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (NextScene.nowName == "MainRoom")
        {
            manu.SetActive(true);
        }
        else
        {
            manu.SetActive(false);
        }
    }
}
