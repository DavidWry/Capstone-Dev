﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private int health;
    private DropProbability probability = null;
    private GameManager gameManager = null;
    // Use this for initialization
    void Start () {
        health = 10;
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (health < 0)
        {
            string tempName = probability.DetermineDrop();
            GameObject itemObj = gameManager.GetItemObj(tempName);
            itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
            itemObj.transform.localScale = new Vector3(4, 4, 4);
            var worldCanvas = GameObject.Find("worldCanvas").transform;
            itemObj.transform.parent = worldCanvas;
            itemObj.transform.position = transform.position;
            Destroy(gameObject);
        }
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}