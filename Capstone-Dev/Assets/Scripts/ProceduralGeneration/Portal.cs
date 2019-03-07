using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class Portal : MonoBehaviour {

    public string nextSceneName;
    private bool isPlayerNearby;
    private GameObject player;
	// Use this for initialization
	void Start () {
        isPlayerNearby = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))//button B in joystick
            {
                SaveSystem.SavePlayer(player.GetComponent<Player_New>());
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
