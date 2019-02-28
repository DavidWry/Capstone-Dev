﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewEnemyJumper : MonoBehaviour
{

    private float rangeForAttack;
    private float waitTime;
    //private float startWaitTime;

    private float health;
    private float currentHealth;

    private DropProbability probability = null;
    private GameManager gameManager = null;
    //public GameObject crystal;

    Vector3 startPos;
    Vector3 nextPos;
    private Transform player;
    private Vector3 targetPos;
    private float speed;
    private float arcHeight;

    public GameObject landing;
    public GameObject impact;
    private bool canJump;


    private Animator anim;

    private bool hasInstantiatedLanding;
    private bool hasInstantiatedImpact;

    private Vector3 offset;
    private Rigidbody rb;

    private Scene scene;

    public Image healthBar;

    private bool isStunned;

    private CapsuleCollider capsule;

    private float ratio;
    private float reachedDistance;


    // public AnimationClip death;

    private void Awake()
    {

        //change size according to the scene
        capsule = gameObject.GetComponent<CapsuleCollider>();


        scene = SceneManager.GetActiveScene();

        if (scene.name == "2_1")
        {
            transform.localScale = new Vector3(6f, 6f, 1f);
            rangeForAttack = 140f;
            capsule.radius = 1.83f;
            capsule.height = 5.73f;
            landing.transform.localScale = new Vector3(50f, 20f, 1f);
            impact.transform.localScale = new Vector3(3f, 3f, 1f);
            speed = 70f;
            arcHeight =17f;
            reachedDistance = 2.0f;
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rangeForAttack = 7f;
            capsule.radius = 0.55f;
            capsule.height = 5.46f;
            landing.transform.localScale = new Vector3(2.5f, 1f, 1f);
            impact.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            speed = 8f;
            arcHeight = 1f;
            reachedDistance = 0.1f;
        }

    }
    void Start()
    {


        health = 90;
        currentHealth = health;


        waitTime = 0f;
        canJump = true;

        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        anim = GetComponent<Animator>();

        hasInstantiatedLanding = false;
        hasInstantiatedImpact = false;

        offset = new Vector3(0, 0.5f, 0);
        rb = GetComponent<Rigidbody>();

        isStunned = false;
    }

    void Update()
    {
        // Debug.Log("Length is: " + death.length);

        if (player != null)
        {
            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.position.x < transform.position.x)
            {
                scale.x *= -1;
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
            }
            else
            {
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
            }
            transform.localScale = scale;

            // startPos = transform.position;
            //initial states for jumping
            /*  if ((Vector3.Distance(startPos, player.position) <= rangeForAttack) && (canJump == true))
              {

                  targetPos = new Vector3(player.position.x, player.position.y, player.position.z);

                  startPos = transform.position;
                  anim.SetBool("isJumping", true);
                  Instantiate(landing, targetPos - offset , Quaternion.identity);

                  canJump = false;
              }


              if (anim.GetBool("isJumping") == true)
              {

                  float x0 = startPos.x;
                  float x1 = targetPos.x;
                  float dist = x1 - x0;
                  float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
                  float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
                  float baseZ = Mathf.Lerp(startPos.z, targetPos.z, (nextX - x0) / dist);
                  float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
                  nextPos = new Vector3(nextX, baseY + arc, baseZ - arc);

                  // Rotate to face the next position, and then move there
                  // transform.rotation = LookAt2D(nextPos - transform.position);
                  transform.position = nextPos;

              }
              else
              {
                  anim.SetBool("isJumping", false);
              }

              if (Vector3.Distance(nextPos,targetPos)<=0.01f)
             // if(nextPos == targetPos)
              {

                  waitTime -= Time.deltaTime;
                  canJump = false;

                  anim.SetBool("isJumping", false);
              }



              if( (Vector3.Distance(nextPos, targetPos) > 0.01) && (Vector3.Distance(nextPos, targetPos) < 0.1))
              {
                  Instantiate(impact, targetPos - offset, Quaternion.identity);
              }


              if (waitTime <= 0)
              {
                  canJump = true;
                  targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
                  waitTime = 3.0f;
              }*/
            if (!isStunned)
            {
                if (waitTime <= 0f)
                {
                    //targetPos = player.position;
                    targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
                    startPos = transform.position;
                    canJump = true;
                    hasInstantiatedLanding = false;
                    hasInstantiatedImpact = false;
                }
                if ((Vector3.Distance(startPos, targetPos) <= rangeForAttack) && (canJump == true))
                {
                    //targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
             
                    ratio = Mathf.Abs( startPos.x - targetPos.x) / rangeForAttack;
                    
                    anim.SetBool("isJumping", true);

                    if (hasInstantiatedLanding == false)
                    {
                        Instantiate(landing, targetPos - offset, Quaternion.identity);
                        hasInstantiatedLanding = true;
                    }

                    float x0 = startPos.x;
                    float x1 = targetPos.x;
                    float dist = x1 - x0;
                    float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * ratio * Time.deltaTime);
                    float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist ) ;
                    float baseZ = Mathf.Lerp(startPos.z, targetPos.z, (nextX - x0) / dist ) ;
                    float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
                  
                    nextPos = new Vector3(nextX, baseY + arc, baseZ - arc);
                    //baseZ - arc
                    // Rotate to face the next position, and then move there
                    // transform.rotation = LookAt2D(nextPos - transform.position);
                    transform.position = nextPos;

                    waitTime = 2f;
                }
                else
                {
                    anim.SetBool("isJumping", false);
                }

                if (Vector3.Distance(nextPos, targetPos) <= reachedDistance)
                {
                    anim.SetBool("isJumping", false);
                    if (hasInstantiatedImpact == false)
                    {
                        Instantiate(impact, targetPos - offset, Quaternion.identity);
                        hasInstantiatedImpact = true;
                    }
                    canJump = false;
                    waitTime -= Time.deltaTime;


                }
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
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;

    }
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Chest")
        {
            //isStunned = true;
          //  rb.velocity = Vector3.zero;
            anim.SetBool("isJumping", false);
            StartCoroutine(WaitAfterStun(2f));

        }

        if (collision.gameObject.tag == "Obstacle")
        {
            isStunned = true;
            rb.velocity = Vector3.zero;
            anim.SetBool("isJumping", false);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            StartCoroutine(WaitAfterStun(2f));
        }
    }

    public void Stun(float stunTime)
    {

        isStunned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("isJumping", false);
        StartCoroutine(WaitAfterStun(stunTime));



    }
    private IEnumerator WaitAfterStun(float time)
    {

        yield return new WaitForSeconds(time);
        waitTime = 0f;
        canJump = true;
        isStunned = false;
    }



}



/// 
/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
/// that makes the local +X axis point in the given forward direction.
/// 
/// forward direction
/// Quaternion that rotates +X to align with forward



