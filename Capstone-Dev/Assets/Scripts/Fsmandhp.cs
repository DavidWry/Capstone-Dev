using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fsmandhp : MonoBehaviour {

    public float hp = 25;
    public GameObject jiguang;
    public GameObject toufazuo;
    public GameObject toufayou;
    private GameObject a;


   private Animator anim;
	// Use this for initialization
	void Start () {
        a = gameObject;
        anim = a.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {



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

    public void instjiguang()
    {

        jiguang.GetComponent<Instlightbeam>().resetjg();

    }
    public void insttoufa()
    {
        if (hp > 30) {
            int randoma = (int)Random.Range(0, 3);
            print(randoma);
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
