using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_WithAnime : MonoBehaviour {

    public float lifeTime;
    float currentTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
	}
}
