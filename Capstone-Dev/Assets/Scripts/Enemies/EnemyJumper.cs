using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour {

    private int health;
    private float attackRange;
    public GameObject landingArea;
    private Transform startPos;
    private Transform endPos;
    public float journeyTime=1.0f;
    public float speed;
    private Vector3 target;
   
    //private Transform player;


    private float startTime;
    Vector3 centerPoint;
    Vector3 startRelCenter;
    Vector3 endRelCenter;
	void Start ()
    {
        health = 100;
        attackRange = 5f;
        startPos = gameObject.transform;
        endPos = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(endPos.position.x, endPos.position.y, endPos.position.z);
        

    }
	
	void Update ()
    {
       // if (transform.position.x == target.x && transform.position.y == target.y )
       

        if (Vector3.Distance(startPos.position,target) <= attackRange)
        {
           
                GetCenter(Vector3.up);
                float fracComplete = (Time.time - startTime) / journeyTime * speed;
                transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
                transform.position += centerPoint;
          
            
        }
        /*if (Vector3.Distance(startPos.position, endPos.position) <= 1.5)
        {
            Destroy(gameObject);
        }*/

            if (health <= 0)
        {
            Destroy(gameObject);
        }
        



    }
    private void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos.position + target) * 0.5f;
        centerPoint -= direction;
        startRelCenter = startPos.position - centerPoint;
        endRelCenter = target - centerPoint;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

    }
}
