using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using UnityEngine.SceneManagement;


public class EnemySlider : MonoBehaviour
{
    private Rigidbody rb;
    public float dashSpeed;
    public float dashTime;
    public bool canDash;
    private int direction;

    private Player_New player;
    private Transform target;
    private Vector3 targetPos;


    private int health;
    private float currentHealth;
    private float rangeForAttack;
    private int damage;
    private Vector3 dir;

    private Animator anim;
    private bool hasCollided;
    public bool hasReached;
    private float reachedDistance;

    private DropProbability probability = null;
    private GameManager gameManager = null;
    private Scene scene;

    public Image healthBar;

    private CapsuleCollider capsule;
    private bool isStunned;

    public CamerShake cameraShake;

    private bool isDrop;

    public GameObject slide;
    GameObject tempSlide;
    private bool shouldDestroyDash;

    private bool hasCollidedWithOthers;
    private bool isDashing;
    // public AnimationClip death;

   
    void Start()
    {
        isDrop = false;
        scene = SceneManager.GetActiveScene();
        capsule = gameObject.GetComponent<CapsuleCollider>();

        if (scene.name == "2_1"|| scene.name == "2_2")
        {
            transform.localScale = new Vector3(6f, 6f, 1f);

            dashSpeed = 80f;
            rangeForAttack = 100f;
            capsule.radius = 1.84f;
            capsule.height = 5.51f;
            rangeForAttack = 120f;
            reachedDistance = 3f;
            slide.transform.localScale = new Vector3(14f, 14f, 1f);
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            dashSpeed = 6.5f;
            rangeForAttack = 6f;
            capsule.radius = 0.55f;
            capsule.height = 5.51f;
            reachedDistance = 0.1f;
            slide.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }
        rb = GetComponent<Rigidbody>();

        canDash = true;
        dashTime = 0.8f;

        damage = 7;
        health = 130;
        currentHealth = health;

        player = GetComponent<Player_New>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        hasCollided = false;
        hasReached = false;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isStunned = false;

        shouldDestroyDash = false;

        hasCollidedWithOthers = false;
        isDashing = false;

    }

    // Update is called once per frame
    void Update()
    {
       
        //if player isnt dead
        if (target != null)
        {
            //Make the enemy face the player
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


            if (isStunned == false)
            {
                if ((Vector3.Distance(transform.position, target.position) <= rangeForAttack) && (canDash == true))
                {
                    targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
                    anim.SetTrigger("Sliding");
                    dir = (targetPos - transform.position).normalized * dashSpeed;

                    var dirSlide = (targetPos - transform.position);
                    var angle = Mathf.Atan2(dirSlide.y, dirSlide.x) * Mathf.Rad2Deg;
                    var offset = new Vector3(0f, -0.8f,0f);
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    // var rotSlide = new Vector3(0, 0, angle);
               
                    tempSlide = (GameObject)Instantiate(slide, transform.position + offset, q);
        


                    rb.velocity = dir;
                    canDash = false;
                    isDashing = true;
                    hasReached = false;
                    shouldDestroyDash = false;
                    // StartCoroutine(cameraShake.Shake(0.2f, 0.15f));

                }

                //has the enemy reached the target position
                if (Vector2.Distance(transform.position, targetPos) <= reachedDistance)
                {
                    hasReached = true;
                    // shouldDestroyDash = true;
                    Destroy(tempSlide, 0.45f);
                }

                //Destroy the dash indicator
                if (shouldDestroyDash == true)
                {
                 //   Destroy(tempSlide, 0.45f);
                }

                if (hasReached == true)
                {
                   
                    canDash = false;
                    anim.SetBool("isRunning", false);
                    rb.velocity = Vector3.zero;
                    dashTime -= Time.deltaTime;
                    isDashing = false;
                  //  shouldDestroyDash = true;

                }

                //if the countdown for the next dash becomes zero
                if (dashTime <= 0)
                {
                    canDash = true;
                    dashTime = 1.6f;
                    hasReached = false;
                    hasCollidedWithOthers = false;
                    //  rb.isKinematic = false;
                    rb.WakeUp();
                    target = GameObject.FindGameObjectWithTag("Player").transform;
                    

                }
            }
            //check if under range and if he can dash


            if (currentHealth <= 0)
            {
                //drop item

                anim.SetTrigger("hasDied");
                Destroy(gameObject, 0.75f);
              //  Destroy(tempSlide, 0.5f);
                if (probability && !isDrop)
                {
                    string tempName = probability.DetermineDrop();
                    GameObject itemObj = gameManager.GetItemObj(tempName);
                    itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2")
                        itemObj.transform.localScale = new Vector3(4, 4, 4);
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                    isDrop = true;
                }

            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (hasCollided == false && dashTime == 1.6f)
            {
                //shouldDestroyDash = true;
                Destroy(tempSlide, 0.5f);
               // hasCollided = true;
                rb.velocity = Vector3.zero;
                anim.SetBool("isRunning", false);
                other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
                // StartCoroutine(IfCollidedWithPlayer(2f));
                hasReached = true;
                hasCollided = false;
            
            }
           


        }



        if ((other.gameObject.tag == "Minion") || (other.gameObject.tag == "Obstacle") || (other.gameObject.tag == "Chest"))
        {
           
           if (hasCollidedWithOthers == false)
            {
                //Destroy(tempSlide, 0.5f);
                anim.SetBool("isRunning", false);
                rb.velocity = Vector3.zero;
   
               
               // hasReached = true;
               // hasCollidedWithOthers = true;
                StartCoroutine(WaitAfterStun(1.6f));
            }

        
          //  isStunned = true;

           /* anim.SetBool("isRunning", false);
            rb.velocity = Vector3.zero;
             hasReached = true;
            canDash = false;
            Destroy(tempSlide, 0.5f);*/

            //shouldDestroyDash = true;
            //StartCoroutine(WaitAfterStun(1.6f));
          

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
        hasReached = true;
        StartCoroutine(WaitAfterStun(stunTime));

    }
    private IEnumerator WaitAfterStun(float time)
    {
        canDash = false;
        yield return new WaitForSeconds(time);
        dashTime = 1.6f;
       
        canDash = true;
        hasCollided = false;
        hasReached = false;
       // hasCollidedWithOthers = false;
      
    }




}
