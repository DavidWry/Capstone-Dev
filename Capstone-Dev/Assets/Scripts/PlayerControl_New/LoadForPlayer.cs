using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class LoadForPlayer : MonoBehaviour {

    Player_New player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player_New>();
        player.LoadPlayerData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
