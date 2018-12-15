using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverSound : MonoBehaviour {

    AudioSource audioSource;
    [SerializeField]
    float randVolume;
    [SerializeField]
    float randPitch;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        randVolume = Random.Range(0.6f, 1.0f);
        randPitch = Random.Range(0.6f, 1.4f);
        audioSource.pitch = randPitch;
        audioSource.volume = randVolume;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
