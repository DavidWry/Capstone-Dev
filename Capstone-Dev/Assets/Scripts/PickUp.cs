using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private bool isLootNearby = false;
    private Loot currentLoot = null;
    private GameManager gameManager = null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshLoot(Loot loot)
    {
        //show the loot items on ground
        if (loot != null)
            loot.ShowHide(true);
        else if (currentLoot != null)
            currentLoot.ShowHide(false);

        //check loot
        currentLoot = loot;

        if (loot != null)
        {
            isLootNearby = true;
            gameManager.PressBObject.SetActive(true);
        }
        else 
        {
            isLootNearby = false;
            gameManager.PressBObject.SetActive(false);
        }
    }
}
