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
 


    void Start()
    {
        //Set player as the target
        scene = SceneManager.GetActiveScene();
    
        if (scene.name == "2_1")
        {

            transform.localScale = new Vector3(5f, 5f, 1f);
            speed = 40.0f;
            rangeForAttack = 120;
            explosion.transform.localScale = new Vector3(40f, 40f, 1f);
        }
        else if (scene.name == "First Level")
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            speed = 2.0f;
            rangeForAttack = 6;
            explosion.transform.localScale = new Vector3(2f, 2f, 1f);
        }

        health = 51;
        currentHealth = health;
        player2 = GetComponent<Player_New>();

        
        
        damage = 5;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        defaultColor = myRenderer.material.color;

    }
    private void Awake()
    {

    }
    void Update()
    {

       

        // Attack player if its under the range
        if (target != null)
        {
            //face the player
            var scale = transform.localScale;
           // var hBarScale = healthBar.transform.localScale;

            scale.x = Mathf.Abs(scale.x);
            //   hBarScale.x = Mathf.Abs(hBarScale.x);
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
         //   healthBar.transform.localScale = hBarScale;


            distanceForColor = Vector2.Distance(target.position, transform.position);
            
            if (Vector2.Distance(transform.position, target.position) <= rangeForAttack && Vector2.Distance(transform.position, target.position) > 0.5)
            {
                //move enemy to the player's position
                anim.SetBool("isRunning", true);
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                myRenderer.material.color = Color.Lerp(startColor, endColor, distanceForColor/rangeForAttack );
                

            }
            else
            {
                anim.SetBool("isRunning", false);
                myRenderer.material.color = defaultColor;
            }

            if (currentHealth <= 0)
            {
                //drop item
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
    }


    //Handle damage taken
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / health;
    }

}
