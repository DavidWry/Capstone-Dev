using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyRangedSpear : MonoBehaviour
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

    private DropProbability probability = null;
    private GameManager gameManager = null;

    private Animator anim;
    private Scene scene;


    void Start ()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1")
        {
            rangeForAttack = 50;
            speed = 15f;
            chaseRange = 60;
        }
        else
        {
            rangeForAttack = 5;
            speed = 2.3f;
            chaseRange = 6;
        }
        health = 70;
        
    
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = 1.2f;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim = GetComponent<Animator>();

        //projectileArray = new GameObject[numProjectiles];
    }
	
	
	void Update ()
    {
        
        //Attack if under the range
        if (player != null)
        {

            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.position.x < transform.position.x)
            {
                scale.x *= -1;
            }
            transform.localScale = scale;


            if (Vector2.Distance(transform.position, player.position) <= rangeForAttack)
            {
                if (timeBetweenShots <= 0)
                {
                    anim.SetTrigger("Attack");
                    Instantiate(projectile, transform.position, Quaternion.identity);


                    timeBetweenShots = 1.5f;
                }
                else
                {
                    anim.SetBool("isRunning", false);
                    timeBetweenShots -= Time.deltaTime;
                }
            }
            else if (Vector2.Distance(transform.position, player.position) <= chaseRange && Vector2.Distance(transform.position, player.position) > rangeForAttack)
            {
                anim.SetBool("isRunning", true);
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
               
            }
            else
            {
                anim.SetBool("isRunning", false);
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



    public void TakeDamage(int damage)
    {
        health -= damage;

    }

}
