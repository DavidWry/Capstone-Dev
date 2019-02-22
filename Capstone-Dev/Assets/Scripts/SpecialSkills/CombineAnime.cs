using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineAnime : MonoBehaviour {

    private GameObject PlayerObj;
    private Animator animator;

    // Use this for initialization
    void Start () {
		PlayerObj = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerObj)
        {
            Shoot_New shoot = PlayerObj.GetComponent<Shoot_New>();
            if (!shoot.CombineOn && (shoot.SkillReady || shoot.GoldenFinger) && Input.GetButton("YButton"))
            {
                animator.Play("Combine");
            }
            else
            {
                animator.Play("Empty");
            }
        }
    }
}
