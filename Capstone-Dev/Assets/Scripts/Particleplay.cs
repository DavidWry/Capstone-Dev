using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particleplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.eulerAngles = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (this.gameObject.GetComponent<ParticleSystem>().isStopped)
        {

            Destroy(this.gameObject);

        }
	}
}
