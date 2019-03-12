using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDash : MonoBehaviour {

    
    public GameObject enemy;
    private EnemySlider es;
    private bool canDash;
    
	void Start ()
    {
        // es = GameObject.Find("EnemySlider").GetComponent<EnemySlider>();
        // es = GetComponent<EnemySlider>();


        //   es = enemy.GetComponent<EnemySlider>();
        // canDash = es.hasReached;
       // enemy = GameObject.Find("Slider");
       

    }
	
	// Update is called once per frame
	void Update ()
    {
        //  if (enemy != null)
        // {

      //  es = enemy.GetComponent<EnemySlider>();

        //canDash = GameObject.Find("Slider").GetComponent<EnemySlider>().hasReached;

        /* if (canDash == true)
         {
             Destroy(gameObject, 0.5f);
         }*/
        /*if(enemy.GetComponent<EnemySlider>().hasReached == true)
        {
            Destroy(gameObject, 0.5f);
        }*/

      /*  if (es.canDash == true)
        {
            Destroy(gameObject, 0.5f);
        }*/
            
       // }
        
        
	}

    public void DestroyD()
    {
        Destroy(gameObject, 0.5f);
    }
}
