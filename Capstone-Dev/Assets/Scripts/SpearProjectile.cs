﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

// Spears used by the Type 2 Minions
public class SpearProjectile : MonoBehaviour
{

    public float speed;
    private Player player2;
    private Transform player;
    private Vector3 target;
    private int damage;
  
    
    void Start()
    {
        damage = 8;
        player2 = GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y,player.position.z);
        
    }

    private void FixedUpdate()
    {
        
        //Once instantiated, head to the player's position
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
       

        }
        
    }

    //Handle Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            DestroyProjectile();
            
        } 

        if (other.gameObject.tag =="Player")
        {
            
            DestroyProjectile();

            //Call TakeDamage function from the player's script
            other.gameObject.GetComponent<Player>().TakeDamage(damage);

        }

        
 
    }

    // Destroy the projectile
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
