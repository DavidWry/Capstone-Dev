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
    private Vector2 target;
    private int damage;
  
    
    void Start()
    {
        damage = 10;
        //speed = 5;
        //var trail = GameObject.FindGameObjectsWithTag("Stompwavetrail");
       player2 = GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
    }

    private void Update()
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
            Debug.Log("dsdajsio");
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
