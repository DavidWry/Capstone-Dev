using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2slam : MonoBehaviour {
    public GameObject slamParticle;
    public ParticleSystem slamParticleSystem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!slamParticleSystem.isPlaying)
        {

            slamParticle.SetActive(false);

        }

	}


    public void slam()
    {
        slamParticle.SetActive(true);
        slamParticleSystem.Play();

    }
}
