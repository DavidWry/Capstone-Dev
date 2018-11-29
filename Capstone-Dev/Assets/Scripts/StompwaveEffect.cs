using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompwaveEffect : MonoBehaviour {

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;
    private float timeElapsed;
    public GameObject trail;
    private Transform rocks;
    private Vector2 target;
    void Start()
    {
        rocks = GameObject.FindGameObjectWithTag("Stompwave").transform;
        target = new Vector2(rocks.position.x, rocks.position.y);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeBtwSpawns <= 0)
        {
            Instantiate(trail, transform.position, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;
        }
      
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }


    }
}
