using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyJumper : MonoBehaviour
{
   
    private float rangeForAttack;
    private float timeBetweenJumps;
    private float startTimeBetweenJumps;

    private float health;

    private DropProbability probability = null;
    private GameManager gameManager = null;
    //public GameObject crystal;

    Vector3 startPos;
    Vector3 nextPos;
    private Transform player;
    private Vector3 targetPos;
    public float speed = 10;
    public float arcHeight = 1;

    public GameObject landing;

    void Start()
    {
        health = 55;
        rangeForAttack = 7;

        startTimeBetweenJumps = 2.5f;
        timeBetweenJumps = startTimeBetweenJumps - 1f;

        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetPos = new Vector3(player.position.x, player.position.y, player.position.z);

    }

    void Update()
    {

        if (player !=null)
        {
          
            if (Vector3.Distance(transform.position, player.position) <= rangeForAttack)
            {
                //  if (timeBetweenJumps <= 0)

                //if (isJumping)
                // {
              //  Instantiate(landing, targetPos, Quaternion.identity);
                    float x0 = startPos.x;
                    float x1 = targetPos.x;
                    float dist = x1 - x0;
                    float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
                    float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
                    float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
                    nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

                    // Rotate to face the next position, and then move there
                    // transform.rotation = LookAt2D(nextPos - transform.position);
                    transform.position = nextPos;
                   
                     // timeBetweenJumps = startTimeBetweenJumps;
                //}

            }
            if (nextPos == targetPos)
            {
               // timeBetweenJumps = 2.0f;
            }


               // else
               // {
                   // if ((nextPos == targetPos))
                   // {
                     //   timeBetweenJumps -= Time.deltaTime;
                   // }
                
              //  }
                
                    
                    
               
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

        // Compute the next position, with arc added in
       

        // Do something when we reach the target
       // if (nextPos == targetPos) Arrived();*/
       
    }

   /* void Arrived()
    {
        Destroy(gameObject);
    }

    /// 
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// forward direction
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

    }
}*/
