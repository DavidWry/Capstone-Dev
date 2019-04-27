using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp { 
public class boss2charge : MonoBehaviour {
    int waitcount=0;
    public float chargetime = 0;
    public GameObject chargeSprite;
    Vector3 unitvec;
    float distance = 45;
        float temppos;
        bool nmh = false;
        public int counta = 0;
	// Use this for initialization
	void Start () {
        
	}

        // Update is called once per frame
        void Update() {

            if (nmh)
            {
                float p = (GameObject.FindGameObjectWithTag("Player").gameObject.transform.position - gameObject.transform.parent.gameObject.transform.position).x;
                if (p < 0)
                {
                    gameObject.transform.parent.transform.localScale = new Vector3(4, 4, 0);
                    gameObject.transform.parent.position = new Vector3(temppos, gameObject.transform.parent.transform.position.y, gameObject.transform.parent.transform.position.z);
                }
                else
                {
 
                    gameObject.transform.parent.transform.localScale = new Vector3(-4, 4, 0);
                    gameObject.transform.parent.position = new Vector3(temppos+70, gameObject.transform.parent.transform.position.y, gameObject.transform.parent.transform.position.z);

                }

            }
            
        if (waitcount > 3)
        {
            waitcount = 0;
            gameObject.GetComponent<Animator>().SetBool("canCharge", true);
            unitvec = (GameObject.FindGameObjectWithTag("Player").gameObject.transform.position-gameObject.transform.parent.gameObject.transform.position).normalized;
                
            }




        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "row2" )
        {

            if (chargetime < 3  )
            {
                   
                gameObject.GetComponentInParent<Rigidbody>().AddForce(unitvec*1000, ForceMode.Acceleration);
                chargetime += Time.deltaTime;
                if(gameObject.GetComponentInParent<boss2behalf>().hp>0)
                chargeSprite.SetActive(true);
                gameObject.GetComponent<Animator>().SetBool("canCharge", false);
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position, gameObject.transform.parent.gameObject.transform.position) < distance) {
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        player.GetComponent<Rigidbody>().AddForce(unitvec/10,ForceMode.Acceleration);
                        
                        player.GetComponent<Player_New>().TakeDamage(10*Time.deltaTime);
                        

                    }
                }
            else
            {
                chargeSprite.SetActive(false);
                gameObject.GetComponent<Animator>().SetBool("reach", true);
                   
                    chargetime = 0;
            }
        }
    

        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "row1")
        {
            chargeSprite.SetActive(false);
            gameObject.GetComponent<Animator>().SetBool("reach", false);
                gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        }



    }

    public void addwaitcount()
    {
        waitcount++;

    }

        public void facedirection()
        { if (counta == 0)
            {
                counta++;
                nmh = true;
                temppos = gameObject.transform.parent.transform.position.x - 35;
               
            }
 

    }


        public void resetCounta()
        {
            counta = 0;
            nmh = false;
        }
        private void OnTriggerEnter(Collider other)
    {
       


    }
        private void OnCollisionEnter(Collision collision)
        {
            print("nmsld");
        }

    }
}