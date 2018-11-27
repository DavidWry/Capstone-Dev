using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player
    private float chaseRange;
    private float timeBetweenShots; 
    private float startTimeBetweenShots; 

    public GameObject projectile;
    private Transform player;

    void Start ()
    {
        rangeForAttack = 6;
        chaseRange = 8;
        startTimeBetweenShots = 2;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
	}
	
	
	void Update ()
    {
        //Attack if under the range
        if(Vector2.Distance(transform.position, player.position) <= rangeForAttack)
        {
            if (timeBetweenShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBetweenShots = startTimeBetweenShots;
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }
        }
        else if()

        
	}
}
