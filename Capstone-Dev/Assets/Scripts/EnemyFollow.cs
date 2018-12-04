using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

// Follow the player everywhere
public class EnemyFollow : MonoBehaviour
{
    private int health;
    private float speed;
    private Player player2;
    private int damage;
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private Transform target;

    private DropProbability probability = null;
    private GameManager gameManager = null;

    void Start()
    {
        //Set player as the target
        health = 100;
        speed = 1.0f ;
        player2 = GetComponent<Player>();
        rangeForAttack = 6;
        damage = 5;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Attack player if its under the range
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) <= rangeForAttack && Vector2.Distance(transform.position, target.position) > 0.5)
            {
                //move enemy to the player's position
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }

            if (health <= 0)
            {
                //drop item
                string tempName = probability.DetermineDrop();
                GameObject itemObj = gameManager.GetItemObj(tempName);
                itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                if (NextScene.nowName == "2_1")
                    itemObj.transform.localScale = new Vector3(4, 4, 4);
                var worldCanvas = GameObject.Find("worldCanvas").transform;
                itemObj.transform.parent = worldCanvas;
                Destroy(gameObject);
            }
        }
        
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
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

    //Handle damage taken
    public void TakeDamage(int damage)
    {
        health -= damage; 

    }

}
