using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForPlaytest : MonoBehaviour {

    public GameObject mission;
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
        NextScene.loadName = "3_1";
        SceneManager.LoadScene("LoadingScene");
    }
}
