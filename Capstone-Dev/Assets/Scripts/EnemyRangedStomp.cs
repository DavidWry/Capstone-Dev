﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyRangedStomp : MonoBehaviour
{
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private float chaseRange;

    public float speed;
    private float timeBetweenShots;
    private float startTimeBetweenShots;
    private float health;
    public GameObject projectile;

    private int numProjectiles = 5;
    private float spreadFactor = 0.1f;
    public GameObject crystal;
    private Transform player;

    private DropProbability probability = null;
    private GameManager gameManager = null;

    /*private float coneSize =100;
    private float xSpread;
    private float ySpread;
    private Vector3 spread;*/


    void Start()
    {

        health = 55;
        speed = 2.3f;
        rangeForAttack = 5;
        chaseRange = 6;
        startTimeBetweenShots = 2.5f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots - 1.5f;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //projectileArray = new GameObject[numProjectiles];
    }


    void FixedUpdate()
    {
        //Attack if under the range
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= rangeForAttack)
            {
                if (timeBetweenShots <= 0)
                {
                    for (int i = 0; i < numProjectiles; i++)
                    {
                        Quaternion projRotation = transform.rotation;
                        projRotation.x += Random.Range(-spreadFactor, spreadFactor);
                        projRotation.y += Random.Range(-spreadFactor, spreadFactor);
                        Instantiate(projectile, transform.position, projRotation);

                    }

                    timeBetweenShots = startTimeBetweenShots;
                }
                else
                {
                    timeBetweenShots -= Time.deltaTime;
                }
            }
            if (Vector2.Distance(transform.position, player.position) <= chaseRange && Vector2.Distance(transform.position, player.position) > rangeForAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if (health <= 0)
            {
                if (probability)
                {
                    //drop item
                    string tempName = probability.DetermineDrop();
                    GameObject itemObj = gameManager.GetItemObj(tempName);
                    itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    if (NextScene.nowName == "2_1")
                        itemObj.transform.localScale = new Vector3(4, 4, 4);
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                }
                Destroy(gameObject);
                //Instantiate(crystal, transform.position,Quaternion.identity);
            }

        }

    }

    /*private void OnTriggerEnter(Collider other)
    {
        // if hit then move to the opposite direction to show a PUSHBACK effect
        if (other.CompareTag("Projectile"))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.right * 2);
            gameObject.GetComponent<Rigidbody>().velocity = Vector2.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, 1);
            Debug.Log("000");
        }
    }*/

    public void TakeDamage(int damage)
    {
        health -= damage;

    }

}