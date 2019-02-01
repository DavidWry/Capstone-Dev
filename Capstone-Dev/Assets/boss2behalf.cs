﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2behalf : MonoBehaviour {
    GameObject boss2Sprite;
    Animator boss2Anim;
    int breathState = 0;
    float idleTime = 0;
    int hp = 500;
    // Use this for initialization
    void Start () {
        boss2Sprite = gameObject.transform.GetChild(0).gameObject;
        boss2Anim = boss2Sprite.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update() {
        int p = (int)Random.Range(0, 6);
        if (hp <= 375 && breathState == 0&& boss2Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "row1")
        {
            boss2Anim.SetInteger("state", 9);
            breathState++;
        }
        else if (breathState == 1 && hp <= 250&& boss2Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "row1")
        {
            boss2Anim.SetInteger("state", 9);
            breathState++;
        }
        else if (hp <= 0)
        {
            boss2Anim.SetInteger("state", 10);
        }
        else
        {
            if (boss2Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "row1")
            {

                idleTime += Time.deltaTime;
                if (idleTime >= 2)
                {
                    boss2Anim.SetInteger("state", p);
                    idleTime = -1;
                }

            }
            else { idleTime = 0; boss2Anim.SetInteger("state", -1); }


        }

    }

}