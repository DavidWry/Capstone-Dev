using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        if (player.transform.position.x < transform.position.x)
            scale.x *= -1;
        transform.localScale = scale;
    }
}
