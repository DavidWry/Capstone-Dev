﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spears used by the Type 2 Minions
public class SpearProjectile : MonoBehaviour
{

    public float speed;
    private Transform player;
    private Vector2 target;
    void Start()
    {
        //speed = 5;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile();
        }
 
    }

    // Destroy the projectile
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}