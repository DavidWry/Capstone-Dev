using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fsmandhp : MonoBehaviour {

    public float hp = 100;
    public GameObject jiguang;
    public GameObject toufazuo;
    public GameObject toufayou;
    private GameObject a;
    private float DistanceBP;
    private float DistanceBO;
    private GameObject player;
    public float range=100;
    private Vector3 originalpos;
    public bool supposetomove=false;
    private int yuancount=0;
   private Animator anim;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        a = gameObject;
        anim = a.GetComponent<Animator>();
        originalpos = gameObject.transform.parent.transform.position;
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
           
            print(DistanceBP);
            if (DistanceBP < range && DistanceBO < range)
            { 
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
         
                
                }
        else
        {
            toufazuo.SetActive(true);
            toufayou.SetActive(true);


        }

    }
    

}
