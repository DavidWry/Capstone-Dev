using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    Transform Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Player)
            transform.position = new Vector3(Player.position.x, Player.position.y, transform.position.z);
	}
}
