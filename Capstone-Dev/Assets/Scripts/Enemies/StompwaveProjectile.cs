using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

public class StompwaveProjectile : MonoBehaviour {


    private Player_New player2;
    //private Player player2;
    private Transform player;
    private Vector3 target;
    private Vector3 initialPos;
    private float travelDistance;
    private int damage;
    private Scene scene;


    void Start()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.name == "2_1")
        {
            travelDistance = 56f;

        }
        else
        {
            travelDistance = 2.8f;
        }
        damage = 8;
        player2 = GetComponent<Player_New>();
       // player = GameObject.FindGameObjectWithTag("Player").transform;
       // target = new Vector3(player.position.x, player.position.y, player.position.z);
       
        initialPos = transform.position;
        //transform.rotation = Quaternion.LookRotation(target);
        // transform.LookAt(player.position);
    }

    private void Update()
    {

        //Once instantiated, head to the player's position
        //transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // transform.position += transform.right * speed * Time.deltaTime;
        
        if (Vector3.Distance(initialPos, transform.position) >= travelDistance)
        {
            DestroyProjectile();
        }


       /* if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();


        }*/

    }

    //Handle Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            DestroyProjectile();

        }

        if (other.gameObject.tag == "Player")
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
