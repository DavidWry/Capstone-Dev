using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    private float waitTime; //time before you start moving to the next position
    public float startWaitTime;

    public Transform[] moveSpots; //Patrol points
    private int randomSpot;
    private void Start()   
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length); //choose one random point
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed);

        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f) // how close the player should stop from the point
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
