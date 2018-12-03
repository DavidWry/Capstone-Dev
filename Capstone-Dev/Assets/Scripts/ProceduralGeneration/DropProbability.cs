using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class DropProbability : MonoBehaviour {

    public List<Item> ItemList;
    private float totalcount;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string DetermineDrop() {
        foreach (Item item in ItemList)
            totalcount += item.Probability;
        foreach (Item item in ItemList)
            item.Probability = item.Probability / totalcount;

        float count = Random.value;
        string itemName = "";

        foreach (Item item in ItemList) {
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

        return itemName;
    }
}
