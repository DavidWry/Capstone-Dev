using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UI_SkillAmmo : MonoBehaviour {

    Text AmmoNum;
    Shoot_New playerShoot;

	// Use this for initialization
	void Start () {
        AmmoNum = GetComponent<Text>();
		playerShoot = GameObject.FindWithTag("Player").GetComponent<Shoot_New>();
    }
	
	// Update is called once per frame
	void Update () {
		if (playerShoot.CombineOn)
        {
            AmmoNum.enabled = true;
            AmmoNum.text = playerShoot.currentAmmo.ToString();
        }
        else
        {
            AmmoNum.enabled = false;
        }
	}
}
