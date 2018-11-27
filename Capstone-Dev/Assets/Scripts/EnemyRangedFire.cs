using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedFire : MonoBehaviour {

    private float rangeForAttack; //Within what range the enemy will start and continue attacking the player

    private float timeBetweenShots;
    private float startTimeBetweenShots;

    public GameObject projectile;
    private Transform player;
    void Start ()
    {
        rangeForAttack = 7;
        startTimeBetweenShots = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
