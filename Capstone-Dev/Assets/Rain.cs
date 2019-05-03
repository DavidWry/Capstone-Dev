using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    private bool shouldMakeSound;
    private bool hasMadeSound;
    private float soundTime;
    // Use this for initialization
    void Start ()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 15f;
        
    }
	
	// Update is called once per frame
	void Update ()
    { 
        if (shouldMakeSound == true && soundTime == 15f)
        {
            SoundManager.PlaySound("Raining");
            shouldMakeSound = false;
        }
        soundTime -= Time.deltaTime;

        if(soundTime <= 0.0f)
        {
            shouldMakeSound = true;
            soundTime = 15f;
        }
        
       
    }
}
