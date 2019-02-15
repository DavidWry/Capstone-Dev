using UnityEngine;
using UnityEngine.SceneManagement;



public class EnemyRangedStomp : MonoBehaviour
{

    public int numberOfProjectiles;             // Number of projectiles to shoot.
    public float projectileSpeed;               // Speed of the projectile.
    public GameObject ProjectilePrefab;         // Prefab to spawn.


    private Vector3 startPoint;                 // Starting position of the stomp.
    private const float radius = 1F;            // Help us find the move direction.

    private int health;
    private float attackRange;
    private float chaseRange;
    public float speed;
    private Transform target;


    private float timeBetweenShots;
    

    private DropProbability probability = null;
    private GameManager gameManager = null;

    private Animator anim;
    private Scene scene;


    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1")
        {
            attackRange = 40f;
            chaseRange = 60f;
        }
        else
        {
            attackRange = 4f;
            chaseRange = 6;
        }
        health = 80;
       
        
        timeBetweenShots = 1.2f;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim = GetComponent<Animator>();
       

    }

    // Update is called once per frame
    void Update()
    {

        

        //behavior here
        if (target != null)
        {

            //face the player
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (target.position.x < transform.position.x)
            {
                scale.x *= -1;
            }
            transform.localScale = scale;


            startPoint = transform.position;

            if (Vector3.Distance(startPoint, target.position) <= attackRange)
            {
                if (timeBetweenShots <= 0)
                {
                    anim.SetTrigger("Attack");
                    
                    SpawnProjectile(numberOfProjectiles);
                    timeBetweenShots = 1.6f;

                }
                else
                {
                    anim.SetBool("isRunning", false);
                    timeBetweenShots -= Time.deltaTime;
                }

            }
            else if (Vector2.Distance(transform.position, target.position) <= chaseRange && Vector2.Distance(transform.position, target.position) > attackRange)
            {
                anim.SetBool("isRunning", true);

                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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

    // Spawns x number of projectiles.
    private void SpawnProjectile(int _numberOfProjectiles)
    {
        float angleStep = 360f / _numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * projectileSpeed;

            // Create game objects.
            GameObject tmpObj = Instantiate(ProjectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);

            // Destory the gameobject after 10 seconds.
            Destroy(tmpObj, 3F);

            angle += angleStep;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
