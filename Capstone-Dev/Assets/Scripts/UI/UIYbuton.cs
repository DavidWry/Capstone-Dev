using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIYbuton : MonoBehaviour {

    public GameObject empty;
    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<Shoot_New>().SkillReady)
        {
            empty.SetActive(true);
        }
        else
        {
            empty.SetActive(false);
        }
	}
}
