using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyRanged : MonoBehaviour
{
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private float chaseRange;

    public float speed;
    private float timeBetweenShots; 
    private float startTimeBetweenShots;
    private float health;

    public GameObject projectile;
    public GameObject crystal;
    private Transform player;

    void Start ()
    {
        if (gameObject.name == "Minion_Type_2")
        {
            health = 120;
        }
        else
        {
            health = 150;
        }
         
        speed = 2.5f;
        rangeForAttack = 5;
        chaseRange = 8;
        startTimeBetweenShots = 2.0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
	}
	
	
	void Update ()
    {
        //Attack if under the range
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= rangeForAttack)
            {
                if (timeBetweenShots <= 0)
                {
                    Instantiate(projectile, transform.position, Quaternion.identity);
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
                Destroy(gameObject);
                Instantiate(crystal, transform.position,Quaternion.identity);
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
