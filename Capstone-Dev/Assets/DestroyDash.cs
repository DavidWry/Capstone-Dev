using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDash : MonoBehaviour {

    
   // public GameObject enemy;
    private EnemySlider es;
    private bool canDash;
    
	void Start ()
    {

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        canDash = GameObject.Find("Slider").GetComponent<EnemySlider>().hasReached;

        if (canDash == true)
        {

            Destroy(gameObject, 0.5f);

        }
        
	}
}
