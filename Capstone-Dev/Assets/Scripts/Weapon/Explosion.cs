﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int Damage = 1;
    public float speed = 0.1f;
    public float Life = 0.5f;
    private float lifetime = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * speed;
        lifetime += Time.deltaTime;
        if (lifetime > Life)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            if (collision.gameObject.GetComponent<EnemySuicideBomber>())
            {
                collision.gameObject.GetComponent<EnemySuicideBomber>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedSpear>())
            {
                collision.gameObject.GetComponent<EnemyRangedSpear>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedStomp>())
            {
                collision.gameObject.GetComponent<EnemyRangedStomp>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<NewEnemyJumper>())
            {
                collision.gameObject.GetComponent<NewEnemyJumper>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemySlider>())
            {
                collision.gameObject.GetComponent<EnemySlider>().TakeDamage(Damage);

            }
        }
        else if (collision.gameObject.tag == "Chest")
        {
            collision.GetComponent<Chest>().TakeDamage(Damage);
        }
        else if (collision.gameObject.tag == "Dummy")
        {
            collision.GetComponent<Dummy>().TakeDamage(Damage);
        }
        else if (collision.gameObject.tag == "Boss")
        {
            collision.GetComponent<Fsmandhp>().takedamage(Damage);
        }
    }

}
