using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Progression : MonoBehaviour {
    public int gold = 100;
    XmlDocument goldDoc = new XmlDocument();
    string progressionFilePath;
   // Use this for initialization
   void Start () {
        progressionFilePath = Application.dataPath + "/Resources/Progression.xml";
        goldDoc.Load(progressionFilePath);
        gold = int.Parse(goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spend(int Val)
    {
        if (Val == 1&& goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText == "false")
        {
            if (gold > 10)
            {
                gold -= 10;
                goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }

        else if (Val == 2)
        {
            if (gold > 20 && goldDoc.DocumentElement.SelectSingleNode("UC2/Completed").InnerText == "false")
            {
                gold -= 20;
                goldDoc.DocumentElement.SelectSingleNode("UC2/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 3)
        {
            if (gold > 30 && goldDoc.DocumentElement.SelectSingleNode("UC3/Completed").InnerText == "false")
            {
                gold -= 30;
                goldDoc.DocumentElement.SelectSingleNode("UC3/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 4 && goldDoc.DocumentElement.SelectSingleNode("UC4/Completed").InnerText == "false")
        {
            if (gold > 40)
            {
                gold -= 40;
                goldDoc.DocumentElement.SelectSingleNode("UC4/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 5 && goldDoc.DocumentElement.SelectSingleNode("UC5/Completed").InnerText == "false")
        {
            if (gold > 50)
            {
                gold -= 50;
                goldDoc.DocumentElement.SelectSingleNode("UC5/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 6 && goldDoc.DocumentElement.SelectSingleNode("UC6/Completed").InnerText == "false")
        {
            if (gold > 60)
            {
                gold -= 60;
                goldDoc.DocumentElement.SelectSingleNode("UC6/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 7 && goldDoc.DocumentElement.SelectSingleNode("UC7/Completed").InnerText == "false")
        {
            if (gold > 70)
            {
                gold -= 70;
                goldDoc.DocumentElement.SelectSingleNode("UC7/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
        else if (Val == 8 && goldDoc.DocumentElement.SelectSingleNode("UC8/Completed").InnerText == "false")
        {
            if (gold > 80)
            {
                gold -= 80;
                goldDoc.DocumentElement.SelectSingleNode("UC8/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                goldDoc.Save(progressionFilePath);
            }
        }
      
    }

}
