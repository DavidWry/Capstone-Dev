using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour {

    ParticleSystem particleSys;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        particleSys = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (particleSys)
            if (!particleSys.IsAlive())
                GameObject.Destroy(this.gameObject);
        if (audioSource)
            if (!audioSource.isPlaying)
                GameObject.Destroy(this.gameObject);
    }
}
