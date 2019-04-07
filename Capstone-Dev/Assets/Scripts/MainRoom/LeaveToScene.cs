using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Xml;
public class LeaveToScene : MonoBehaviour {
    XmlDocument achievementDoc = new XmlDocument();
    public string nextSceneName;
    float traintime = 0;
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainRoom")
        {
            traintime += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && nextSceneName != "")
        {
            NextScene.loadName = nextSceneName;
            if (SceneManager.GetActiveScene().name == "MainRoom")
            {

                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
               float p=float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC4/Time").InnerText);
                p += traintime;
                achievementDoc.DocumentElement.SelectSingleNode("AC4/Time").InnerText = p.ToString();
                achievementDoc.Save(achievementFilePath);


            }
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public void loadS()
    {
        transform.parent.gameObject.SetActive(false);
        NextScene.loadName = nextSceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
