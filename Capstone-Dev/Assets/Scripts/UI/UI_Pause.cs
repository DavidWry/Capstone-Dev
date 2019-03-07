using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Pause : MonoBehaviour {

    public GameObject pause;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Start"))
        {
            pause.SetActive(true);
            Time.timeScale = 0;
        }
	}

    public void Continue()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        NextScene.loadName = "MainRoom";
        SceneManager.LoadScene("LoadingScene");
    }
}
