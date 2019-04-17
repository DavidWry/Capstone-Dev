using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
public class boss2slam : MonoBehaviour {
    public GameObject slamParticle;
    public ParticleSystem slamParticleSystem;
    public bool slam1 = false;
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

    public void slamming()
    {
        if (slam1) { slam1 = false; }
        if (!slam1) { slam1 = true; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (slam1 && other.gameObject.tag == "Player")
        {
            other.GetComponent<Player_New>().HitPoint -= 20;
        }

    }
}
