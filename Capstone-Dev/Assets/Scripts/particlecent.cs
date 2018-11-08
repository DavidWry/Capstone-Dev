using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlecent : MonoBehaviour {


    public ParticleSystem ParticleCent;
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
            ParticleCent.Clear();
            ParticleCent.Stop();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ParticleCent.Play();
        }

    }


    public void OnParticleSystemStopped()
    {
        ParticleDes.Play();

    }
}
