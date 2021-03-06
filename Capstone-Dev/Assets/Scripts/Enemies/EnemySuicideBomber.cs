﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AssemblyCSharp;


// Follow the player everywhere
public class EnemySuicideBomber : MonoBehaviour
{
    private int health;
    private float currentHealth;
    private float speed;
    private Player_New player2;
    //private Player player2;
    private int damage;
    //Within what range the enemy will start and continue attacking the player
    private Transform target;
    private float rangeForAttack;

    private DropProbability probability = null;
    private GameManager gameManager = null;
    public GameObject explosion;

    private Animator anim;

    private float distanceForColor;
    private Color startColor = Color.red;
    private Color endColor = Color.yellow;
    private Color defaultColor;
    private SpriteRenderer myRenderer;

    private Scene scene;

    public Image healthBar;

    //Patrol
    private Vector3 moveSpot;
    private float patrolTime;

    //max movement on the axes;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private bool isChasing;
    private bool canPatrol;
    private bool collidedWithObstacle;
    private bool reachedPatrolPoint;
    private float chaseAgainTime;

    private Rigidbody rb;
    private bool isStunned;
    private CapsuleCollider capsule;

    //Sound
    private SoundManager soundmanager;

    public bool shouldInstParticle;
    public bool hasInstParticle;
    public GameObject rainEffect;
    GameObject tempRain;
    private float healTimer;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        capsule = gameObject.GetComponent<CapsuleCollider>();
        

        if (scene.name == "2_1"|| scene.name == "2_2" || scene.name == "3_1" || scene.name == "3_2")
        {

            transform.localScale = new Vector3(5f, 5f, 1f);
            speed = 40.0f;
            rangeForAttack = 100;
            explosion.transform.localScale = new Vector3(40f, 40f, 1f);
            capsule.radius = 1.88f;
            capsule.height = 5.73f;
            minX = transform.position.x - 60;
            maxX = transform.position.x + 60;
            minY = transform.position.y + 60;
            maxY = transform.position.y - 60;
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            speed = 2.0f;
            rangeForAttack = 7.5f;
            explosion.transform.localScale = new Vector3(2f, 2f, 1f);
            capsule.radius = 0.45f;
            capsule.height = 5.73f;
            minX = transform.position.x - 3;
            maxX = transform.position.x + 3;
            minY = transform.position.y + 3;
            maxY = transform.position.y - 3;
        }
    }
    void Start()
    {
        //Set player as the target


        health = 51;
        currentHealth = health;
        player2 = GetComponent<Player_New>();

        rb = gameObject.GetComponent<Rigidbody>();
        isStunned = false;
       

        damage = 5;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        defaultColor = myRenderer.material.color;
        
        patrolTime = 1.5f;
        canPatrol = true;
        isChasing = false;
        collidedWithObstacle = false;
        chaseAgainTime = 1.25f;


        moveSpot = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY),0);
        reachedPatrolPoint = false;

        shouldInstParticle = false;
        hasInstParticle = false;
        healTimer = 10f;

        //  soundmanager = GetComponent<SoundManager>();

    }

    void Update()
    {
        // Attack player if its under the range
        if (target != null)
        {
            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);

            if (isChasing == true)      //face the new waypoint you are headed
            {
               
                if (target.position.x < transform.position.x)
                {
                    scale.x *= -1;
                    healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;

                }
                else
                {
                    healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                transform.localScale = scale;
            }
            else
            {
                if (moveSpot.x < transform.position.x)
                {
                    scale.x *= -1;
                    healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else
                {
                    healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                transform.localScale = scale;
            }
            


            if (!isStunned)
            {

                distanceForColor = Vector2.Distance(target.position, transform.position);
                
                if (Vector2.Distance(transform.position, target.position) <= rangeForAttack && Vector2.Distance(transform.position, target.position) > 0.5 && collidedWithObstacle == false)
                {
                        //move enemy to the player's position
                        anim.SetBool("isRunning", true);

                        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    
                    myRenderer.material.color = Color.Lerp(startColor, endColor, distanceForColor / rangeForAttack);
                        isChasing = true;
                    //   reachedPatrolPoint = true;
                }
                
                else
                {

                    if (canPatrol == true)
                    {

                        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
                        // moveSpot = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY),0);

                        anim.SetBool("isRunning", true);
                        myRenderer.material.color = defaultColor;
                        reachedPatrolPoint = false;
                        isChasing = false;

                    }
                    else
                    {
                        anim.SetBool("isRunning", false);
                        isChasing = false;
                    }

                }
            }


            if (currentHealth <= 0)
            {
               
                //drop item
                if (probability)
                {
                    string tempName = probability.DetermineDrop();
                    GameObject itemObj = gameManager.GetItemObj(tempName);
                    itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    if (NextScene.nowName == "2_1"|| NextScene.nowName == "2_2" || scene.name == "3_1" || scene.name == "3_2")
                        itemObj.transform.localScale = new Vector3(4, 4, 4);
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                }
                
                Destroy(gameObject);
            }
            if (collidedWithObstacle == true)
            {
                chaseAgainTime -= Time.deltaTime;

            }

            if(chaseAgainTime <= 0)
            {
                collidedWithObstacle = false;
                chaseAgainTime = 1.25f;
            }
         
            if (Vector2.Distance(transform.position, moveSpot) <= 0.1f)
            {
                reachedPatrolPoint = true;
               
            }

            if (reachedPatrolPoint == true)
            {
                canPatrol = false;
                patrolTime -= Time.deltaTime;
            }

            if (patrolTime < 0f)
            {
                canPatrol = true;
                patrolTime = 1.5f;
                moveSpot = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            }
        }

        if (health >= 51)
        {
            health = 51;
        }

        if (currentHealth >= 51)
        {
            currentHealth = 51;
        }

        if (shouldInstParticle == true & hasInstParticle == false)
        {
            // rainEffect.SetActive(true);
            tempRain = (GameObject)Instantiate(rainEffect, transform.position, Quaternion.identity);
            hasInstParticle = true;
        }

        if (tempRain != null)
        {
            tempRain.transform.position = transform.position;
        }

        if (hasInstParticle == true)
        {
            healTimer -= Time.deltaTime;
        }

        if (healTimer <= 0.0f)
        {
            shouldInstParticle = false;
            hasInstParticle = false;
            healTimer = 10f;
            Destroy(tempRain);
        }


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.PlaySound("Suicider_Exploding");
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(tempRain);
            Destroy(gameObject);
            
            other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
            Destroy(expl, 3);

        }

        if ((other.gameObject.tag == "Minion") || (other.gameObject.tag == "Obstacle") || (other.gameObject.tag == "Chest"))
        {
            /* isStunned = true;
             rb.velocity = Vector3.zero;
             anim.SetBool("isRunning", false);
             StartCoroutine(WaitAfterStun(3f));
             myRenderer.material.color = defaultColor;
             */
            if (isChasing == true)
            {
                // collidedWithObstacle = true;
                 myRenderer.material.color = defaultColor;
                collidedWithObstacle = true;
            }
            else
            {
                canPatrol = false;
                patrolTime = 1.5f;
                reachedPatrolPoint = true;
                collidedWithObstacle = false;

            }




        }
    }


    //Handle damage taken
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;
    }

    public void RainHealthUpdate(float amount)
    {
        currentHealth += amount;
        healthBar.fillAmount = currentHealth / health;
    }
    public void Stun(float stunTime)
    {

        isStunned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("isRunning", false);
        StartCoroutine(WaitAfterStun(stunTime));

    }
    private IEnumerator WaitAfterStun(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }


}
