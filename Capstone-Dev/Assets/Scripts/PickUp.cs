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

        if (loot != null){
            isLootNearby = true;
            gameManager.PressBObject.SetActive(true);
        }
        else {
            isLootNearby = false;
            gameManager.PressBObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Tds_Tile vTile = col.GetComponent<Tds_Tile>();
        Loot loot = col.GetComponent<Loot>();
        /*
        if (vTile != null)
        {

            //ONLY refresh variable current level
            vListCollider.Add(vTile);

            //make the player refresh it's pixel tiles variables
            if (vCharacter != null)
                vCharacter.RefreshVariables(vListCollider);
        }
        */
        if (loot != null){
            //make the player refresh it's pixel tiles variables
            RefreshLoot(loot);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Tds_Tile vTile = col.GetComponent<Tds_Tile>();
        Loot loot = col.GetComponent<Loot>();

        //check if we have it on the list so we can remove it
        //if (vTile != null)
        //{
        //    if (vListCollider.Contains(vTile))
        //   {
        //       vListCollider.Remove(vTile);

        //make the player refresh it's pixel tiles variables
        //       vCharacter.RefreshVariables(vListCollider);
        //   }
        // }
        if (loot != null){
            loot = null;
            RefreshLoot(loot);
        }
    }
}
