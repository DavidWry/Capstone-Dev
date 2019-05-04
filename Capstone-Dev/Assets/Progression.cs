using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
public class Progression : MonoBehaviour
{
    public int gold = 100;
    XmlDocument goldDoc = new XmlDocument();
    public GameObject cursor;
    string progressionFilePath;
    public GameObject circle1, circle2, circle3, circle4, circle5, circle6, circle7, circle8;
    public Sprite gou;
    public Text cost;
    // Use this for initialization
    void Start()
    {
        progressionFilePath = Application.dataPath + "/Resources/Progression.xml";
        goldDoc.Load(progressionFilePath);
        gold = int.Parse(goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText);
        if (goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText == "true")
        {
            circle1.GetComponent<Image>().sprite = gou;
        }
        if (goldDoc.DocumentElement.SelectSingleNode("UC2/Completed").InnerText == "true")
        {
            circle2.GetComponent<Image>().sprite = gou;
        }
        if (goldDoc.DocumentElement.SelectSingleNode("UC3/Completed").InnerText == "true")
        {
            circle3.GetComponent<Image>().sprite = gou;
        }
        if (goldDoc.DocumentElement.SelectSingleNode("UC4/Completed").InnerText == "true")
        {
            circle4.GetComponent<Image>().sprite = gou;
        }
        if (goldDoc.DocumentElement.SelectSingleNode("UC5/Completed").InnerText == "true")
        {
            circle5.GetComponent<Image>().sprite = gou;
        }
        if (goldDoc.DocumentElement.SelectSingleNode("UC6/Completed").InnerText == "true")
        {
            circle6.GetComponent<Image>().sprite = gou;
        }
 
    }

    // Update is called once per frame
    void Update()
    {
        if (cursor.GetComponent<ManuController>().currentButton==0)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "10"; }

        }
        if (cursor.GetComponent<ManuController>().currentButton == 1)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC2/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "20"; }
        }
        if (cursor.GetComponent<ManuController>().currentButton == 2)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC3/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "30"; }
        }
        if ( cursor.GetComponent<ManuController>().currentButton == 3)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC4/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "40"; }
        }
        if (cursor.GetComponent<ManuController>().currentButton == 4)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC5/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "50"; }
        }
        if (cursor.GetComponent<ManuController>().currentButton == 5)
        {
            if (goldDoc.DocumentElement.SelectSingleNode("UC6/Completed").InnerText == "true")
            {
                cost.text = "0";
            }
            else { cost.text = "60"; }
        }
 
    }

    public void spend(int Val)
    {
        if (Val == 1 && goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText == "false")
        {
            if (gold > 10)
            {
                gold -= 10;
                goldDoc.DocumentElement.SelectSingleNode("UC1/Completed").InnerText = "true";
                goldDoc.DocumentElement.SelectSingleNode("Gold").InnerText = gold.ToString();
                circle1.GetComponent<Image>().sprite = gou;
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
                circle2.GetComponent<Image>().sprite = gou;
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
                circle3.GetComponent<Image>().sprite = gou;
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
                circle4.GetComponent<Image>().sprite = gou;
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
                circle5.GetComponent<Image>().sprite = gou;
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
                circle6.GetComponent<Image>().sprite = gou;
                goldDoc.Save(progressionFilePath);
            }
        }


    }

}
