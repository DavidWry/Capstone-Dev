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
    public float range=20;
    public Vector3 originalpos;
    public bool supposetomove=false;
    private int yuancount=0;
    public float remainingtime;
   private Animator anim;
    private bool walk = false;
    bool notyet = false;
    private Vector3 nexspot;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        a = gameObject;
        anim = a.GetComponent<Animator>();
        originalpos = gameObject.transform.parent.transform.position;
        remainingtime = Random.Range(3.0f, 5.0f);
        nexspot = new Vector3(player.transform.position.x + Random.Range(-10.0f, 10.0f), player.transform.position.y + Random.Range(-10.0f, 10.0f), 0);
    }
	
	// Update is called once per frame
	void Update () {
       
        if (hp <= 150 && yuancount == 1)
        {
           
            yuancount++;
            supposetomove = true;
            remainingtime = Random.Range(3.0f, 5.0f);
            walk = false;
        }
       else if (hp <= 250 && yuancount == 0)
        {
            
            yuancount++;
            supposetomove = true;
            remainingtime = Random.Range(3.0f, 5.0f);
            walk = false;


        }








        if (!supposetomove)
        {
            
            DistanceBP = Vector3.Distance(gameObject.transform.parent.transform.position, player.transform.position);
            DistanceBO = Vector3.Distance(gameObject.transform.parent.transform.position, originalpos);
 
         
            if (DistanceBP < range && DistanceBO < range&&!notyet)
            {
                 
                
                if (anim.GetInteger("stage")==-1) {
                    anim.SetInteger("stage", 0);
                }
                
                if (!walk)
                {
                    
                    remainingtime -= Time.deltaTime;
                   

                }
                else
                { float distancebtemp = Vector3.Distance(gameObject.transform.parent.transform.position, nexspot);
                    
                    

                    if (distancebtemp>2) {
                        gameObject.transform.parent.GetComponent<Rigidbody>().MovePosition(gameObject.transform.parent.transform.position+(nexspot - gameObject.transform.parent.transform.position).normalized*Time.deltaTime*10);
                   
                    }
                    else
                    {
                         
                        walk = false;
                    }
                }


                if (remainingtime <= 0)
                {
                    remainingtime = Random.Range(3.0f, 5.0f);

                    walk = true;
                 
                    nexspot = new Vector3(player.transform.position.x + Random.Range(-10.0f, 10.0f), player.transform.position.y + Random.Range(-10.0f, 10.0f), 0);
                    while (Vector3.Distance(nexspot,originalpos)>range||nexspot.x>100||nexspot.y>100||nexspot.x<0||nexspot.y<0)
                    {

                        
                        nexspot = new Vector3(player.transform.position.x + Random.Range(-10.0f, 10.0f), player.transform.position.y + Random.Range(-10.0f, 10.0f), 0);
                      
                    }





                }





                if (hp < 250)
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
                if (!notyet)
                {
                    notyet = true;

                }
                if (DistanceBO > 2&&notyet)
                {
                    
                    Vector3 backvec=originalpos - this.transform.parent.transform.position;
                    backvec = backvec.normalized;
                    gameObject.transform.parent.GetComponent<Rigidbody>().MovePosition(gameObject.transform.parent.transform.position+backvec);

                }
                else { notyet = false; }

            }



        }
        

	}

    public void instjiguang()
    {

        jiguang.GetComponent<Instlightbeam>().resetjg();

    }
    public void insttoufa()
    {
  
        if (hp > 150) {
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
    
    public void takedamage(float a)
    {
        hp -= a;
    }
}
