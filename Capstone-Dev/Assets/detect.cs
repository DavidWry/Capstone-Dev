using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect : MonoBehaviour {
    float distance = 50;
    GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < distance)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (player.transform.position.x > transform.position.x)
                scale.x *= -1;
            transform.localScale = scale;
        }

    }
}
