using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.SceneManagement;


public class DropProbability : MonoBehaviour {

    public List<Item> ItemList;
    public List<Item> ItemListFor2_1;
    public List<Item> ItemListFor2_2;
    public List<Item> ItemListFor3_1;
    public List<Item> ItemListFor3_2;
    public string sceneName;
    private float totalcount;
    // Use this for initialization
    void Start () {
        sceneName = SceneManager.GetActiveScene().name;
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public string DetermineDrop() {
        string itemName = "";
        if (sceneName == "First Level")
        {
            foreach (Item item in ItemList)
                totalcount += item.Probability;
            foreach (Item item in ItemList)
                item.Probability = item.Probability / totalcount;
            float count = Random.value;
            
            foreach (Item item in ItemList)
            {
                if (count < item.Probability)
                {
                    itemName = item.Name;
                    break;
                }
                else
                {
                    count -= item.Probability;
                }
            }

        }
        else if (sceneName == "2_1")
        {
            foreach (Item item in ItemListFor2_1)
                totalcount += item.Probability;
            foreach (Item item in ItemListFor2_1)
                item.Probability = item.Probability / totalcount;
            float count = Random.value;

            foreach (Item item in ItemListFor2_1)
            {
                if (count < item.Probability)
                {
                    itemName = item.Name;
                    break;
                }
                else
                {
                    count -= item.Probability;
                }
            }

        }
        else if (sceneName == "2_2")
        {
            foreach (Item item in ItemListFor2_2)
                totalcount += item.Probability;
            foreach (Item item in ItemListFor2_2)
                item.Probability = item.Probability / totalcount;
            float count = Random.value;

            foreach (Item item in ItemListFor2_2)
            {
                if (count < item.Probability)
                {
                    itemName = item.Name;
                    break;
                }
                else
                {
                    count -= item.Probability;
                }
            }

        }
        else if (sceneName == "3_1")
        {
            foreach (Item item in ItemListFor3_1)
                totalcount += item.Probability;
            foreach (Item item in ItemListFor3_1)
                item.Probability = item.Probability / totalcount;
            float count = Random.value;

            foreach (Item item in ItemListFor3_1)
            {
                if (count < item.Probability)
                {
                    itemName = item.Name;
                    break;
                }
                else
                {
                    count -= item.Probability;
                }
            }

        }
        else if (sceneName == "3_2")
        {
            foreach (Item item in ItemListFor3_2)
                totalcount += item.Probability;
            foreach (Item item in ItemListFor3_2)
                item.Probability = item.Probability / totalcount;
            float count = Random.value;

            foreach (Item item in ItemListFor3_2)
            {
                if (count < item.Probability)
                {
                    itemName = item.Name;
                    break;
                }
                else
                {
                    count -= item.Probability;
                }
            }

        }
        return itemName;
    }
}
