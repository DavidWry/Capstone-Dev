using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyRangedSpear : MonoBehaviour
{
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private float chaseRange;

    public float speed;
    private float timeBetweenShots; 
    private float startTimeBetweenShots;

    private float currentHealth;
    private float health;
    public GameObject projectile;
    
   
    public GameObject crystal;
    private Transform player;

    private DropProbability probability = null;
    private GameManager gameManager = null;

    private Animator anim;
    private Scene scene;

    public Image healthBar;

    private Rigidbody rb;
    private bool isStunned;
    private SpriteRenderer myRenderer;
    private Color color;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1")
        {
            transform.localScale = new Vector3(5f, 5f, 1f);
            rangeForAttack = 110;
            speed = 50.6f;
            chaseRange = 132;
            projectile.transform.localScale = new Vector3(5f, 5f, 1f);
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.23f, 0.23f, 1f);
            rangeForAttack = 5;
            speed = 2.3f;
            chaseRange = 6;
            projectile.transform.localScale = new Vector3(0.23f, 0.23f, 1f);
        }
    }
    void Start ()
    {
        

        health = 70;
        currentHealth = health;
    
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = 1.2f;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim = GetComponent<Animator>();

        rb = gameObject.GetComponent<Rigidbody>();
        isStunned = false;
        myRenderer = GetComponent<SpriteRenderer>();
        color = myRenderer.color;

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
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
            }
            else
            {
                healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
            }
            transform.localScale = scale;

            if (isStunned == false)
            {
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
            }
            

            if (currentHealth <= 0)
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

    private void OnCollisionEnter(Collision other)
    {
  
        if ((other.gameObject.tag == "Minion") || (other.gameObject.tag == "Obstacle"))
        {
            myRenderer.color = Color.red;
            rb.velocity = Vector3.zero;
            isStunned = true;
            anim.SetBool("isRunning", false);
            timeBetweenShots = 1.5f;
            StartCoroutine(WaitAfterStun(3f));
           

        }
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;

    }

    public void Stun(float stunTime)
    {

       
        rb.velocity = Vector3.zero;
        isStunned = true;
        anim.SetBool("isRunning", false);
        timeBetweenShots = 1.5f;
        StartCoroutine(WaitAfterStun(stunTime));
       

    }
    private IEnumerator WaitAfterStun(float time)
    {

        yield return new WaitForSeconds(time);
        isStunned = false;
        timeBetweenShots = 0f;
        myRenderer.color = color;
    }

}
