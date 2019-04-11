using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using AssemblyCSharp;

public class Portal : MonoBehaviour {

    public string nextSceneName;
    private bool isPlayerNearby;
    private GameObject player;
    XmlDocument achievementDoc = new XmlDocument();
    // Use this for initialization
    void Start () {

        isPlayerNearby = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlayerNearby)
        {
          
            if (Input.GetKeyDown((KeyCode.Joystick1Button1)))//button B in joystick
            {
                SaveSystem.SavePlayer(player.GetComponent<Player_New>());
                NextScene.loadName = nextSceneName;
                if (SceneManager.GetActiveScene().name == "First Level")
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC2/Count").InnerText = "true";
                    achievementDoc.Save(achievementFilePath);

                }
                if (SceneManager.GetActiveScene().name == "2_1")
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    GameObject manager=GameObject.Find("GameManager");
                    
                    achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText = manager.GetComponent<ProcedualGeneration2_1>().enemyCount.ToString();
                    //achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText = manager.GetComponent<ProcedualGeneration2_1>().levelTime.ToString();
                    achievementDoc.Save(achievementFilePath);

                }
                if (SceneManager.GetActiveScene().name == "2_2")
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);

                    GameObject manager = GameObject.Find("GameManager");
                    int p = int.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText) + manager.GetComponent<ProcedualGeneration2_2>().enemyCount;
                    //float q= float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText+ manager.GetComponent<ProcedualGeneration2_2>().levelTime.ToString());
                    achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText =p.ToString();
                    //achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText = q.ToString();
                    achievementDoc.Save(achievementFilePath);

                }
                SceneManager.LoadScene("LoadingScene");
                print("dfkjshds");
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
