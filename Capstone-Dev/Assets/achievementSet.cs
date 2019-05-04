using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using AssemblyCSharp; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class achievementSet : MonoBehaviour {
    GameObject player;
    public bool ac1, ac2, ac3, ac4, ac5, ac6, ac7, ac8, ac9, ac10, ac11, ac12, ac13, ac14, ac15, ac16, ac17, ac18, ac19, ac20;
    XmlDocument achievementDoc = new XmlDocument();
    XmlDocument goldDoc = new XmlDocument();
    string progressionFilePath;
    public Image img1;
    public Text txt1;
    int gold = 0;
    public Text txt2;
    public float transp = 0;
    public bool popping;
    Color imgcolor;
    Color txtcolor;
    public float waittime=0;
    float levelTime = 0;
    // Use this for initialization
    void Start () {
        imgcolor = img1.color;
        txtcolor = txt1.color;
        progressionFilePath = Application.dataPath + "/Resources/Progression.xml";
        goldDoc.Load(progressionFilePath);
        gold = int.Parse(goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText);
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
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (popping)
            {
                if (transp < 1 && waittime < 2)
                {
                    transp += 0.05f;
                    imgcolor = new Color(imgcolor.r, imgcolor.g, imgcolor.b, transp);
                    txtcolor = new Color(txtcolor.r, txtcolor.g, txtcolor.b, transp);
                    img1.color = imgcolor;
                    txt1.color = txtcolor;
                    txt2.color = txtcolor;
                }
                else
                {

                    waittime += Time.deltaTime;
                    if (waittime > 2)
                    {
                        if (transp > 0)
                        {
                            transp -= 0.05f;
                            imgcolor = new Color(imgcolor.r, imgcolor.g, imgcolor.b, transp);
                            txtcolor = new Color(txtcolor.r, txtcolor.g, txtcolor.b, transp);
                            img1.color = imgcolor;
                            txt1.color = txtcolor;
                            txt2.color = txtcolor;
                        }
                        else
                        {


                            popping = false;
                            waittime = 0;
                            transp = 0;

                        }
                    }

                }

            }





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
                if (SceneManager.GetActiveScene().name == "MainRoom" && bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC2/Count").InnerText) == true && bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC2/Completed").InnerText) == false)
                {
                    gold += 50;
                    
                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                
                    goldDoc.Save(progressionFilePath);
                    popup(2);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC2/Completed").InnerText = "true";
                    ac2 = true;
                    achievementDoc.Save(achievementFilePath);
                }
            }
            if (!ac3)
            {
                if (SceneManager.GetActiveScene().name == "2_1")
                {  gold += 50;
                    
                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                
                    goldDoc.Save(progressionFilePath);
                    print("nimasile");
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC3/Completed").InnerText = "true";
                    ac3 = true;
                    achievementDoc.Save(achievementFilePath);
                    popup(3);
                }

            }
            if (!ac4)
            {
                if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC4/Time").InnerText) > 600.0f)
                {
                    gold += 50;

                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                    goldDoc.Save(progressionFilePath);
                    popup(4);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC4/Completed").InnerText = "true";
                    ac4 = true;
                    achievementDoc.Save(achievementFilePath);
                }

            }

            if (!ac5)
            {
                if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText) == 0 && SceneManager.GetActiveScene().name == "2_3")
                {
                    if (GameObject.Find("Portal_2(Clone)"))
                    {
                        gold += 50;

                        goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                        goldDoc.Save(progressionFilePath);
                        string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                        achievementDoc.Load(achievementFilePath);
                        achievementDoc.DocumentElement.SelectSingleNode("AC5/Completed").InnerText = "true";
                        ac5 = true;
                        achievementDoc.Save(achievementFilePath);
                        popup(5);

                    }

                }
                if (SceneManager.GetActiveScene().name == "MainRoom")
                {
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC5/Remaining").InnerText = "0";

                    achievementDoc.Save(achievementFilePath);

                }

            }



            if (!ac7)
            {
                if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC7/Count").InnerText) == 0 && SceneManager.GetActiveScene().name == "2_3")
                {
                    if (GameObject.Find("Portal_2(Clone)"))
                    {
                        gold += 50;

                        goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                        goldDoc.Save(progressionFilePath);
                        popup(7);
                        string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                        achievementDoc.Load(achievementFilePath);
                        achievementDoc.DocumentElement.SelectSingleNode("AC7/Completed").InnerText = "true";
                        ac7 = true;
                        achievementDoc.Save(achievementFilePath);
                    }
                }
            }
            if (!ac8)
            {
                if (SceneManager.GetActiveScene().name == "2_3")
                {

                    if (GameObject.Find("Portal_2(Clone)") && GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint > 95)
                    {
                        gold += 50;

                        goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                        goldDoc.Save(progressionFilePath);

                        popup(8);
                        string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                        achievementDoc.Load(achievementFilePath);
                        achievementDoc.DocumentElement.SelectSingleNode("AC8/Completed").InnerText = "true";
                        ac8 = true;
                        achievementDoc.Save(achievementFilePath);
                    }

                }

            }

            if (!ac9)
            {
                ac9 = bool.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText);
                if (ac9)
                {
                    gold += 50;

                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                    goldDoc.Save(progressionFilePath);
                    popup(9);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC9/Completed").InnerText = "true";
                    ac9 = true;
                    achievementDoc.Save(achievementFilePath);
                }

            }

            if (!ac10)
            {


            }
            if (!ac11)
            {
                if (SceneManager.GetActiveScene().name == "2_3")
                {
                    if (GameObject.Find("Portal_2(Clone)"))
                    {
                        if (float.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText + levelTime) < 600)
                        {
                            gold += 50;

                            goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                            goldDoc.Save(progressionFilePath);
                            string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                            achievementDoc.Load(achievementFilePath);
                            achievementDoc.DocumentElement.SelectSingleNode("AC11/Completed").InnerText = "true";
                            ac11 = true;
                            achievementDoc.Save(achievementFilePath);
                            popup(11);

                        }
                        else
                        {
                            string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                            achievementDoc.Load(achievementFilePath);
                            achievementDoc.DocumentElement.SelectSingleNode("AC11/Time").InnerText = "0";
                            achievementDoc.Save(achievementFilePath);



                        }
                    }

                }

            }


            if (!ac12)
            {
                if (player.GetComponent<Player_New>().leftWeapon.Name == "Laser" || player.GetComponent<Player_New>().rightWeapon.Name == "Laser")
                {
                    gold += 50;

                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                    goldDoc.Save(progressionFilePath);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC12/Completed").InnerText = "true";
                    ac12 = true;
                    achievementDoc.Save(achievementFilePath);
                    popup(12);



                }

            }
            if (!ac13)
            {

                if (SceneManager.GetActiveScene().name == "2_3")
                {

                    if (GameObject.Find("Portal_2(Clone)") && GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint > 0 && GameObject.FindWithTag("Player").GetComponent<Player_New>().HitPoint < 5)
                    {
                        gold += 50;

                        goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                        goldDoc.Save(progressionFilePath);
                        string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                        achievementDoc.Load(achievementFilePath);
                        achievementDoc.DocumentElement.SelectSingleNode("AC13/Completed").InnerText = "true";
                        ac13 = true;
                        achievementDoc.Save(achievementFilePath);
                        popup(13);
                    }

                }
            }

            if (!ac14)
            {
                if (int.Parse(achievementDoc.DocumentElement.SelectSingleNode("AC14/Count").InnerText) >= 100)
                {
                    gold += 50;

                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                    goldDoc.Save(progressionFilePath);
                    string achievementFilePath = Application.dataPath + "/Resources/Achievements.xml";
                    achievementDoc.Load(achievementFilePath);
                    achievementDoc.DocumentElement.SelectSingleNode("AC14/Completed").InnerText = "true";
                    ac14 = true;
                    achievementDoc.Save(achievementFilePath);
                    popup(14);
                }


            }

            if (!ac18)
            {
                if (ac1 && ac2 && ac3 && ac4 && ac5 && ac6 && ac7 && ac8 && ac9 && ac10 && ac11 && ac12 && ac13 && ac14)
                {
                    gold += 50;

                    goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();

                    goldDoc.Save(progressionFilePath);

                    popup(18);
                }


            }

        }

    }


        void popup(int num)
        {
            popping = true;
            if (num == 1)
            {
                txt1.text = "RAGE";
                txt2.text = "Killed 20 enemies within 20 seconds";

            }
            else if (num == 2) {
                txt1.text = "READY TO START";
                txt2.text = "Got through the tutorial";
            }
            else if (num == 3) {
                txt1.text = "MELTING";
                txt2.text = "Reached the first level";
            }
            else if (num == 4) {
                txt1.text = "PRACTICE MAKES PERFECT";
                txt2.text = "Trained for more than 10 minutes";
            }
            else if (num == 5) {
                txt1.text = "CLEAN SWEEP";
                txt2.text = "Killed every single enemy in the level";
            }
            else if (num == 6) {
                txt1.text = "BEAT IT";
                txt2.text = "Completed a run";
            }
            else if (num == 7) {
                txt1.text = "PURE AND SIMPLE";
                txt2.text = "Completed a run without using ults";
            }
            else if (num == 8) {
                txt1.text = "HEALTHY WEALTHY";
                txt2.text = "Have more than 95% HP at the end of a run";
            }
            else if (num == 9) {
                txt1.text = "TREASURE HUNTER";
                txt2.text = "Opened up every chest in a floor";
            }
            else if (num == 10) {
                txt1.text = "SAY HELLO TO MY LITTLE FRIEND";
                txt2.text = "Unlocked the character";
            }
            else if (num == 11) {
                txt1.text = "RUSH HOUR";
                txt2.text = "Completed a level with less than 10 minutes";
            }
            else if (num == 12) {
                txt1.text = "GLASS CUTTER";
                txt2.text = "Equipped laser on both hands";
            }
            else if (num == 13) {
                txt1.text = "SAILING CLOSE TO WIND";
                txt2.text = "Having less than 5% HP at the end of a level";
            }
            else if (num == 14) {
                txt1.text = "FRESH MEAT";
                txt2.text = "Used hook for more than 100 times";
            }
            else if (num == 15) {
                txt1.text = "DAZZLING";
                txt2.text = "Picked up more than 5 different weapons in a single run";
            }
            else if (num == 16) {
                txt1.text = "TOO BLIND TO SEE";
                txt2.text = "Completed a floor without dealing any damage";
            }
            else if (num == 17)
            {
                txt1.text = "ADDICTED";
                txt2.text = "Played for more than 5 hours";
            }
            else if (num == 18)
            {
                txt1.text = "PLATNIUM";
                txt2.text = "Completed all other achievements";
            }
            else if (num == 19)
            {
                txt1.text = "LOOK WHAT I FOUND";
                txt2.text = "Discovered a secret path";
            }
            else if (num == 20)
            {
                txt1.text = "PISTOL MASTER";
                txt2.text = "Used only pistol in a single run";
            }

        }
    
}
 

