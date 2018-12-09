using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fsmandhp : MonoBehaviour {

    public float hp = 100;
    public GameObject jiguang;
    public GameObject toufazuo;
    public GameObject toufayou;
    private GameObject a;
    public float DistanceBP;
    public float DistanceBO;
    private GameObject player;
    public float range=100;
    public Vector3 originalpos;
    public bool supposetomove=false;
    private int yuancount=0;
    private float remainingtime;
   private Animator anim;
    private bool wander = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        a = gameObject;
        anim = a.GetComponent<Animator>();
        originalpos = gameObject.transform.parent.transform.position;
        remainingtime = Random.Range(5.0f, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {

        if (hp <= 30 && yuancount == 1)
        {
           
            yuancount++;
            supposetomove = true;


        }
       else if (hp <= 50 && yuancount == 0)
        {
            
            yuancount++;
            supposetomove = true;



        }








        if (!supposetomove)
        {
            DistanceBP = Vector3.Distance(gameObject.transform.parent.transform.position, player.transform.position);
            DistanceBO = Vector3.Distance(gameObject.transform.parent.transform.position, originalpos);
           
         
            if (DistanceBP < range && DistanceBO < range)
            {
                if (anim.GetInteger("stage")==-1) {
                    anim.SetInteger("stage", 0);
                }

                if (!wander)
                {

                    remainingtime -= Time.deltaTime;

                }
                else
                {


                }


                if (remainingtime < 0)
                {
                    remainingtime = Random.Range(5, 10);
                    wander = true;


                }





                if (hp < 50)
                {
                    int randomNum = (int)Random.Range(1, 6);
                    anim.SetInteger("stage", randomNum);


                }

                else
                {
                    int randomNum = (int)Random.Range(1, 5);
                    anim.SetInteger("stage", randomNum);


                }
            }


            else
            {   
                anim.SetInteger("stage", -1);
              
                if (DistanceBO > 2)
                {
                    
                    Vector3 backvec=originalpos - this.transform.parent.transform.position;
                    backvec = backvec.normalized;
                    gameObject.transform.parent.GetComponent<Rigidbody>().MovePosition(gameObject.transform.parent.transform.position+backvec);

                }

            }



        }
        

	}

    public void instjiguang()
    {

        jiguang.GetComponent<Instlightbeam>().resetjg();

    }
    public void insttoufa()
    {
  
        if (hp > 30) {
            int randoma = (int)Random.Range(0, 3);
           
            if (randoma == 0) {
                toufazuo.SetActive(true);
                    
                    
                    }
            if (randoma == 1) {
                toufayou.SetActive(true);
            }
            if (randoma == 2)
            {
                toufazuo.SetActive(true);
                toufayou.SetActive(true);
                
                    

            }
            print(randoma);

        }
        else
        {
            toufazuo.SetActive(true);
            toufayou.SetActive(true);


        }

     


    }
    

}
