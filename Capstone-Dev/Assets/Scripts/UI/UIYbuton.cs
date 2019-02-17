using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UIYbuton : MonoBehaviour {

    public Sprite empty;
    public Player_New player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player_New>();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
