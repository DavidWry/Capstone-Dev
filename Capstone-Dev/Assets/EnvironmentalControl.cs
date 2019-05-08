using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalControl : MonoBehaviour {

    // Use this for initialization

    public GameObject largeWind;
    public GameObject shortWind;
    public GameObject rain;

    public float waitTime;
    private int num;
    public float effectTime;
    void Start ()
    {
        waitTime = 10f;
        num = 1;
        effectTime = 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        waitTime -= Time.deltaTime;

        if (waitTime <= 0.0f)
        {
           
            num = Random.Range(1, 4);
            if (num == 1)  // Rain
            {
                rain.SetActive(true);
                largeWind.SetActive(false);
                shortWind.SetActive(false);
                effectTime = 15f;

            }
            else if (num == 2)  // Large Wind
            {
                largeWind.SetActive(true);
                rain.SetActive(false);
                shortWind.SetActive(false);
                effectTime = 20f;
            }
            else if (num == 3)  // Short Wind
            {
                shortWind.SetActive(true);
                largeWind.SetActive(false);
                rain.SetActive(false);
                effectTime = 16f;
            }

            waitTime = 12f;
            waitTime += effectTime;
        }
       
    }

 
}
