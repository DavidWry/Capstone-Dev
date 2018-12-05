using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particlebg : MonoBehaviour {

    public ParticleSystem ParticleCons;
    public ParticleSystem ParticleDes;

 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         
        if (Input.GetKeyDown(KeyCode.B))
        {
            ParticleCons.Stop();

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ParticleCons.Play();
        }

    }


    public void OnParticleSystemStopped()
    {
       

    }
}
