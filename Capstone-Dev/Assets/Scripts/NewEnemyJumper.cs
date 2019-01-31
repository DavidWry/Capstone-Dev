using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyJumper : MonoBehaviour
{
   
    private float rangeForAttack;
    private float waitTime;
    //private float startWaitTime;

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
    public GameObject impact;
    private bool canJump;
    Vector3 newTargetPos;

    private Animator anim;
    void Start()
    {
        health = 55;
        rangeForAttack = 7;

        
        waitTime = 3f;
        canJump = true;

        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        anim = GetComponent<Animator>();

    }

    void Update()
    {

        if (player !=null)
        {
            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.position.x < transform.position.x)
            {
                scale.x *= -1;
            }
            transform.localScale = scale;

            //initial states for jumping
            if ((Vector3.Distance(startPos, player.position) <= rangeForAttack) && (canJump == true))
            {
                
                targetPos = new Vector3(player.position.x, player.position.y, player.position.z);

                startPos = transform.position;
                anim.SetBool("isJumping", true);
                Instantiate(landing, targetPos, Quaternion.identity);
                
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
          
            if (Vector3.Distance(nextPos,targetPos)<=0.1f)
           // if(nextPos == targetPos)
            {
                waitTime -= Time.deltaTime;
                canJump = false;
                Instantiate(impact, targetPos, Quaternion.identity);
                anim.SetBool("isJumping", false);
            }

           

            if (waitTime <= 0)
            {
                canJump = true;
                targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
                waitTime = 2.0f;
            }

           


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
    public void TakeDamage(int damage)
    {
        health -= damage;

    }
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }




}



/// 
/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
/// that makes the local +X axis point in the given forward direction.
/// 
/// forward direction
/// Quaternion that rotates +X to align with forward



