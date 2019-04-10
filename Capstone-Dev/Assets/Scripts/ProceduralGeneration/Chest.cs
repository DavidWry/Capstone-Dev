using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
public class Chest : MonoBehaviour {
    XmlDocument achievementDoc = new XmlDocument();
    private int health;
    public int currentHealth;
    private DropProbability probability = null;
    private GameManager gameManager = null;
    public Image healthBar;
    public string sceneName;
    // Use this for initialization
    void Start () {
        health = 10;
        currentHealth = 10;
        if (sceneName == "2_1"|| sceneName == "2_2"|| sceneName == "3_1"|| sceneName == "3_2")
            healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sceneName = SceneManager.GetActiveScene().name;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentHealth < 0)
        {
            string tempName = probability.DetermineDrop();
            GameObject itemObj = gameManager.GetItemObj(tempName);
            itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
            if (sceneName == "2_1"|| sceneName == "2_2" || sceneName == "3_1" || sceneName == "3_2") {
                if (itemObj.transform.localScale.x == 2) {//is gun
                    itemObj.transform.localScale = new Vector3(30, 30, 30);
                }
                else if (itemObj.transform.localScale.x == 25)
                {
                    itemObj.transform.localScale = new Vector3(25, 25, 25);
                }
                else//crystal
                    itemObj.transform.localScale = new Vector3(60, 60, 60);
            }
            if (SceneManager.GetActiveScene().name == "2_1")
            {
                gameManager.GetComponent<ProcedualGeneration2_1>().lootCount--;
                if (gameManager.GetComponent<ProcedualGeneration2_1>().lootCount == 0) {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                }
            }
            else if(SceneManager.GetActiveScene().name == "2_2"){
                gameManager.GetComponent<ProcedualGeneration2_2>().lootCount--;
                if (gameManager.GetComponent<ProcedualGeneration2_1>().lootCount == 0)
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                }
            }
            var worldCanvas = GameObject.Find("worldCanvas").transform;
            itemObj.transform.parent = worldCanvas;
            Destroy(gameObject);
        }
        if(sceneName=="2_1"|| sceneName == "2_2" || sceneName == "3_1" || sceneName == "3_2")
            healthBar.fillAmount = currentHealth / health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
