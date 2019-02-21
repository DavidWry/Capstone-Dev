using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AssemblyCSharp;


// Follow the player everywhere
public class EnemySuicideBomber : MonoBehaviour
{
    private int health;
    private float currentHealth;
    private float speed;
    private Player_New player2;
    //private Player player2;
    private int damage;
     //Within what range the enemy will start and continue attacking the player
    private Transform target;
    private float rangeForAttack;

    private DropProbability probability = null;
    private GameManager gameManager = null;
    public GameObject explosion;

    private Animator anim;

    private float distanceForColor;
    private Color startColor = Color.red;
    private Color endColor = Color.yellow;
    private Color defaultColor;
    private SpriteRenderer myRenderer;

    private Scene scene;

    public Image healthBar;

    
    private Rigidbody rb;

    private bool isStunned;

    private void Awake()
    {
        //Handle size for 2 different levels
        scene = SceneManager.GetActiveScene();

        if (scene.name == "2_1")
        {

            transform.localScale = new Vector3(5f, 5f, 1f);
            speed = 41.0f;
            rangeForAttack = 140;
            explosion.transform.localScale = new Vector3(40f, 40f, 1f);
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            speed = 2.1f;
            rangeForAttack = 7;
            explosion.transform.localScale = new Vector3(2f, 2f, 1f);
        }
    }
    void Start()
    {
        //Set player as the target
       
        health = 51;
        currentHealth = health;
        player2 = GetComponent<Player_New>();

        rb = gameObject.GetComponent<Rigidbody>();
        isStunned = false;
        
        damage = 5;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim = GetComponent<Animator>();

        myRenderer = GetComponent<SpriteRenderer>();
        defaultColor = myRenderer.material.color;

    }
   
    void Update()
    { 
        // Attack player if its under the range
        if (target != null)
        {
            //face the player
            var scale = transform.localScale;
          

            scale.x = Mathf.Abs(scale.x);
      
            if (target.position.x < transform.position.x)
            {
                scale.x *= -1;
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;

            }
            else
            {
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
            }
            transform.localScale = scale;

            if (!isStunned && currentHealth > 0)
            {

                distanceForColor = Vector2.Distance(target.position, transform.position);

                if (Vector2.Distance(transform.position, target.position) <= rangeForAttack && Vector2.Distance(transform.position, target.position) > 0.5)
                {
                    //move enemy to the player's position
                    anim.SetBool("isRunning", true);
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    myRenderer.material.color = Color.Lerp(startColor, endColor, distanceForColor / rangeForAttack);

                }
                else
                {
                    anim.SetBool("isRunning", false);
                    myRenderer.material.color = defaultColor;
                }
            }


            if (currentHealth <= 0)
            {
                
                //drop crystl
                if (probability)
                {
                    string tempName = probability.DetermineDrop();
                    GameObject itemObj = gameManager.GetItemObj(tempName);
                    itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    if (NextScene.nowName == "2_1")
                        itemObj.transform.localScale = new Vector3(4, 4, 4);
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                }
                Destroy(gameObject);
            }

        }


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {


            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
            other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
            Destroy(expl, 3);

        }

        //handle collision with other enemies or obstacles
        /*
        if( (other.gameObject.tag == "Minion") || (other.gameObject.tag == "Obstacle"))
        {
            isStunned = true;
            rb.velocity = Vector3.zero;
            anim.SetBool("isRunning", false);
            StartCoroutine(WaitAfterStun(3f));
            myRenderer.material.color = defaultColor;
            

        }*/
    }


    //Handle damage taken
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;
    }

    //stun the enemy
    public void Stun(float stunTime)
    {
        
        isStunned = true;
        rb.velocity = Vector3.zero;
        anim.SetBool("isRunning", false);
        StartCoroutine(WaitAfterStun(stunTime));
      
    }
    private IEnumerator WaitAfterStun(float time)
    {

        yield return new WaitForSeconds(time);
        isStunned = false;
    }


}
