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
    private Transform player;

    private DropProbability probability = null;
    private GameManager gameManager = null;

    void Start ()
    {
        health = 20;
        speed = 3;
        rangeForAttack = 6;
        chaseRange = 10;
        startTimeBetweenShots = 2;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                //drop item
                string tempName = probability.DetermineDrop();
                GameObject itemObj = gameManager.GetItemObj(tempName);
                itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                itemObj.transform.localScale = new Vector3(4, 4, 4);
                var worldCanvas = GameObject.Find("worldCanvas").transform;
                itemObj.transform.parent = worldCanvas;
                Destroy(gameObject);
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
