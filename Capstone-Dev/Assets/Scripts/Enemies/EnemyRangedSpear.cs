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
    public float health;
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

    private CapsuleCollider capsule;
    private float addTime;

    private bool isDrop;
    
    private void Awake()
    {

        scene = SceneManager.GetActiveScene();
        capsule = gameObject.GetComponent<CapsuleCollider>();
        if (scene.name == "2_1"|| scene.name == "2_2" || scene.name == "3_1" || scene.name == "3_2")
        {
            transform.localScale = new Vector3(5f, 5f, 1f);
            rangeForAttack = 125;
            speed = 50.6f;
            chaseRange = 176;
            capsule.radius = 1.88f;
            capsule.height = 5.73f; 
            addTime = 0.4f;

            projectile.transform.localScale = new Vector3(5f, 5f, 1f);
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.23f, 0.23f, 1f);
            rangeForAttack = 7.3f;
            speed = 2.4f;
            chaseRange = 9f;
            capsule.radius = 0.45f;
            capsule.height = 5.73f;
            addTime = 0.0f;
            projectile.transform.localScale = new Vector3(0.23f, 0.23f, 1f);

        }
    }
    void Start()
    {

        isDrop = false;
        health = 65;
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


    void Update()
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


                        timeBetweenShots = 1.0f + addTime;
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
                anim.SetTrigger("hasDied");
                Destroy(gameObject, 0.75f);

                if (probability && !isDrop)
                {
                    //drop item
                    string tempName = probability.DetermineDrop();
                    GameObject itemObj = gameManager.GetItemObj(tempName);
                    itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || scene.name == "3_1" || scene.name == "3_2")
                        itemObj.transform.localScale = new Vector3(90, 90, 1);
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                    isDrop = true;
                }

                //Instantiate(crystal, transform.position,Quaternion.identity);
            }
            if (health >= 65)
            {
                health = 65;
            }

            if (currentHealth >= 65)
            {
                currentHealth = 65;
            }

        }

    }

   /* private void OnCollisionEnter(Collision other)
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
    }*/



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;

    }

    public void RainHealthUpdate(float amount)
    {
        currentHealth += amount;
        healthBar.fillAmount = currentHealth / health;
    }

    public void Stun(float stunTime)
    {


        rb.velocity = Vector3.zero;
        isStunned = true;
        anim.SetBool("isRunning", false);
        timeBetweenShots = 1.5f + addTime;
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
