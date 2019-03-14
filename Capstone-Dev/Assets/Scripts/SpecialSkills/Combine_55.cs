using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Combine_55 : MonoBehaviour {
    Shoot_New shoot;
    GameObject flame;

	// Use this for initialization
	void Start () {
        shoot = transform.parent.GetComponent<Shoot_New>();
        flame = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (shoot.CombineTag_55 && shoot.CombineOn)
        {
            flame.SetActive(true);
        }
        else
        {
            flame.SetActive(false);
        }
	}
}
