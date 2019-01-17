using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour {

    private int health;
    private float attackRange;
    public GameObject landingArea;
    private Transform player;
    Vector2 startPos;
    Vector2 controlPoint;
    Vector2 endPos;
	void Start ()
    {
        health = 100;
        attackRange = 5f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        endPos = player.position;
        startPos = transform.position;

        controlPoint.x = startPos.x - (startPos.x - endPos.x) / 2;
        controlPoint.y = startPos.y - (startPos.y - endPos.y) / 2;
    }
	
	void Update ()
    {
		
	}
}
