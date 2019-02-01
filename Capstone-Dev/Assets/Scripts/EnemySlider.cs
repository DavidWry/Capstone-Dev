using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class EnemySlider : MonoBehaviour
{
    private Rigidbody rb;
    public float dashSpeed;
    private float dashTime;
    private bool canDash;
    private int direction;

    private Player_New player;
    private Transform target;
    private Vector3 targetPos;


    private int health;
    private float rangeForAttack;
    private int damage;
    private Vector3 dir;

    private Animator anim;
    private bool hasCollided ;
    private bool hasReached;


    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        dashSpeed = 7f;
        canDash = true;
        dashTime = 2f;
        rangeForAttack = 6f;
        damage = 8;

        player = GetComponent<Player_New>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        hasCollided = false;
        hasReached = false;
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
            }
            transform.localScale = scale;
            
            //check if under range and if he can dash
            if( (Vector3.Distance(transform.position, target.position) <= rangeForAttack) && (canDash == true))
            {
                targetPos = new Vector3(target.position.x, target.position.y,target.position.z);
                anim.SetTrigger("Sliding");
                dir = (targetPos - transform.position).normalized * dashSpeed;
                rb.velocity = dir;
                canDash = false;
                hasReached = false;
            }

            //has the enemy reached the target position
            if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
            {
                hasReached = true;
                
                   

            }
            if (hasReached == true)
            {
                canDash = false;
                anim.SetBool("isRunning", false);
                rb.velocity = Vector3.zero;
                dashTime -= Time.deltaTime;
            }

            //if the countdown for the next dash becomes zero
            if (dashTime <= 0)
            {
               canDash = true;
                dashTime = 2f;
                target = GameObject.FindGameObjectWithTag("Player").transform; 
            }

        }
	}
    
    private void OnCollisionEnter(Collision other)
    {
        
        
        if (other.gameObject.tag == "Player")
         {
            if (hasCollided == false)
            {
                hasCollided = true;
                rb.velocity = Vector3.zero;
                anim.SetBool("isRunning", false);
                other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
                // StartCoroutine(IfCollidedWithPlayer(2f));
                hasReached = true;
                hasCollided = false;
               // canDash = true;

               // dashTime = 2f;
            //    target = GameObject.FindGameObjectWithTag("Player").transform;
            }
         

        }
    
       

        if(other.gameObject.tag == "Obstacle")
        {
            anim.SetBool("isRunning", false);
            rb.velocity = Vector3.zero;
            hasReached = true;
           // StartCoroutine(IfCollidedWithPlayer(2f));
          // canDash = true;
        

        }
       
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    IEnumerator IfCollidedWithPlayer(float newWaitTime)
    {
        yield return new WaitForSeconds(newWaitTime);
    }


}
