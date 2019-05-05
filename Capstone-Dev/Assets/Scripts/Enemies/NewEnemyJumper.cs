using System.Collections;
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
    private bool isDrop;

    private bool hasReached;
    private bool hasCollidedWithObjects;

    private CapsuleCollider cc;

    private float shakeTime;
   
    private bool shouldShake;
    private bool hasShaken;

    private bool shouldMakeSound;
    private bool hasMadeSound;
    private float soundTime;

    public bool shouldInstParticle;
    public bool hasInstParticle;
    public GameObject rainEffect;
    GameObject tempRain;
    private float healTimer;

    // public AnimationClip death;

    private void Awake()
    {

        //change size according to the scene
        capsule = gameObject.GetComponent<CapsuleCollider>();


        scene = SceneManager.GetActiveScene();

        if (scene.name == "2_1" || scene.name == "2_2" || scene.name == "3_1" || scene.name == "3_2")
        {
            transform.localScale = new Vector3(6f, 6f, 1f);
            rangeForAttack = 140f;
            capsule.radius = 1.83f;
            capsule.height = 5.73f;
            landing.transform.localScale = new Vector3(16f, 14f, 1f);
            impact.transform.localScale = new Vector3(9f, 9f, 1f);
            speed = 70f;
            arcHeight =17f;
            reachedDistance = 2.0f;
            offset = new Vector3(0, 9f, 0);
            soundTime = 14f;
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            rangeForAttack = 7f;
            capsule.radius = 0.55f;
            capsule.height = 5.46f;
            landing.transform.localScale = new Vector3(0.8f, 0.7f, 1f);
            impact.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            speed = 4f;
            arcHeight = 1f;
            reachedDistance = 0.1f;
            offset = new Vector3(0, 0.5f, 0);
            soundTime = 0.6f;
        }

    }
    void Start()
    {

        isDrop = false;
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

        
        rb = GetComponent<Rigidbody>();

        isStunned = false;

        hasReached = false;
        hasCollidedWithObjects = false;

        cc = GetComponent<CapsuleCollider>();

        shakeTime = 0.3f;
 
        shouldShake = false;
        hasShaken = false;

        shouldMakeSound = true;
        hasMadeSound = false;

        shouldInstParticle = false;
        hasInstParticle = false;
        healTimer = 10f;
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
            if (isStunned ==false)
            {
                if (waitTime <= 0f)
                {
                    //targetPos = player.position;
                    targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
                    startPos = transform.position;
                    canJump = true;
                    hasInstantiatedLanding = false;
                    hasInstantiatedImpact = false;
                    hasReached = false;
                    hasCollidedWithObjects = false;
                    hasShaken = false;
                    shouldShake = false;
                    shouldMakeSound = false;
                    hasMadeSound = false;
                }
                if ((Vector3.Distance(startPos, targetPos) <= rangeForAttack) && (canJump == true) &&(hasCollidedWithObjects == false))
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

                    cc.isTrigger = true;
                    transform.position = nextPos;

                    waitTime = 2f;
                    
                }
                else
                {
                    anim.SetBool("isJumping", false);
                    cc.isTrigger = false;
                }

                if (Vector3.Distance(nextPos, targetPos) <= reachedDistance)
                {
                    
                    hasReached = true;
                    shouldShake = true;
                   
                }


                if(Vector3.Distance(nextPos, targetPos) <= soundTime)
                {
                    shouldMakeSound = true;
                }

                if (shouldShake == true && hasShaken == false)
                {
                    if (scene.name == "First Level")
                    {
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<scshake1>().time = shakeTime;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<scshake>().time = shakeTime;
                    }
                    hasShaken = true;
                }

                if (shouldMakeSound == true  && hasMadeSound == false)
                {
                    SoundManager.PlaySound("Jumper");
                    hasMadeSound = true;
                }

                if(hasReached == true)
                {
                    
                    anim.SetBool("isJumping", false);
                    
                    if (hasCollidedWithObjects == false)
                    {
                        if (hasInstantiatedImpact == false)
                        {
                            Instantiate(impact, targetPos - offset, Quaternion.identity);
                            hasInstantiatedImpact = true;
                        }
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
            Destroy(tempRain);
            if (probability && !isDrop)
            {
                //drop item
                string tempName = probability.DetermineDrop();
                GameObject itemObj = gameManager.GetItemObj(tempName);
                itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || scene.name == "3_1" || scene.name == "3_2")
                    itemObj.transform.localScale = new Vector3(90, 90, 1);
                var worldCanvas = GameObject.Find("worldCanvas").transform;
                itemObj.transform.parent = worldCanvas;
                isDrop = true;
            }

            //Instantiate(crystal, transform.position,Quaternion.identity);
        }

        if (health >= 90)
        {
            health = 90;
        }

        if (currentHealth >= 90)
        {
            currentHealth = 90;
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

    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Chest")
        {
            //isStunned = true;
            //  rb.velocity = Vector3.zero;
            // anim.SetBool("isJumping", false);
            // StartCoroutine(WaitAfterStun(2f));
            hasReached = true;
            hasCollidedWithObjects = true;


        }

        if (collision.gameObject.tag == "Player")
        {
            isStunned = true;
            rb.velocity = Vector3.zero;
            anim.SetBool("isJumping", false);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            StartCoroutine(WaitAfterStun(2f));
        }
    }*/

    public void Stun(float stunTime)
    {

        isStunned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("isJumping", false);
        StartCoroutine(WaitAfterStun(stunTime));



    }
    private IEnumerator WaitAfterStun(float time)
    {
        waitTime = 0f;
        canJump = true;
        isStunned = false;
        yield return new WaitForSeconds(time);
       
    }



}



/// 
/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
/// that makes the local +X axis point in the given forward direction.
/// 
/// forward direction
/// Quaternion that rotates +X to align with forward



