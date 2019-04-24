using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForPlaytest : MonoBehaviour {

    public GameObject mission;
    public GameObject mission02;
    public GameObject menu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void manuPop()
    {
        menu.SetActive(false);
        mission.SetActive(true);
    }

    public void manusds()
    {
        menu.SetActive(false);
        mission02.SetActive(true);
    }
}
