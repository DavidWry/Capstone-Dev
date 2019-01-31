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

    private float rangeForAttack;
    private Vector3 dir;

    private Animator anim;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        dashSpeed = 7f;
        canDash = true;
        dashTime = 2f;
        rangeForAttack = 6f;

        player = GetComponent<Player_New>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
       
        if (target != null)
        {
            if((Vector3.Distance(transform.position, target.position) <= rangeForAttack) && (canDash == true))
            {
                anim.SetTrigger("Sliding");
                dir = (target.position - transform.position).normalized * dashSpeed;
                rb.velocity = dir;
                canDash = false;
            }
            if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            {
                anim.SetBool("isRunning", false);
                dashTime -= Time.deltaTime;
                canDash = false;
                rb.velocity = new Vector3(0f, 0f, 0f);
               // transform.position = target.position;
                
            }
            if (dashTime <= 0)
            {
                canDash = true;
                dashTime = 2f;
                target = GameObject.FindGameObjectWithTag("Player").transform; 
            }

        }
	}
}
