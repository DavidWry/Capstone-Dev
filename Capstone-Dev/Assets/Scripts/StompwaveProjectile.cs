﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class StompwaveProjectile : MonoBehaviour {

    public float speed;
    private Player player2;
    private Transform player;
    private Vector3 target;
    private Vector3 initialPos;
    private float travelDistance;
    private int damage;


    void Start()
    {
        damage = 8;
        //speed = 5;
        //var trail = GameObject.FindGameObjectsWithTag("Stompwavetrail");
        player2 = GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y, player.position.z);
        travelDistance = 30f;
        initialPos = transform.position;
        //transform.rotation = Quaternion.LookRotation(target);
        // transform.LookAt(player.position);
    }

    private void FixedUpdate()
    {

        //Once instantiated, head to the player's position
        //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(initialPos, transform.position) >= travelDistance)
        {
            DestroyProjectile();
        }


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

        if (other.gameObject.tag == "Player")
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
