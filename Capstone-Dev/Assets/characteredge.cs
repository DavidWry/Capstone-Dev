using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
public class characteredge : MonoBehaviour {
    bool p = false;
   public float timehit = 0;
    bool onEdge = false;
    // Use this for initialization
    void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {

        if (timehit >= .5f&&gameObject.transform.parent.tag=="Boss")
        {

            p = false;
            gameObject.transform.parent.GetComponent<boss2charge>().chargetime = 4;
        }
        if (p)
        {
            timehit += Time.deltaTime;
        }
        else { timehit = 0; }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            GetComponentInParent<boss2behalf>().TakeDamage(other.GetComponent<Projectile>().Damage);
            Destroy(other.gameObject);
            
        }
         
         
        if (gameObject.name=="Pig")
        {
           
            if (other.tag == "edge" && gameObject.transform.parent.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "row2")
            {
                p = true;
                timehit += Time.deltaTime;
            }
            else if (other.name == "Cat" && other.GetComponent<characteredge>().onEdge == true)
            {
                p = true;

            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (gameObject.name=="Cat")
        {
            if (other.tag == "edge")
            {
                onEdge = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (gameObject.name=="Cat")
        {
            if (other.tag == "edge")
            {
                onEdge = false;
            }
        }
    }
}
