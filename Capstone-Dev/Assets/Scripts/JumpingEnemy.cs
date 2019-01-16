using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpingEnemy : MonoBehaviour
{
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private float chaseRange;

    public float speed;
    private float timeBetweenJumps;
    private float startTimeBetweenJumps;
    private int health;

   
    public GameObject crystal;
    private Transform player;
    public GameObject AreaDamage;
    private DropProbability probability = null;
    private GameManager gameManager = null;

    void Start()
    {
        speed = 2.5f;
        rangeForAttack = 5;
        chaseRange = 8;
        startTimeBetweenJumps = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenJumps = startTimeBetweenJumps;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }


    void Update()
    {
        //Attack if under the range
        if (player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= rangeForAttack)
            {
                if (timeBetweenJumps<=0)
                {
                    Instantiate(AreaDamage, player.transform.position, Quaternion.identity);

                    timeBetweenJumps = startTimeBetweenJumps;
                }
                else
                {
                    timeBetweenJumps -= Time.deltaTime;
                }
            }

            if (Vector2.Distance(transform.position, player.position) <= chaseRange && Vector2.Distance(transform.position, player.position) > rangeForAttack)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if (health <= 0)
            {
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
                Destroy(gameObject);
                //Instantiate(crystal, transform.position,Quaternion.identity);
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
