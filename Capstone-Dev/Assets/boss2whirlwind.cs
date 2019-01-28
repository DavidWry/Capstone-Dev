using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2whirlwind : MonoBehaviour {

    public GameObject windParticle;
    public ParticleSystem windParticleSystem;
    public Animator bossAnimator;
    float timeCount=0;
    Vector3 nextSpot;
    // Use this for initialization
    void Start()
    {
        bossAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (windParticleSystem.isPlaying)
        {
            if (timeCount == 0)
            {
                nextSpot = new Vector3(0, 0, 0);
               
               timeCount = 1;
            }
           
            else
            {
                Vector3 addition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0) * Time.deltaTime;
                print(addition);
                nextSpot += addition;
               
                gameObject.transform.parent.transform.position += nextSpot;

            }


        }
        else
        {
            nextSpot = new Vector3(0, 0, 0);

            timeCount = 0;
        }
    }

    public void whrilStart()
    {
        windParticleSystem.Play();

    }

    public void whrilEnd()
    {
        windParticleSystem.Stop();

    }


    void OnTriggerEnter(Collider other)
    {
        if (windParticleSystem.isPlaying&&other.tag != "Player")
        {
            nextSpot *= -1;
        }

    }

}
