using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

// Spears used by the Type 2 Minions
public class SpearProjectile : MonoBehaviour
{

    private float speed;
    private Player_New player2;
    //private Player player2;
    private Transform player;
    private Vector3 target;
    private int damage;
    private Scene scene;
    
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1"|| scene.name == "2_2")
        {
            speed = 60.5f;
        }
        else
        {
            speed = 2.75f;
        }
        damage = 8;
        player2 = GetComponent<Player_New>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y,player.position.z);
        
    }

    private void FixedUpdate()
    {

        //spin 
        
        //Once instantiated, head to the player's position
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.Rotate(Vector3.forward, Time.deltaTime * 270, Space.Self);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
       

        }
        
    }

    //Handle Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            DestroyProjectile();
            
        } 

        if (other.gameObject.tag =="Player")
        {
            
            DestroyProjectile();

            //Call TakeDamage function from the player's script
            other.gameObject.GetComponent<Player_New>().TakeDamage(damage);

        }

        
 
    }

    // Destroy the projectile
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
