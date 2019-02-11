using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anime_Door : MonoBehaviour {

    Animator anime;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        anime.SetBool("Open", true);
        anime.SetBool("Close", false);
    }

    private void OnTriggerExit(Collider other)
    {
        anime.SetBool("Close", true);
        anime.SetBool("Open", false);
    }
}
