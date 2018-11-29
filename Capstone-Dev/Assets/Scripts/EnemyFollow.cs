using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follow the player everywhere
public class EnemyFollow : MonoBehaviour
{
    public int health;
    public float speed;
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private Transform target;

    void Start()
    {
        //Set player as the target
        health = 100;
        rangeForAttack = 4;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // Attack player if its under the range
        if (Vector2.Distance(transform.position, target.position) <= rangeForAttack && Vector2.Distance(transform.position, target.position) > 0.5)
        {
            //move enemy to the player's position
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        if (health == 0)
        {
            Destroy(gameObject);
        }
        
    }
 
    private void OnTriggerEnter(Collider other)
    {
        // if hit then move to the opposite direction to show a PUSHBACK effect
        if (other.CompareTag("Projectile"))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-transform.right * 2);
            gameObject.GetComponent<Rigidbody>().velocity = Vector2.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, 1);
            Debug.Log("000");
        }
    }

    //Handle damage taken
    public void TakeDamage(int damage)
    {
        health -= damage; 

    }

}
