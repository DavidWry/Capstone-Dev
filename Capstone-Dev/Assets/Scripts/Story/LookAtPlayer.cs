using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    public bool op = true;
    GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        if (player)
            if (player.transform.position.x < transform.position.x && op)
                scale.x *= -1;
        else if (player.transform.position.x > transform.position.x && !op)
                scale.x *= -1;
        transform.localScale = scale;
    }
}
