using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class CrystalPickup : MonoBehaviour
{


    private int rechargeAmount = 20;
	void Start ()
    {
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //Call TakeDamage function from the player's script
            other.gameObject.GetComponent<Player>().Power = other.gameObject.GetComponent<Player>().Power + rechargeAmount;
            Destroy(gameObject);

        }
    }
}
