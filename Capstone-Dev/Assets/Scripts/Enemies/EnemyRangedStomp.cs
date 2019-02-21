﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyRangedStomp : MonoBehaviour
{

    public int numberOfProjectiles;             // Number of projectiles to shoot.
    public float projectileSpeed;               // Speed of the projectile.
    public GameObject projectilePrefab;         // Prefab to spawn.


    private Vector3 startPoint;                 // Starting position of the stomp.
    private const float radius = 1F;            // Help us find the move direction.

    private int health;
    private float currentHealth;

    private float attackRange;
    private float chaseRange;
    public float speed;
    private Transform target;


    private float timeBetweenShots;
    

    private DropProbability probability = null;
    private GameManager gameManager = null;

    private Animator anim;
    private Scene scene;

    public Image healthBar;

    private Rigidbody rb;
    private bool isStunned;
    private SpriteRenderer myRenderer;
    private Color color;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1")
        {
            transform.localScale = new Vector3(5f, 5f, 1f);

            attackRange = 80f;
            chaseRange = 120f;
            projectilePrefab.transform.localScale = new Vector3(20f, 20f, 1f);
            projectileSpeed = 40f;
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            attackRange = 4f;
            chaseRange = 6;
            projectilePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            projectileSpeed = 2f;
        }
    }
    private void Start()
    {
      
      
        health = 80;
        currentHealth = health;
        
        timeBetweenShots = 1.2f;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim = GetComponent<Animator>();

        rb = gameObject.GetComponent<Rigidbody>();
        isStunned = false;
        myRenderer = GetComponent<SpriteRenderer>();
        color = myRenderer.color;

    }

    // Update is called once per frame
    void Update()
    {
        //behavior here
        if (target != null)
        {

            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
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

           
            if (isStunned == false && currentHealth >=0)
            {

                startPoint = transform.position;
                if (Vector3.Distance(startPoint, target.position) <= attackRange)
                {
                    if (timeBetweenShots <= 0)
                    {
                        anim.SetTrigger("Attack");

                        SpawnProjectile(numberOfProjectiles);
                        timeBetweenShots = 1.6f;

                    }
                    else
                    {
                        anim.SetBool("isRunning", false);
                        timeBetweenShots -= Time.deltaTime;
                    }

                }
                else if (Vector2.Distance(transform.position, target.position) <= chaseRange && Vector2.Distance(transform.position, target.position) > attackRange)
                {
                    anim.SetBool("isRunning", true);

                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else
                {
                    anim.SetBool("isRunning", false);
                }
            }
            

            if (currentHealth <= 0)
            {
                anim.SetTrigger("hasDied");
                Destroy(gameObject, 0.75f);
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
                
                //Instantiate(crystal, transform.position,Quaternion.identity);
            }


        }

    }

    // Spawns x number of projectiles.
    private void SpawnProjectile(int _numberOfProjectiles)
    {
        float angleStep = 360f / _numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * projectileSpeed;

            // Create game objects.
            GameObject tmpObj = Instantiate(projectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);

            // Destory the gameobject after 10 seconds.
            Destroy(tmpObj, 3F);

            angle += angleStep;
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if ((other.gameObject.tag == "Minion") || (other.gameObject.tag == "Obstacle"))
        {
            myRenderer.color = Color.blue;
            isStunned = true;
            rb.velocity = Vector3.zero;
            anim.SetBool("isRunning", false);
            timeBetweenShots = 1.6f;
            StartCoroutine(WaitAfterStun(3f));
          

        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;
    }

    public void Stun(float stunTime)
    {
       
        isStunned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("isRunning", false);
        timeBetweenShots = 1.6f;
        StartCoroutine(WaitAfterStun(stunTime));
  
      
    }
    private IEnumerator WaitAfterStun(float time)
    {

        yield return new WaitForSeconds(time);
        isStunned = false;
        timeBetweenShots = 0f;
        myRenderer.color = color;
    }
}
