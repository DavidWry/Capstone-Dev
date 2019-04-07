using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using AssemblyCSharp; 
using UnityEngine.SceneManagement;
public class achievementSet : MonoBehaviour {
    GameObject player;
    public bool ac1, ac2, ac3, ac4, ac5, ac6, ac7, ac8, ac9, ac10, ac11, ac12, ac13, ac14, ac15, ac16, ac17, ac18, ac19, ac20;
    XmlDocument achievementDoc = new XmlDocument();
    float levelTime = 0;
    // Use this for initialization
    void Start () {

            string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
        achievementDoc.Load(achievementFilePath);
        ac1 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC1/Completed").InnerText);
        ac2 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC2/Completed").InnerText);
        ac3 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC3/Completed").InnerText);
        ac4 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC4/Completed").InnerText);
        ac5 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC5/Completed").InnerText);
        ac6 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC6/Completed").InnerText);
        ac7 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC7/Completed").InnerText);
        ac8 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC8/Completed").InnerText);
        ac9 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText);
        ac10 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC10/Completed").InnerText);
        ac11 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC11/Completed").InnerText);
        ac12 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC12/Completed").InnerText);
        ac13 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC13/Completed").InnerText);
        ac14 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC14/Completed").InnerText);
        ac15 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC15/Completed").InnerText);
        ac16 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC16/Completed").InnerText);
        ac17 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC17/Completed").InnerText);
        ac18 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC18/Completed").InnerText);
        ac19 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC19/Completed").InnerText);
        ac20 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC20/Completed").InnerText);
    }
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "2_3")
        {
            levelTime += Time.deltaTime;
        }

        if (!player)
        {
            player = GameObject.FindWithTag("Player");

        }


        if (!ac2)
        {  
            if (SceneManager.GetActiveScene().name == "MainRoom"&& bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC2/Count").InnerText)==true&&bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC2/Completed").InnerText)==false)
            {
                popup(2);
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC2/Completed").InnerText = "true";
                
                achievementDoc.Save(achievementFilePath);
            }
        }
        if (!ac3)
        {
            if (SceneManager.GetActiveScene().name == "2_1")
            {
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC3/Completed").InnerText = "true";

                achievementDoc.Save(achievementFilePath);
                popup(3);
            }

        }
        if (!ac4)
        {
            if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC4/Time").InnerText) > 600.0f)
            {
                popup(4);
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC4/Completed").InnerText = "true";

                achievementDoc.Save(achievementFilePath);
            }

        }

        if (!ac5)
        {
            if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText)==0&& SceneManager.GetActiveScene().name == "2_3")
            {
                if (GameObject.Find("Portal_2(Clone)"))
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC5/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                    popup(5);
                    
                }

            }

        }

 

        if (!ac7)
        {
            if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC7/Count").InnerText) == 0 && SceneManager.GetActiveScene().name == "2_3")
            {
                if (GameObject.Find("Portal_2(Clone)"))
                {
                    popup(7);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC7/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                }
            }
        }
        if (!ac8)
        {
            if (SceneManager.GetActiveScene().name == "2_3")
            {

                if (GameObject.Find("Portal_2(Clone)")&&GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint >95)
                {
                   
                    
                    popup(8);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC8/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                }

            }

        }

        if (!ac9)
        {
            ac9 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText);
            if (ac9)
            {
                popup(9);
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText = "true";

                achievementDoc.Save(achievementFilePath);
            }

        }

        if (!ac10)
        {

           
        }
        if (!ac11)
        {
            if (SceneManager.GetActiveScene().name == "2_3") {
                if (GameObject.Find("Portal_2(Clone)")){
                    if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText + levelTime ) < 600) {
                        string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                        achievementDoc.Load(achievementFilePath);
                        achievementDoc.DocumentElement.SelectSingleNode("AC11/Completed").InnerText = "true";

                        achievementDoc.Save(achievementFilePath);
                        popup(11);

                    }
                }

            }

        }


        if (!ac12)
        {
            if (player.GetComponent<Player_New>().leftWeapon.Name == "Laser" || player.GetComponent<Player_New>().rightWeapon.Name == "Laser")
            {
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC12/Completed").InnerText = "true";

                achievementDoc.Save(achievementFilePath);
                popup(12);
               


            }

        }
        if (!ac13)
        {

            if (SceneManager.GetActiveScene().name == "2_3")
            {

                if (GameObject.Find("Portal_2(Clone)")&&GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint>0&&GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint<5)
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC13/Completed").InnerText = "true";

                    achievementDoc.Save(achievementFilePath);
                    popup(13);
                }

            }
        }

        if (!ac14)
        {
          if(int.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC14/Count").InnerText) >= 100)
            {
                string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                achievementDoc.Load(achievementFilePath);
                achievementDoc.DocumentElement.SelectSingleNode("AC14/Completed").InnerText = "true";

                achievementDoc.Save(achievementFilePath);
                popup(14);
            }


        }

        if (!ac18)
        {
            if (ac1 && ac2 && ac3 && ac4 && ac5 && ac6 && ac7 && ac8 && ac9 && ac10 && ac11 && ac12 && ac13 && ac14)
            {

                popup(18);
            }


        }

	}




    void popup(int num)
    {



    }
}
 

