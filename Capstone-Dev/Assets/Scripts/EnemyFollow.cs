using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Follow the player everywhere
public class EnemyFollow : MonoBehaviour
{

    public float speed;

    private Transform target;

    void Start()
    {
        //Set player as the target
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > 1)
        {
            //move enemy to the player's position
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
    }

}
