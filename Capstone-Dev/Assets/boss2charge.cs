using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2charge : MonoBehaviour {
    int waitcount=0;
    float chargetime = 0;
    public GameObject chargeSprite;
    Vector3 unitvec;
	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update() {
        if (waitcount > 3)
        {
            waitcount = 0;
            gameObject.GetComponent<Animator>().SetBool("canCharge", true);
            unitvec = (GameObject.FindGameObjectWithTag("Player").gameObject.transform.position-gameObject.transform.parent.gameObject.transform.position).normalized;
        }



        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "row2" )
        {

            if (chargetime < 3 || Vector3.Distance(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position, gameObject.transform.parent.gameObject.transform.position) < 5)
            {
                gameObject.GetComponentInParent<Rigidbody>().MovePosition(gameObject.transform.parent.gameObject.transform.position + unitvec*Time.deltaTime*50);
                chargetime += Time.deltaTime;
                chargeSprite.SetActive(true);
                gameObject.GetComponent<Animator>().SetBool("canCharge", false);
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

        }



    }

    public void addwaitcount()
    {
        waitcount++;

    }

    public void facedirection()
    {
        float p=(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position - gameObject.transform.parent.gameObject.transform.position).x;
        if (p < 0) {
            gameObject.transform.parent.transform.rotation = Quaternion.Euler(0, 180, 0);
                
                }
        else
        {
            gameObject.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "row2"&&other.gameObject.tag!="Player")
        {
            unitvec *= -1;
            if (gameObject.transform.parent.transform.rotation.y == 180)
            {
                gameObject.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else { gameObject.transform.parent.transform.rotation = Quaternion.Euler(0, 180, 0); }


        };


    }

}
