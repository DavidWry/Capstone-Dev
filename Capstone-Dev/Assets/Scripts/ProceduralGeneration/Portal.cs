using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    public string nextSceneName;
    private bool isPlayerNearby;
	// Use this for initialization
	void Start () {
        isPlayerNearby = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))//button B in joystick
            {

                NextScene.loadName = nextSceneName;
                SceneManager.LoadScene("LoadingScene");
            }
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNearby = true;
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNearby = false;
        }

    }
}
