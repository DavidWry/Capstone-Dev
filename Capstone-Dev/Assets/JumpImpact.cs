using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class JumpImpact : MonoBehaviour {

    private int damage;

    private Player_New player;
    private Transform player2;
    
    void Start ()
    {
        damage = 5;
        player = GetComponent<Player_New>();
        player2 = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        Destroy(gameObject, 0.2f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
            
        }
    }
}
