using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fsmandhp : MonoBehaviour {

    public float hp = 500;
    public GameObject jiguang;
    public GameObject toufazuo;
    public GameObject toufayou;
    public GameObject portal;
    public ParticleSystem zhaptc;
    public ParticleSystem ptc1;
    public ParticleSystem ptc2;
    public ParticleSystem ptc3;
    public ParticleSystem ptc4;
    public ParticleSystem ptc5;
    public Material mat1;
    private GameObject a;
    public float DistanceBP;
    public float DistanceBO;
    private GameObject player;
    public float range=200;
    public Vector3 originalpos;
    public bool supposetomove=false;
    private int yuancount=0;
    public float remainingtime;
   private Animator anim;
    private bool walk = false;
    bool notyet = false;
    float cs = 3;
    private Vector3 nexspot;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        a = gameObject;
        anim = a.GetComponent<Animator>();
        originalpos = gameObject.transform.parent.transform.position;
        remainingtime = Random.Range(3.0f, 5.0f);
        nexspot = new Vector3(player.transform.position.x + Random.Range(-40.0f, 40.0f), player.transform.position.y + Random.Range(-40.0f, 40.0f), 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
        {
            ptc1.Clear();
            ptc2.Clear();
            ptc3.Clear();
            ptc4.Clear();
            ptc5.Clear();
            ptc1.Stop();
            ptc2.Stop();
            ptc3.Stop();
            ptc4.Stop();
            ptc5.Stop();
            jiguang.GetComponent<Instlightbeam>().angle = 361;
            
            gameObject.GetComponent<SpriteRenderer>().material = mat1;
            anim.speed = 0;
            zhaptc.transform.position = gameObject.transform.position;
            zhaptc.Play();
            if (cs > 0)
            {
                cs -= 3*Time.deltaTime;
                gameObject.transform.parent.transform.localScale = new Vector3(cs, cs, cs);
            }
            else
            {  
                Destroy(gameObject.transform.parent.gameObject);
                portal.transform.position = gameObject.transform.position;
                portal.SetActive(true);
              
                
            }

            }
         if (!notyet)
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
                    
                    

                    if (distancebtemp>20) {
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
                 
                    nexspot = new Vector3(player.transform.position.x + Random.Range(-40.0f, 40.0f), player.transform.position.y + Random.Range(-40.0f, 40.0f), 0);
                    while (nexspot.x>990||nexspot.y<280||nexspot.x<200||nexspot.y>590)
                    {

                        
                        nexspot = new Vector3(player.transform.position.x + Random.Range(-200.0f, 200.0f), player.transform.position.y + Random.Range(-200.0f, 200.0f), 0);
                      
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
           else if (randoma == 1) {
                toufayou.SetActive(true);
            }
            else if (randoma == 2)
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
