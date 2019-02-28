﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shield : MonoBehaviour {

    public float lifeTime;
    private float currentTime = 0;
    public int Damage = 5;
    float stun = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.parent)
        {
            transform.position = transform.parent.position;
        }
		if (currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            if (collision.gameObject.GetComponent<EnemySuicideBomber>())
            {
                collision.gameObject.GetComponent<EnemySuicideBomber>().TakeDamage(Damage);
                collision.gameObject.GetComponent<EnemySuicideBomber>().Stun(stun);
                Vector3 vector = collision.transform.position -transform.position;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(vector.normalized * 1000 * collision.transform.lossyScale.x);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedSpear>())
            {
                collision.gameObject.GetComponent<EnemyRangedSpear>().TakeDamage(Damage);
                collision.gameObject.GetComponent<EnemyRangedSpear>().Stun(stun);
                Vector3 vector = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(vector.normalized * 1000 * collision.transform.lossyScale.x);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedStomp>())
            {
                collision.gameObject.GetComponent<EnemyRangedStomp>().TakeDamage(Damage);
                collision.gameObject.GetComponent<EnemyRangedStomp>().Stun(stun);
                Vector3 vector = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(vector.normalized * 1000 * collision.transform.lossyScale.x);
            }
            else if (collision.gameObject.GetComponent<NewEnemyJumper>())
            {
                collision.gameObject.GetComponent<NewEnemyJumper>().TakeDamage(Damage);
                collision.gameObject.GetComponent<NewEnemyJumper>().Stun(stun);
                Vector3 vector = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(vector.normalized * 1000 * collision.transform.lossyScale.x);
            }
            else if (collision.gameObject.GetComponent<EnemySlider>())
            {
                collision.gameObject.GetComponent<EnemySlider>().TakeDamage(Damage);
                collision.gameObject.GetComponent<EnemySlider>().Stun(stun);
                Vector3 vector = collision.transform.position - transform.position;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(vector.normalized * 1000 * collision.transform.lossyScale.x);
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