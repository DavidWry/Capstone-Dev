using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {


    private SpriteRenderer sprite;
    private bool activate;
    private bool hasLeft;
    private float showTime;

    private Color color;
	void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        activate = false;
        hasLeft = false;
        showTime = 3.0f;
        color = sprite.color;
        color.a = 0;
        sprite.color = color;  
	}
	
	// Update is called once per frame
	void Update ()
    {
        //has player entered the trigger
        if (activate == true)
        {
            color = sprite.color;
            color.a += (Time.deltaTime / 2);
            
            // showTime -= Time.deltaTime;
            sprite.color = color;

        }

        //player has entered and text has been fully shown
        if(color.a > 1)
        {
           color.a = 1;
            activate = false;
        }
       
        // destroy the trigger after player has exited and some time has passed.
        if (showTime <= 0)
        {
              Destroy(gameObject);
        }

        //if player has exited the trigger, fade the text 
        if (hasLeft == true)
        {
            color = sprite.color;
            color.a -= (Time.deltaTime / 2);
            showTime -= Time.deltaTime;
            sprite.color = color;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasLeft = true;
        }
    }
}
